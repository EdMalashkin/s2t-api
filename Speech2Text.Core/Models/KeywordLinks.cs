
using System.Collections.Generic;

namespace Speech2Text.Core.Models
{
	public class KeywordLinks
	{
		public int Start;
		public required string Text;
		public required List<int> Indexes;
	}
}
