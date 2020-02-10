using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Services.AppAuthentication;
using Azure;
using Microsoft.AspNetCore.Authentication;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string accessToken = req.Headers["X-MS-TOKEN-AAD-ACCESS-TOKEN"];

            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation(accessToken);
            string Endpoint = "https://demo0217.blob.core.windows.net/demo0217/blob.txt";

            // Local Debug —p
            //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://storage.azure.com/");

            
            // Blob Create
            TokenCredential tokenCredential = new TokenCredential(accessToken);
            StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
            CloudBlockBlob blob = new CloudBlockBlob(new Uri(Endpoint), storageCredentials);
            await blob.UploadTextAsync("Blob created by Azure AD authenticated user.");


            return (ActionResult)new OkObjectResult($"Blob created by Azure AD authenticated.");
        }
    }
}
