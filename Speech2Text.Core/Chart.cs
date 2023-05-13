using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace Speech2Text.Core
{
    public class Chart
    {
        private readonly Dictionary<string, string> settings;

        public Chart(string text) : this(
            text,
            new Dictionary<string, string>{
                {"format","png"},
                {"width","300"},
                {"height","300"},
                {"fontScale","15"},
                {"scale","linear"},
                {"text",""},
            }){ }

        public Chart(string text, Dictionary<string, string> settings)
        {
            this.settings = settings;
            this.settings["text"] = text;
    }

        public async Task<Stream> GetPng()
        {
            var chart = await "https://quickchart.io/wordcloud"
            .PostJsonAsync((object)this.settings)
            .ReceiveStream();
            return chart;  
        }
    }
}
