﻿using Flurl.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speech2Text.Core.Models
{
	public class Stats
	{
		private readonly Transcript transcript;

		public Stats(Transcript t) 
		{
			this.transcript = t;
		}

		public object GetTopics()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (transcript.Data != null)
			{
				foreach (JObject o in transcript.Data)
				{
					stringBuilder.AppendFormat("{0} ", o.GetValue("lemmatized").ToString());
				}
			}
			string text = stringBuilder.ToString();
			char[] separators = new char[] { ' ', ',', ':', ';', '?', '!', '\n', '\r', '\t' };
			var result = text.Split(separators)
								.Where(x => x.Length > 0)   
								.GroupBy(x => x)
								.Select(g => new { Word = g.Key, Freq = g.Count()})
								.Where(x => x.Freq > 1)
								.OrderByDescending(g => g.Freq)
								.ThenBy(g => g.Word)
								.ToList();
			return result;
		}
	}
}