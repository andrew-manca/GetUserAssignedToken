using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace FunctionApp10
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var clientId = "b1f2beba-ad79-402e-b87d-ebd7c4802342";
            var client = new HttpClient();
            var url = Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT") + "?resource=https://management.azure.com/&api-version=2019-08-01&client_id=" + clientId;
            //var url = Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT") + "?resource=https://management.azure.com/&api-version=2017-09-01";
            client.DefaultRequestHeaders.Add("secret", Environment.GetEnvironmentVariable("IDENTITY_HEADER"));
            client.DefaultRequestHeaders.Add("X-IDENTITY-HEADER", Environment.GetEnvironmentVariable("IDENTITY_HEADER"));
            log.LogInformation(client.DefaultRequestHeaders.ToString());
            var response = await client.GetAsync(url);

            log.LogInformation(response.RequestMessage.ToString());
            var conent = await response.Content.ReadAsStringAsync();
            log.LogInformation(response.StatusCode.ToString());
            log.LogInformation(conent);



            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = conent;

            return new OkObjectResult(responseMessage);
        }
    }
}
