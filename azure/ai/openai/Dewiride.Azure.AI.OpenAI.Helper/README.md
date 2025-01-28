# Azure OpenAI Helper

The **Azure OpenAI Helper** provides utility methods to interact with Azure OpenAI endpoints for both standard API requests and streaming responses. It includes robust retry logic and detailed logging for handling errors and timeouts.

## Features

- **Non-Streaming Responses**: Send a request to Azure OpenAI and receive a deserialized response.
- **Streaming Responses**: Stream responses from Azure OpenAI, useful for large or dynamic outputs.
- **Retry Logic**: Automatically retries requests on failure with configurable retry count and delay.
- **Logging**: Logs errors and retry attempts for easier debugging.

---

## Installation

1. Ensure your project references `Newtonsoft.Json` and `Microsoft.Extensions.Logging`.
2. Add this helper to your project as part of a service.

---

## Methods

### 1. **GetChatCompletionResponseAsync**

Send a standard request to the Azure OpenAI API and receive a deserialized response.

#### **Method Signature**
```csharp
public async Task<TResponse?> GetChatCompletionResponseAsync<TRequest, TResponse>(
    string apiEndpoint,
    string apiKey,
    TRequest dataRequest,
    int maxRetryAttempts = 10,
    int retryDelayMs = 1000
);
```

#### **Parameters**
- `apiEndpoint` (string): The URL of the Azure OpenAI endpoint.
- `apiKey` (string): Your Azure OpenAI API key.
- `dataRequest` (TRequest): The request payload.
- `maxRetryAttempts` (int): Maximum number of retries in case of failure (default: 10).
- `retryDelayMs` (int): Delay between retries in milliseconds (default: 1000).

#### **Example Usage**
```csharp
var helper = new AzureOpenAiHelper(logger);

var request = new
{
    model = "gpt-4",
    messages = new[]
    {
        new { role = "system", content = "You are a helpful assistant." },
        new { role = "user", content = "Hello, how are you?" }
    }
};

var response = await helper.GetChatCompletionResponseAsync<object, OpenAiDataResponse>(
    apiEndpoint: "https://<your-endpoint>.openai.azure.com/openai/deployments/<deployment-id>/chat/completions?api-version=2024-01-01",
    apiKey: "<your-api-key>",
    dataRequest: request
);

if (response != null)
{
    Console.WriteLine($"Response: {response.Choices.FirstOrDefault()?.Message?.Content}");
}
else
{
    Console.WriteLine("Failed to retrieve a response.");
}
```

---

### 2. **GetChatCompletionStreamedResponseAsync**

Stream the response from Azure OpenAI, processing each chunk of data as it arrives.

#### **Method Signature**
```csharp
public async Task GetChatCompletionStreamedResponseAsync<TRequest>(
    string azureOpenAiEndpoint,
    string azureOpenAiKey,
    TRequest request,
    Action<string> onMessageReceived,
    int maxRetryAttempts = 5,
    int retryDelayMs = 1000
);
```

#### **Parameters**
- `azureOpenAiEndpoint` (string): The URL of the Azure OpenAI streaming endpoint.
- `azureOpenAiKey` (string): Your Azure OpenAI API key.
- `request` (TRequest): The request payload.
- `onMessageReceived` (Action<string>): A callback to handle streamed content.
- `maxRetryAttempts` (int): Maximum number of retries in case of failure (default: 5).
- `retryDelayMs` (int): Delay between retries in milliseconds (default: 1000).

#### **Example Usage**
```csharp
var helper = new AzureOpenAiHelper(logger);

var request = new
{
    model = "gpt-4",
    messages = new[]
    {
        new { role = "system", content = "You are a helpful assistant." },
        new { role = "user", content = "Tell me a story!" }
    },
    stream = true
};

await helper.GetChatCompletionStreamedResponseAsync(
    azureOpenAiEndpoint: "https://<your-endpoint>.openai.azure.com/openai/deployments/<deployment-id>/chat/completions?api-version=2024-01-01",
    azureOpenAiKey: "<your-api-key>",
    request: request,
    onMessageReceived: message =>
    {
        Console.WriteLine($"Streamed content: {message}");
    }
);
```

---

## Logging

The `AzureOpenAiHelper` relies on `ILogger<AzureOpenAiHelper>` for logging. To use this feature, ensure you inject an appropriate logger instance into the helper.

---

## Configuration

- **Retry Configuration**: Customize `maxRetryAttempts` and `retryDelayMs` to suit your application's needs.
- **Timeout Handling**: Both methods handle timeouts gracefully using retry logic and logging warnings.

---

## Dependencies

- `Newtonsoft.Json`: For JSON serialization and deserialization.
- `Microsoft.Extensions.Logging`: For logging errors and warnings.

---

## License

This project is licensed under the MIT License. See `LICENSE` for details.

## Company

- Dewiride Technologies Private Limited

## Repository

- [GitHub Repository](https://github.com/DewirideTechnologies/dewiride-sdk-for-net)

---