using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Speech2Text.Api.Models
{
    public class Transcript
    {
        [JsonProperty(PropertyName = "id")] // until renamed in Azure
        public string Id { get; set; } = string.Empty;
        public string OriginalURL { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
