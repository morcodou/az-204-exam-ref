using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;

namespace SharedAccessSignatures
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageAccount = "az204testing";

            DateTimeOffset startTimeKey = DateTimeOffset.UtcNow;
            DateTimeOffset endTimeKey = DateTimeOffset.UtcNow.AddDays(7);
            DateTimeOffset startTimeSAS = startTimeKey;
            DateTimeOffset endTimeSAS = startTimeSAS.AddDays(1);

            Uri blobEndpointUri = new Uri($"https://{storageAccount}.blob.core.windows.net");

            var defaultCredentials = new DefaultAzureCredential(true);

            BlobServiceClient blobClient = new BlobServiceClient(blobEndpointUri,
                                           defaultCredentials);

            //Get the key. We are going to use this key for creating the SAS
            UserDelegationKey key = blobClient.GetUserDelegationKey(startTimeKey,
            endTimeKey);

            System.Console.WriteLine($"User Key Starts on: {key.SignedStartsOn}");
            System.Console.WriteLine($"User Key Expires on: {key.SignedExpiresOn}");
            System.Console.WriteLine($"User Key Service: {key.SignedService}");
            System.Console.WriteLine($"User Key Version: {key.SignedVersion}");

            //We need to use the BlobSasBuilder for creating the SAS
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                StartsOn = startTimeSAS,
                ExpiresOn = endTimeSAS
            };

            //We set the permissions Create, List, Add, Read, and Write
            blobSasBuilder.SetPermissions("clarw");

            string sasToken = blobSasBuilder.ToSasQueryParameters
            (key, storageAccount).ToString();

            System.Console.WriteLine($"SAS Token: {sasToken}");
        }
    }
}


