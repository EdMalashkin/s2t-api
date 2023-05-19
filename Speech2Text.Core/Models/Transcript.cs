//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace Speech2Text.Core.Models
{
    public class TranscriptTask
    {
        [JsonConstructor]
        public TranscriptTask() { }

        public TranscriptTask(string id, string originalUrl, string language)
        {
            this.Id = id;
            this.OriginalURL = originalUrl;
            this.Language = language;
        }

        public TranscriptTask(string id, string originalUrl, string language, DateTime? processedAt)
		{
			this.Id = id;
			this.OriginalURL = originalUrl;	
			this.Language = language;
            this.ProcessedAt = processedAt;
        }

        public string Id { get; set; } = string.Empty;
        public string OriginalURL { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }

    public class Transcript : TranscriptTask
	{
        [JsonConstructor]
        public Transcript() { }

		public Transcript(TranscriptTask t) : base(t.Id, t.OriginalURL, t.Language, t.ProcessedAt) { }

        //[JsonProperty(Order = 10)] // to be the last one
        public JsonArray? Data { get; set; }

        public string? Error { get; set; }

		//public string ToJson()
		//{
		//	return JsonConvert.SerializeObject(this);
		//}
	}
}
