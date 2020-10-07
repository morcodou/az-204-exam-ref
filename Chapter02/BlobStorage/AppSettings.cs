using Microsoft.Extensions.Configuration;

namespace BlobStorage
{
    public class AppSettings
    {
        public string SASToken { get; set; }
        public string AccountName { get; set; }
        public string ContainerName { get; set; }

        // public string SourceSASConnectionString { get; set; }
        // public string SourceAccountName { get; set; }
        // public string SourceContainerName { get; set; }
        // public string DestinationSASConnectionString { get; set; }
        // public string DestinationAccountName { get; set; }
        // public string DestinationContainerName { get; set; }

        public static AppSettings LoadAppSettings()
        {
            IConfigurationRoot configRoot = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", false)
                .Build();
            AppSettings appSettings = configRoot.Get<AppSettings>();
            return appSettings;
        }

        // public static AppSettings LoadAppSettings()
        // {
        //     IConfigurationRoot configRoot = new ConfigurationBuilder()
        //         .AddJsonFile("AppSettings.json",false)
        //         .Build();
        //     AppSettings appSettings = configRoot.Get<AppSettings>();
        //     return appSettings;
        // }
    }
}