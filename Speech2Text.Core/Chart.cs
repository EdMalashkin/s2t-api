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
        private readonly string text;

        public Chart(string text)
        {
            this.text = text;
        }

        public async Task<Stream> GetPng()
        {
            var chart = await "https://quickchart.io/wordcloud"
            .PostJsonAsync(new { format = "png", width = "300", height = "300", fontScale = "15", scale = "linear", text = this.text })
            .ReceiveStream();
            return chart;  
        }
    }
}
