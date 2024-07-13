using Newtonsoft.Json;
using Speech2Text.Core.Models;

namespace Speech2Text.Core.Tests
{
	[TestClass]
	public class UnitTest1
	{
		Transcript transcript;

        public UnitTest1()
        {
			// deserializing the Transcript object from UnitTest1 file 
			var fileText = File.ReadAllText("UnitTest1.json");
			if (string.IsNullOrEmpty(fileText))
			{
				throw new Exception("File is empty");
			}
			var o = JsonConvert.DeserializeObject<Transcript>(fileText);
			if (o != null)
			{
				transcript = o;
			}
			else
			{
				throw new Exception("Failed to deserialize the file");
			}
		}

		[TestMethod]
		public void TestMethod1()
		{
			List<KeywordLinks>? expected = new List<KeywordLinks> {new KeywordLinks { Start = 4, Text = "а слово1 і слова2 і ще раз Слову1", Indexes = new List<int> { 1, 7 } }};
			List<KeywordLinks>? actual = new Stats(transcript).GetLinks("слово1");
			Assert.AreEqual(expected.First(), actual?.First());
		}
	}
}