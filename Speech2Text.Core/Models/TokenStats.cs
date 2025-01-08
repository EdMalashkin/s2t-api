using Newtonsoft.Json.Linq;

namespace Speech2Text.Core.Models
{
	public class TokenStats
	{
		private readonly Transcript transcript;

		public TokenStats(Transcript t) 
		{
			this.transcript = t;
		}

		public object GetTopics(int minfreq)
		{
			string text = "";
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
					.SelectMany(x => x["tokens"].Select(token => (string)token["lemma"]))
					.Where(x => x.Length > 0)
					.GroupBy(x => x)
					.Select(g => new { Word = g.Key.ToUpper(), Freq = g.Count(), Links = GetLinks(g.Key) })
					.Where(x => x.Freq >= minfreq)
					.OrderByDescending(g => g.Freq)
					.ThenBy(g => g.Word)
					.ToList();
				return new { Url = transcript.OriginalURL, Stats = result };
			}
			else throw new Exception("No data");
		}

		public List<KeywordLinks>? GetLinks(string keyword)
		{
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
									.SelectMany(parent => parent["tokens"]
										.Where(token => (string)token["lemma"] == keyword)
										.Select(token => new { 
																Lemma = (string)token["lemma"], 
																Text = (string)parent["text"],
																Start = GetTimeInSec((double)parent["start"]),
																Position = (int)token["start"],
																Index = (int)token["index"]
										}))
									.GroupBy(l => new { l.Text, l.Start })
									.Select(l => new KeywordLinks()
									{	Start = l.Key.Start, 
										Text = l.Key.Text,
										Indexes = l.Select(t => t.Position).ToList()
										// Positions = l.Select(t => t.Position).ToList() // Aggregate all positions into a list
									})
									.ToList();
				return result;
			}
			return null;
		}

		private int GetTimeInSec(double time)
		{
			return ((int)Math.Floor(time));
		}
	}
}
