### Usage Example

Here’s how to use the generic method to call the Azure OpenAI API by passing a request and handling the response:

---

#### 1. **Define Request and Response Models**

Ensure you have defined your request and response models. For example:

```csharp
public class OpenAiDataRequest : OpenAiDataBaseRequest
{
    // Additional fields, if required
}

public class OpenAiDataResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("index")]
    public int Index { get; set; }
}
```

---

#### 2. **Call the Generic API Method**

Use the `SendApiRequestAsync<TRequest, TResponse>` method to send a request to the API.

```csharp
// Define your request data
var requestData = new OpenAiDataRequest
{
    Messages = new List<Message>
    {
        new Message { Role = "system", Content = "You are a helpful assistant." },
        new Message { Role = "user", Content = "Tell me a joke." }
    },
    Temperature = 0.7f,
    TopP = 1.0f,
    MaxTokens = 100,
    Stream = false
};

// API endpoint and key
string apiEndpoint = "https://your-openai-endpoint.com/v1/completions";
string apiKey = "your-api-key-here";

// Call the method
var response = await SendApiRequestAsync<OpenAiDataRequest, OpenAiDataResponse>(apiEndpoint, apiKey, requestData);

// Handle the response
if (response != null)
{
    foreach (var choice in response.Choices)
    {
        Console.WriteLine($"Response Text: {choice.Text}");
    }
}
else
{
    Console.WriteLine("Failed to get a response from the API.");
}
```

---

#### 3. **Retry Logic**

The method automatically retries the request if a transient error (e.g., timeout) occurs. You can customize the retry count and delay using the optional parameters:

```csharp
int maxRetryAttempts = 5;
int retryDelayMs = 2000;

var response = await SendApiRequestAsync<OpenAiDataRequest, OpenAiDataResponse>(
    apiEndpoint, 
    apiKey, 
    requestData, 
    maxRetryAttempts, 
    retryDelayMs
);
```

---

#### 4. **Logging**

Ensure your `_logger` is configured to capture logs during retries or failures for better debugging.

## Company

- Dewiride Technologies Private Limited

## Repository

- [GitHub Repository](https://github.com/DewirideTechnologies/dewiride-sdk-for-net)