namespace Speech2Text.Core
{
    public class YoutubeTranscript
    {
        string _youtubeSourceUrl;
        string _youtubeHandlerUrl;

        public YoutubeTranscript(string youtubeSourceUrl, string youtubeHandlerUrl)
        {
            _youtubeSourceUrl = youtubeSourceUrl;
            _youtubeHandlerUrl = youtubeHandlerUrl;
        }

        public string RawText()
        {

            return _youtubeSourceUrl;
        }
    }
}