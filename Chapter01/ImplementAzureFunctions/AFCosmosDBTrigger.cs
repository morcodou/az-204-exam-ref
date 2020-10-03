using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ImplementAzureFunctions
{
    public static class AFCosmosDBTrigger
    {
        [FunctionName("AFCosmosDBTrigger")]
        public static void Run(
            [CosmosDBTrigger(
            databaseName: "databaseName",
            collectionName: "collectionName",
            ConnectionStringSetting = "AzureWebJobsStorage",
            LeaseCollectionName = "leases",CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Document> input,
            ILogger logger)
        {
            if (input != null && input.Count > 0)
            {
                logger.LogInformation("Documents modified " + input.Count);
                logger.LogInformation("First document Id " + input[0].Id);
                logger.LogInformation("Modified document: " + input[0]);
            }
        }
    }
}
