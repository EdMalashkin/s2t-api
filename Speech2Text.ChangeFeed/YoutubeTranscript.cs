using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Speech2Text.ChangeFeed
{
	public class YoutubeTranscript
	{
		private readonly string _url;
		private readonly string _language;

		public YoutubeTranscript(string url) : this(url, "uk") { }

		public YoutubeTranscript(string url, string language)
		{
			this._url = url;
			this._language = language;
		}

		private string GetYoutubeVideoID()
		{
			var url = new Uri(_url);
			return HttpUtility.ParseQueryString(url.Query).Get("v");
		}

		public string GetJson()
		{
			var id = GetYoutubeVideoID();
			var serviceUrl = string.Format("https://youtubetranscript.azurewebsites.net/api/youtube-transcript?id={0}&lang={1}", id, _language);
			var web = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, serviceUrl);
			var response = web.SendAsync(request);
			var json = response.Result.Content.ReadAsStringAsync().Result;
			return json;
		}
	}
}
