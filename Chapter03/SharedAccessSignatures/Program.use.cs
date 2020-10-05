using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Azure;
using Azure.Identity;
using System.IO;

namespace SharedAccessSignatures
{
    class ProgramUseSAS
    {
        static void MainUse(string[] args)
        {
            string storageAccount = "az204testing";
            string containerName = "az204-blob-testing";
            string blobName = System.IO.Path.GetRandomFileName();

            DateTimeOffset startTimeKey = DateTimeOffset.UtcNow;
            DateTimeOffset endTimeKey = DateTimeOffset.UtcNow.AddDays(7);
            DateTimeOffset startTimeSAS = startTimeKey;
            DateTimeOffset endTimeSAS = startTimeSAS.AddYears(1);

            Uri blobEndpointUri = new Uri($"https://{storageAccount}.blob.core.windows.net");

            var defaultCredentials = new DefaultAzureCredential(true);

            BlobServiceClient blobClient = new BlobServiceClient(blobEndpointUri,
                                           defaultCredentials);

            //Get the key. We are going to use this key for creating the SAS
            UserDelegationKey key = blobClient.GetUserDelegationKey(startTimeKey,
            endTimeKey);

            Console.WriteLine($"User Key Starts on: {key.SignedStartsOn}");
            Console.WriteLine($"User Key Expires on: {key.SignedExpiresOn}");
            Console.WriteLine($"User Key Service: {key.SignedService}");
            Console.WriteLine($"User Key Version: {key.SignedVersion}");

            //We need to use the BlobSasBuilder for creating the SAS
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = startTimeSAS,
                ExpiresOn = endTimeSAS,
                Protocol = Azure.Storage.Sas.SasProtocol.Https
            };

            //We set the permissions Create, List, Add, Read, and Write
            blobSasBuilder.SetPermissions(BlobSasPermissions.All);

            string sasToken = blobSasBuilder.ToSasQueryParameters
           (key, storageAccount).ToString();

            Console.WriteLine($"SAS Token: {sasToken}");

            //We construct the full URI for accessing the Azure Storage Account
            UriBuilder blobUri = new UriBuilder()
            {
                Scheme = "https",
                Host = $"{storageAccount}.blob.core.windows.net",
                Path = $"{containerName}/{blobName}",
                Query = sasToken
            };

            //We create a random text file
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(blobName))
            {
                sw.Write("This is a testing blob for uploading using user delegated SAS tokens");
            }

            BlobClient testingBlob = new BlobClient(blobUri.Uri);
            testingBlob.Upload(blobName);

            //Now we download the blob again and print the content.

            Console.WriteLine($"Reading content from testing blob {blobName}");
            Console.WriteLine();

            BlobDownloadInfo downloadInfo = testingBlob.Download();

            using (StreamReader sr = new StreamReader(downloadInfo.Content, true))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Finished reading content from testing blob");

        }
    }
}
