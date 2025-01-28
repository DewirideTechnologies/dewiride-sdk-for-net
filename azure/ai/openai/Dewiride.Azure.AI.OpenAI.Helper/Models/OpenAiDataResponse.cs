using Newtonsoft.Json;

namespace Dewiride.Azure.AI.OpenAI.Helper.Models
{
    // Base response model for OpenAI API responses
    public class OpenAiDataBaseResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }

        [JsonProperty("system_fingerprint")]
        public string SystemFingerprint { get; set; }
    }

    // Response model for standard OpenAI data responses
    public class OpenAiDataResponse : OpenAiDataBaseResponse
    {
        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; }
    }

    // Response model for extension-specific OpenAI data responses
    public class OpenAiExtensionDataResponse : OpenAiDataBaseResponse
    {
        [JsonProperty("choices")]
        public List<ExtensionChoice> Choices { get; set; }
    }

    // Stream-specific response model
    public class OpenAiStreamResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("choices")]
        public List<StreamChoice> Choices { get; set; }
    }

    // Usage information about tokens
    public class Usage
    {
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }

    // Base choice class, shared between different response types
    public class BaseChoice
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }
    }

    // Choice for standard non-streaming responses
    public class Choice : BaseChoice
    {
        [JsonProperty("message")]
        public MessageBaseResponse Message { get; set; }
    }

    // Choice for extension-specific responses
    public class ExtensionChoice : BaseChoice
    {
        [JsonProperty("message")]
        public ExtensionMessageResponse Message { get; set; }
    }

    // Choice for streaming responses
    public class StreamChoice
    {
        [JsonProperty("delta")]
        public Delta Delta { get; set; }
    }

    // Streaming delta data
    public class Delta
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    // Base message response
    public class MessageBaseResponse
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    // Message response for extension-specific responses
    public class ExtensionMessageResponse : MessageBaseResponse
    {
        [JsonProperty("end_turn")]
        public bool EndTurn { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; }
    }

    // Context data for extension-specific responses
    public class Context
    {
        [JsonProperty("citations")]
        public List<Citation> Citations { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }
    }

    // Citation details
    public class Citation
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("filepath")]
        public string FilePath { get; set; }

        [JsonProperty("chunk_id")]
        public string ChunkId { get; set; }
    }
}
