using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Speech2Text.Core.Models
{
    public class TranscriptTask
    {
        public TranscriptTask() { }

        public TranscriptTask(string id, string originalUrl, string language)
        {
            this.Id = id;
            this.OriginalURL = originalUrl;
            this.Language = language;
        }

        public string Id { get; set; } = string.Empty;
        public string OriginalURL { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }

    public class Transcript : TranscriptTask
	{
        public Transcript() { }

		public Transcript(TranscriptTask t) : base(t.Id, t.OriginalURL, t.Language) { }

        [JsonProperty(Order = 10)] // to be the last one
        public JArray? Data { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string? Error { get; set; }
	}
}
