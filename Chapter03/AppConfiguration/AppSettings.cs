using Microsoft.Extensions.Configuration;
using System;

namespace AppConfiguration
{
    public class AppSettings
    {
        public string SourceSASConnectionString { get; set; }
        public string SourceAccountName { get; set; }
        public string SourceContainerName { get; set; }
        public string DestinationSASConnectionString { get; set; }
        public string DestinationAccountName { get; set; }
        public string DestinationContainerName { get; set; }

        public static AppSettings LoadAppSettings()
        {
            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString"));

            var config = builder.Build();
            AppSettings appSettings = new AppSettings();
            appSettings.SourceSASConnectionString = config["TestAZ204:StorageAccount:Source: ConnectionString"];
            appSettings.SourceAccountName = config["TestAZ204:StorageAccount:Source:AccountName"];
            appSettings.SourceContainerName = config["TestAZ204:StorageAccount:Source: ContainerName"];
            appSettings.DestinationSASConnectionString = config["TestAZ204:StorageAccount: Destination:ConnectionString"];
            appSettings.DestinationAccountName = config["TestAZ204:StorageAccount:Destination: AccountName"];
            appSettings.DestinationContainerName = config["TestAZ204:StorageAccount:Destination: ContainerName"];
            return appSettings;
        }
    }
}