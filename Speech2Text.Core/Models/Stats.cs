using System.Text;
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

		public object GetTopics()
		{
			string text = "";
			if (transcript != null && transcript.Data != null)
			{
				text = string.Join(" ", transcript.Data.Select(j => j.Value<string>("lemmatized")));
			}
			
			var result = text.Split(separators)
								.Where(x => x.Length > 0)   
								.GroupBy(x => x)
								.Select(g => new { Word = g.Key, Freq = g.Count(), Links = GetLinks(g.Key) })
								.Where(x => x.Freq > 1)
								.OrderByDescending(g => g.Freq)
								.ThenBy(g => g.Word)
								.ToList();
			return result;
		}

		private object GetLinks(string keyword)
		{
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
									.Where(x => x.Value<string>("lemmatized").Split(separators).Contains(keyword))
									//.Select(l => new { Link = GetLink(l), Start = l.Value<double>("start") })
									.Select(l => new { Link = GetLink(l)})
									.ToList();
				return result;
			}
			return null;
		}

		private string GetLink(JToken l)
		{
			return String.Format("{0}?t={1}", transcript.OriginalURL, GetTimeInSec(l.Value<double>("start")));
		}

		private int GetTimeInSec(double time)
		{
			//var timeSpan = TimeSpan.FromSeconds(time);
			//return ((int)timeSpan.TotalSeconds);
			return ((int)Math.Floor(time));
		}
	}
}
