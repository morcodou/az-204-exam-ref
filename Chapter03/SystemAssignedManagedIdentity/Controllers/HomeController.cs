using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SystemAssignedManagedIdentity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string keyVaultName = "<PUT_YOUR_KEY_VAULT_NAME_HERE>";
            string secretName = "<PUT_YOUR_SECRET_NAME_HERE>";

            //Get a token for accessing the Key Vault.
            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            //Create a Key Vault client for accessing the items in the vault.
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = Task.Run(async () => await keyVault
                                .GetSecretAsync($"https://{keyVaultName}.vault.azure.net/secrets/{secretName}"))
                                .GetAwaiter()
                                .GetResult();

            ViewBag.KeyVaultName = keyVaultName;
            ViewBag.keyName = secretName;
            ViewBag.secret = secret.Value;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}