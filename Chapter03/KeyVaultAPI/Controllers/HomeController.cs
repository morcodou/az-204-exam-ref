using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KeyVaultAPI.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            string keyVaultName = "<YOUR_VAULT's_NAME>";
            string vaultBaseURL = $"https://{keyVaultName}.vault.azure.net";


            //Get a token for accessing the Key Vault.
            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            //Create a Key Vault client for accessing the items in the vault;
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            // Manage secrets in the Key Vault.
            // Create a new secret
            string secretName = "secret-az204";

            Task.Run(async () => await keyVault.SetSecretAsync(vaultBaseURL,
                                 secretName,
             "This is a secret testing value")).Wait();
            var secret = Task.Run(async () => await keyVault.GetSecretAsync
             ($"{vaultBaseURL}/secrets/{secretName}")).GetAwaiter().GetResult();
            // Update an existing secret
            Task.Run(async () => await keyVault.SetSecretAsync(vaultBaseURL,
                                 secretName,
             "Updated the secret testing value")).Wait();
            secret = Task.Run(async () => await keyVault.GetSecretAsync
                ($"{vaultBaseURL}/secrets/{secretName}")).GetAwaiter().GetResult();
            // Delete the secret
            Task.Run(async () => await keyVault.DeleteSecretAsync(vaultBaseURL,
                                 secretName)).Wait();

            // Manage certificates in the Key Vault
            string certName = "cert-az204";
            // Create a new self-signed certificate
            var policy = new CertificatePolicy
            {
                IssuerParameters = new IssuerParameters
                {
                    Name = "Self",
                },
                KeyProperties = new KeyProperties
                {
                    Exportable = true,
                    KeySize = 2048,
                    KeyType = "RSA"
                },
                SecretProperties = new SecretProperties
                {
                    ContentType = "application/x-pkcs12"
                },
                X509CertificateProperties = new X509CertificateProperties
                {
                    Subject = "CN=AZ204KEYVAULTDEMO"
                }
            };

            Task.Run(async () => await keyVault.CreateCertificateAsync(vaultBaseURL,
            certName, policy, new CertificateAttributes { Enabled = true })).Wait();
            // When you create a new certificate in the Key Vault it takes some time
            // before it's ready.
            // We added some wait time here for the sake of simplicity.
            Thread.Sleep(10000);
            var certificate = Task.Run(async () => await keyVault.GetCertificateAsync
             (vaultBaseURL, certName)).GetAwaiter().GetResult();
            // Update properties associated with the certificate.
            CertificatePolicy updatePolicy = new CertificatePolicy
            {
                X509CertificateProperties = new X509CertificateProperties
                {
                    SubjectAlternativeNames = new SubjectAlternativeNames
                    {
                        DnsNames = new[] { "az204.examref.testing" }
                    }
                }
            };


            Task.Run(async () => await keyVault.UpdateCertificatePolicyAsync(
                                 vaultBaseURL, certName, updatePolicy)).Wait();
            Task.Run(async () => await keyVault.CreateCertificateAsync(vaultBaseURL,
                                 certName)).Wait();
            Thread.Sleep(10000);


            certificate = Task.Run(async () => await keyVault.GetCertificateAsync(
                                               vaultBaseURL, certName)).
                                               GetAwaiter().GetResult();

            Task.Run(async () => await keyVault.UpdateCertificateAsync(certificate.
                                       CertificateIdentifier.Identifier, null,
                                       new CertificateAttributes
                                       {
                                           Enabled =
                                       false
                                       })).Wait();
            Thread.Sleep(10000);

            // Delete the self-signed certificate.
            Task.Run(async () => await keyVault.DeleteCertificateAsync(vaultBaseURL,
                                 certName)).Wait();

            // Manage keys in the Key Vault
            string keyName = "key-az204";
            NewKeyParameters keyParameters = new NewKeyParameters
            {
                Kty = "EC",
                CurveName = "SECP256K1",
                KeyOps = new[] { "sign", "verify" }
            };

            Task.Run(async () => await keyVault.CreateKeyAsync(vaultBaseURL, keyName,
                                 keyParameters)).Wait();
            var key = Task.Run(async () => await keyVault.GetKeyAsync(vaultBaseURL,
                                           keyName)).GetAwaiter().GetResult();

            // Update keys in the Key Vault
            Task.Run(async () => await keyVault.UpdateKeyAsync(vaultBaseURL, keyName,
                                 null, new KeyAttributes
                                 {
                                     Expires = DateTime.UtcNow.
                                 AddYears(1)
                                 })).Wait();
            key = Task.Run(async () => await keyVault.GetKeyAsync(vaultBaseURL,
                                       keyName)).GetAwaiter().GetResult();

            // Delete keys from the Key Vault
            Task.Run(async () => await keyVault.DeleteKeyAsync(vaultBaseURL, keyName)).
                                 Wait();


            return View();
        }


        public ActionResult Index0()
        {
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