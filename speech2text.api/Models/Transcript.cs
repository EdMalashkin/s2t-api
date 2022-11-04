namespace Speech2Text.Api.Models
{
    public class Transcript
    {
        public int ID { get; set; }
        public string OriginalURL { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
