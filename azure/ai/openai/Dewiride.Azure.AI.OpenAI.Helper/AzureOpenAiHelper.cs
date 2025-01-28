using Dewiride.Azure.AI.OpenAI.Helper.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Channels;

namespace Dewiride.Azure.AI.OpenAI.Helper
{
    public class AzureOpenAiHelper
    {
        private readonly ILogger<AzureOpenAiHelper> _logger;

        public AzureOpenAiHelper(ILogger<AzureOpenAiHelper> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse?> GetChatCompletionResponseAsync<TRequest, TResponse>(
            string apiEndpoint,
            string apiKey,
            TRequest dataRequest,
            int maxRetryAttempts = 10,
            int retryDelayMs = 1000)
        {
            using HttpClient client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };

            for (int retryCount = 0; retryCount < maxRetryAttempts; retryCount++)
            {
                try
                {
                    // Build the request
                    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint)
                    {
                        Headers = { { "api-key", apiKey } },
                        Content = new StringContent(JsonConvert.SerializeObject(dataRequest), Encoding.UTF8, "application/json")
                    };

                    // Send the request and get the response
                    using HttpResponseMessage response = await client.SendAsync(request);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Ensure response is successful and deserialize
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<TResponse>(responseBody);
                }
                catch (HttpRequestException ex) when (ex.InnerException is TaskCanceledException)
                {
                    _logger.LogError($"Timeout occurred. Retry {retryCount + 1}/{maxRetryAttempts}...");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during request. Retry {retryCount + 1}/{maxRetryAttempts}. Exception: {ex.Message}");
                }

                // Wait before retrying
                await Task.Delay(retryDelayMs);
            }

            _logger.LogError($"All retries failed for request to {apiEndpoint}");
            return default;
        }

        public async IAsyncEnumerable<string> GetChatCompletionStreamedResponseAsync<TRequest>(
            string azureOpenAiEndpoint,
            string azureOpenAiKey,
            TRequest request,
            int maxRetryAttempts = 5,
            int retryDelayMs = 1000)
        {
            var channel = Channel.CreateUnbounded<string>();

            _ = Task.Run(async () =>
            {
                try
                {
                    using HttpClient client = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };

                    for (int retryCount = 0; retryCount < maxRetryAttempts; retryCount++)
                    {
                        try
                        {
                            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, azureOpenAiEndpoint)
                            {
                                Headers = { { "api-key", azureOpenAiKey } },
                                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
                            };

                            HttpResponseMessage response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                            response.EnsureSuccessStatusCode();

                            using Stream responseStream = await response.Content.ReadAsStreamAsync();
                            using StreamReader reader = new StreamReader(responseStream);

                            while (!reader.EndOfStream)
                            {
                                string? line = await reader.ReadLineAsync();
                                if (line == null || line.StartsWith("data: [DONE]"))
                                    break;

                                if (line.StartsWith("data: "))
                                {
                                    string jsonData = line.Substring("data: ".Length);
                                    var chunk = JsonConvert.DeserializeObject<OpenAiStreamResponse>(jsonData);

                                    string? content = chunk?.Choices?.FirstOrDefault()?.Delta?.Content;
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        await channel.Writer.WriteAsync(content);
                                    }
                                }
                            }

                            channel.Writer.Complete(); // Complete the channel when done
                            return;
                        }
                        catch (HttpRequestException ex) when (ex.InnerException is TaskCanceledException)
                        {
                            _logger.LogWarning($"Timeout error occurred. Retry attempt {retryCount + 1} of {maxRetryAttempts}...");
                            await Task.Delay(retryDelayMs);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Error during streaming. Retry attempt {retryCount + 1}. Exception: {ex.Message}");
                            await Task.Delay(retryDelayMs);
                        }
                    }

                    _logger.LogError("Failed to complete streaming after maximum retry attempts.");
                    channel.Writer.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error: {ex.Message}");
                    channel.Writer.Complete(ex); // Complete the channel with an error
                }
            });

            // Read data from the channel
            while (await channel.Reader.WaitToReadAsync())
            {
                while (channel.Reader.TryRead(out var chunk))
                {
                    yield return chunk; // Return each chunk to the caller
                }
            }
        }
    }
}
