using Newtonsoft.Json.Linq;

namespace Speech2Text.Core.Models
{
	public class Stats
	{
		private readonly Transcript transcript;
		private readonly char[] separators = new char[] { ' ', ',', ':', ';', '?', '!', '\n', '\r', '\t' };

		public Stats(Transcript t) 
		{
			this.transcript = t;
		}

		public object GetTopics(int minfreq)
		{
			string text = "";
			if (transcript != null && transcript.Data != null)
			{
				text = string.Join(" ", transcript.Data.Select(j => j.Value<string>("lemmatized")));

				var result = text.Split(separators)
					.Where(x => x.Length > 0)
					.GroupBy(x => x)
					.Select(g => new { Word = g.Key, Freq = g.Count(), Links = GetLinks(g.Key) })
					.Where(x => x.Freq >= minfreq)
					.OrderByDescending(g => g.Freq)
					.ThenBy(g => g.Word)
					.ToList();
				return new { Url = transcript.OriginalURL, Stats = result };
			}
			else throw new Exception("No data");
		}

		private object GetLinks(string keyword)
		{
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
									.Where(x => x.Value<string>("lemmatized").Split(separators).Contains(keyword))
									//.Select(l => new { Link = GetLink(l), Start = GetTimeInSec(l.Value<double>("start")) })
									//.Select(l => new { Link = GetLink(l), Text = l.Value<string>("text") })
									.Select(l => new { Start = GetTimeInSec(l.Value<double>("start")), Text = l.Value<string>("text") })
									.ToList();
				return result;
			}
			return null;
		}

		private string GetLink(JToken l)
		{
			string delim = transcript.OriginalURL.Contains("?") ? "&" : "?";
			return String.Format("{0}{1}t={2}", transcript.OriginalURL, delim, GetTimeInSec(l.Value<double>("start")));
		}

		private int GetTimeInSec(double time)
		{
			//var timeSpan = TimeSpan.FromSeconds(time);
			//return ((int)timeSpan.TotalSeconds);
			return ((int)Math.Floor(time));
		}
	}
}
