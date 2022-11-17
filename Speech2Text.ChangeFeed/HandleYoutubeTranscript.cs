using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Speech2Text.ChangeFeed
{
    public static class HandleYoutubeTranscript
    {
        [FunctionName("HandleYoutubeTranscript")]
        public static void Run([CosmosDBTrigger(
            databaseName: "db1",
            collectionName: "tasks",
            ConnectionStringSetting = "CosmosDBSettings",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("Document url " + input[0].Id);
            }
        }
    }


}
