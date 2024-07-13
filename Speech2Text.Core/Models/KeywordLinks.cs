
using System.Collections.Generic;

namespace Speech2Text.Core.Models
{
	public class KeywordLinks
	{
		public int Start;
		public string Text;
		public List<int> Indexes;

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			KeywordLinks kl = (KeywordLinks)obj;
			return (Start == kl.Start) && (Text == kl.Text) && (Indexes.SequenceEqual(kl.Indexes));
		}

		//public override int GetHashCode()
		//{
		//	throw new NotImplementedException();
		//}
	}
}
