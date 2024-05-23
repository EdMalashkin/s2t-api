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

		private object? GetLinks(string keyword)
		{
			if (transcript != null && transcript.Data != null)
			{
				var result = transcript.Data
									.Where(x => x.Value<string>("lemmatized").Split(separators).Contains(keyword))
									//.Select(l => new { Link = GetLink(l), Start = GetTimeInSec(l.Value<double>("start")) })
									//.Select(l => new { Link = GetLink(l), Text = l.Value<string>("text") })
									.Select(l => new { Start = GetTimeInSec(l.Value<double>("start")), 
														Text = l.Value<string>("text"),
														Order = GetOrderIndexes(keyword, l.Value<string>("text"), l.Value<string>("cleaned"), l.Value<string>("lemmatized"))
									})
									.ToList();
				return result;
			}
			return null;
		}

		private List<int> GetOrderIndexes(string keyword, string text, string cleaned, string lemmatized)
		{
			List<int> lemmatizedIndexes = FindIndexes(lemmatized.Split(separators), keyword);
			List<string> cleanedWords = FindWords(cleaned.Split(separators), lemmatizedIndexes);
			List<int> textIndexes = FindIndexes(text.Split(separators), cleanedWords);
			return textIndexes;
		}

		private List<string> FindWords(string[] array, List<int> indexes)
		{
			List<string> result = new List<string>();
			foreach (int index in indexes)
			{
				if (index >= 0 && index < array.Length)
				{
					result.Add(array[index]);
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of the bounds of the array.");
				}
			}
			return result;
		}

		private List<int> FindIndexes(string[] array, List<string> words)
		{
			List<int> result = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (words.Contains(array[i]))
				{
					result.Add(i);
				}
			}
			return result;
		}

		private List<int> FindIndexes(string[] array, string searchString)
		{
			List<int> indexes = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == searchString)
				{
					indexes.Add(i);
				}
			}
			return indexes;
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
