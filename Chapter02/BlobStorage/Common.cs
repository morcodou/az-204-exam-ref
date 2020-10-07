using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;

namespace BlobStorage
{
    public class Common
    {
        public static CloudBlobClient CreateBlobClientStorageFromSAS(string SAStoken, string accountName)
        {
            CloudStorageAccount storageAccount;
            CloudBlobClient blobClient;
            try
            {
                bool useHttps = true;
                StorageCredentials storageCredentials =
                new StorageCredentials(SAStoken);
                storageAccount = new CloudStorageAccount(storageCredentials, accountName, null, useHttps);
                blobClient = storageAccount.CreateCloudBlobClient();
            }
            catch (System.Exception)
            {
                throw;
            }

            return blobClient;
        }

        // public static BlobServiceClient CreateBlobClientStorageFromSAS(string stringSASConnectionString)
        // {
        //     BlobServiceClient blobClient;
        //     try
        //     {
        //         blobClient = new BlobServiceClient(SASConnectionString);
        //     }
        //     catch (System.Exception)
        //     {
        //         throw;
        //     }

        //     return blobClient;

        // }
    }
}