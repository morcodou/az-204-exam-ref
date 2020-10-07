using System.Threading.Tasks;
using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace StorageTier
{
class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Moving blobs between Access Tiers");
            Task.Run(async () => await StartContainersDemo()).Wait();
        }

        public static async Task StartContainersDemo()
        {
            string BlobFileName = "Testing.zip";
            AppSettings appSettings = AppSettings.LoadAppSettings();

            //Get a cloud client for the  Storage Account
            BlobServiceClient blobClient = Common.CreateBlobClientStorageFromSAS(appSettings.SASConnectionString);

            //Get a reference for each container
            var containerReference = blobClient.GetBlobContainerClient(appSettings.ContainerName);

            //Get a reference for the  blob
            var blobReference = containerReference.GetBlobClient(BlobFileName);

            //Get current Access Tier
            BlobProperties blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                        $"Inferred: {blobProperties.AccessTierInferred}\t" +
                        $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}");

            //Change Access Tier to Cool
            blobReference.SetAccessTier(AccessTier.Cool);

            //Get current Access Tier
            blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                        $"Inferred: {blobProperties.AccessTierInferred}\t" +
                        $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}");

            //Change Access Tier to Archive
            blobReference.SetAccessTier(AccessTier.Archive);

            //Get current Access Tier
            blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                        $"Inferred: {blobProperties.AccessTierInferred}\t" +
                        $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}");

            //Change Access Tier to Hot
            blobReference.SetAccessTier(AccessTier.Hot);

            //Get current Access Tier
            blobProperties = await blobReference.GetPropertiesAsync();
            System.Console.WriteLine($"Access Tier: {blobProperties.AccessTier}\t" +
                        $"Inferred: {blobProperties.AccessTierInferred}\t" +
                        $"Date last Access Tier change: {blobProperties.AccessTierChangedOn}\t" +
                        $"Archive Status: {blobProperties.ArchiveStatus}" );
        }
    }
}