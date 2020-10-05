using Azure.Storage.Blobs;
using System;
using System.Threading.Tasks;

namespace AppConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Copy items between Containers Demo!");
            Task.Run(async () => await StartContainersDemo()).Wait();
        }

        public static async Task StartContainersDemo()
        {
            string sourceBlobFileName = "Testing.zip";
            AppSettings appSettings = AppSettings.LoadAppSettings();

            //Get a cloud client for the source Storage Account
            BlobServiceClient sourceClient = Common.CreateBlobClientStorageFromSAS(appSettings.SourceSASConnectionString);

            //Get a reference for each container
            var sourceContainerReference = sourceClient.GetBlobContainerClient(appSettings.SourceContainerName);
            var destinationContainerReference = sourceClient.GetBlobContainerClient(appSettings.DestinationContainerName);

            //Get a reference for the source blob
            var sourceBlobReference = sourceContainerReference.GetBlobClient(sourceBlobFileName);
            var destinationBlobReference = destinationContainerReference.GetBlobClient(sourceBlobFileName);

            //Move the blob from the source container to the destination container
            await destinationBlobReference.StartCopyFromUriAsync(sourceBlobReference.Uri);

        }
    }
}