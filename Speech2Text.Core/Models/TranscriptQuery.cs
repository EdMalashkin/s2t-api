using System.Text;

namespace Speech2Text.Core.Models
{
	public class TranscriptQuery
	{
		Transcript _transcript;
		StringBuilder _query;
		bool isWhere = false;

		public TranscriptQuery(Transcript t)
		{
			_transcript = t;
			_query = new StringBuilder();
		}

		public override string ToString()
		{
			_query.Append("select * from c");
			if (_transcript.OriginalURL.Length > 0) Append(String.Format("c.OriginalURL= \"{0}\"", _transcript.OriginalURL));
			if (_transcript.Language.Length > 0) Append(String.Format("c.Language= \"{0}\"", _transcript.Language));
			return _query.ToString();
		}

		private void Append(string s)
		{
			if (!isWhere)
			{
				_query.Append(" where ");
				isWhere = true;
			}
			else
			{
				_query.Append(" and ");
			}
			_query.Append(s);
		}
	}
}
