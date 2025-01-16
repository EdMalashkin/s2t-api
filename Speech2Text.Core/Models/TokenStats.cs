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
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
					.SelectMany(x => x["tokens"]
						.Select(token => new { Word = (string)token["lemma"], Theme = (bool)token["theme"] }))
					.GroupBy(x => new {x.Word, x.Theme })
					.Select(g => new { Word = g.Key.Word.ToUpper(), g.Key.Theme, Freq = g.Count(), Links = GetLinks(g.Key.Word) })
					.Where(x => x.Freq >= minfreq)
					.OrderByDescending(g => g.Freq).ThenBy(g => g.Word)
					.ToList();
				return new { Url = transcript.OriginalURL, Stats = result };
			}
			else throw new Exception("No data");
		}

		public List<KeywordLinks2>? GetLinks(string keyword)
		{
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
									.SelectMany(parent => parent["tokens"]
										.Where(token => (string)token["lemma"] == keyword)
										.Select(token => new { 
																Lemma = (string)token["lemma"], 
																Text = (string)parent["text"],
																Time = GetTimeInSec((double)parent["start"]),
																Offset = (int)token["offset"]
										}))
									.GroupBy(l => new { l.Text, l.Time })
									.Select(l => new KeywordLinks2()
									{	Time = l.Key.Time, 
										Text = l.Key.Text,
										Offsets = l.Select(t => t.Offset).ToList() // Aggregate all positions into a list
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
