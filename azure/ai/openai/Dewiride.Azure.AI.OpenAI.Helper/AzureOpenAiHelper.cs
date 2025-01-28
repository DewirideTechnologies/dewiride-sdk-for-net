using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

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
    }
}
