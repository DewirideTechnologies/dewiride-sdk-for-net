using Newtonsoft.Json;

namespace Dewiride.Azure.AI.OpenAI.Helper
{
    public class Rootobject
    {
        [JsonProperty("data_sources")]
        public List<DataSources> DataSources { get; set; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("top_p")]
        public float TopP { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("stop")]
        public object Stop { get; set; }

        [JsonProperty("stream")]
        public bool Stream { get; set; }
    }

    public class DataSources
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }
    }

    public class Parameters
    {
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("index_name")]
        public string IndexName { get; set; }

        [JsonProperty("semantic_configuration")]
        public string SemanticConfiguration { get; set; }

        [JsonProperty("query_type")]
        public string QueryType { get; set; }

        [JsonProperty("fields_mapping")]
        public FieldsMapping FieldsMapping { get; set; }

        [JsonProperty("in_scope")]
        public bool InScope { get; set; }

        [JsonProperty("role_information")]
        public string RoleInformation { get; set; }

        [JsonProperty("filter")]
        public object Filter { get; set; }

        [JsonProperty("strictness")]
        public int Strictness { get; set; }

        [JsonProperty("top_n_documents")]
        public int TopNDocuments { get; set; }

        [JsonProperty("authentication")]
        public Authentication Authentication { get; set; }
    }

    public class FieldsMapping
    {
        [JsonProperty("content_fields_separator")]
        public string ContentFieldsSeparator { get; set; }

        [JsonProperty("content_fields")]
        public List<string> ContentFields { get; set; }

        [JsonProperty("filepath_field")]
        public string FilepathField { get; set; }

        [JsonProperty("title_field")]
        public string TitleField { get; set; }

        [JsonProperty("url_field")]
        public string UrlField { get; set; }

        [JsonProperty("vector_fields")]
        public List<object> VectorFields { get; set; }
    }

    public class Authentication
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

}
