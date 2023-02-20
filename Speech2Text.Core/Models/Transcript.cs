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

        [JsonProperty(PropertyName = "id")] // until renamed in Azure
        public string Id { get; set; } = string.Empty;
        public string OriginalURL { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }

	public class Transcript : TranscriptTask
	{
		public Transcript() { }

		public Transcript(TranscriptTask t) : base(t.Id, t.OriginalURL, t.Language) { }

		public JArray? Data { get; set; }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
