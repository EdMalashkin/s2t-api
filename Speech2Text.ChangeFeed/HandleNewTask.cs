using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Speech2Text.Core.Services;

namespace Speech2Text.ChangeFeed
{
    public static class HandleNewTask
    {
        [FunctionName("HandleNewTask")]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: "db1",
                collectionName: "tasks",
                ConnectionStringSetting = "ConnectionString",
                LeaseCollectionName = "leases")]
            IReadOnlyList<Document> input,
            ILogger log)
        {
            var s = GetDBService();
            foreach (var d in input)
			{
                var json = GetJson(d);
                log.LogInformation(json);
                s.AddAsync(d.Id, json);
            }
        }

        private static ICosmosDbService<string> GetDBService()
        {
            var cosmosDbService = new CosmosDbServiceBuilder<string>("https://speech2text-cosmosdb.documents.azure.com:443/",
                "7egNq99fnAimJWSS2JHOVRizbzgQKglH51xJh4ZYnA62035a758XaPwyLcqZ8y29E1go8vnrvbILACDbQGOa7g==",
                "db1",
                "youtubeTranscripts").GetCosmosDbTaskService();
            return cosmosDbService;
        }

        private static string GetJson(Document d)
        {
            var link = d.GetPropertyValue<string>("OriginalURL");
            var lang = d.GetPropertyValue<string>("Language");
            var y = new YoutubeTranscript(link, lang);
            return y.GetJson();
        }
    }
}
