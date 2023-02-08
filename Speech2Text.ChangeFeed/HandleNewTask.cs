using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Speech2Text.ChangeFeed
{
    public static class HandleNewTask
    {
        [FunctionName("HandleNewTask")]
        public static void Run([CosmosDBTrigger(
            databaseName: "db1",
            collectionName: "tasks",
            ConnectionStringSetting = "ConnectionString",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
			}
			foreach (var d in input)
			{
				var link = d.GetPropertyValue<string>("OriginalURL");
				var lang = d.GetPropertyValue<string>("Language");
				var y = new YoutubeTranscript(link, lang);
				var json = y.GetJson();
				log.LogInformation(json);
			}
        }
    }
}
