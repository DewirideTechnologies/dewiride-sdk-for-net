using Newtonsoft.Json;

namespace Dewiride.Azure.AI.OpenAI.Helper.Models
{
    public class OpenAiDataBaseRequest
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; } = new();

        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("top_p")]
        public float TopP { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("stop")]
        public object? Stop { get; set; }

        [JsonProperty("stream")]
        public bool Stream { get; set; }
    }

    public class OpenAiExtensionDataRequest : OpenAiDataBaseRequest
    {
        [JsonProperty("data_sources")]
        public List<Datasource> DataSources { get; set; } = new();
    }

    public class OpenAiDataRequest : OpenAiDataBaseRequest
    {
    }

    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }

    public class Datasource
    {
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; } = new();
    }

    public class Parameters
    {
        [JsonProperty("endpoint")]
        public string SearchEndpoint { get; set; } = string.Empty;

        [JsonProperty("index_name")]
        public string IndexName { get; set; } = string.Empty;

        [JsonProperty("semantic_configuration")]
        public string SemanticConfiguration { get; set; } = string.Empty;

        [JsonProperty("query_type")]
        public string QueryType { get; set; } = string.Empty;

        [JsonProperty("fields_mapping")]
        public FieldsMapping FieldsMapping { get; set; } = new();

        [JsonProperty("in_scope")]
        public bool InScope { get; set; }

        [JsonProperty("role_information")]
        public string RoleInformation { get; set; } = string.Empty;

        [JsonProperty("filter")]
        public object? Filter { get; set; }

        [JsonProperty("strictness")]
        public int Strictness { get; set; } = 3;

        [JsonProperty("top_n_documents")]
        public int TopNDocuments { get; set; }

        [JsonProperty("authentication")]
        public Authentication Authentication { get; set; } = new();
    }

    public class Authentication
    {
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("key")]
        public string SearchKey { get; set; } = string.Empty;
    }

    public class FieldsMapping
    {
        [JsonProperty("content_fields_separator")]
        public string ContentFieldsSeparator { get; set; } = string.Empty;

        [JsonProperty("content_fields")]
        public List<string> ContentFields { get; set; } = new();

        [JsonProperty("filepath_field")]
        public string FilePathField { get; set; } = string.Empty;

        [JsonProperty("title_field")]
        public string TitleField { get; set; } = string.Empty;

        [JsonProperty("url_field")]
        public string UrlField { get; set; } = string.Empty;

        [JsonProperty("vector_fields")]
        public List<string> VectorFields { get; set; } = new();
    }
}
