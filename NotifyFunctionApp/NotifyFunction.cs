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
using System.Text;

namespace NotifyFunctionApp
{
    public static class NotifyFunction
    {
        [FunctionName("SendNotice")]
        public static async Task<IActionResult> SendNotice(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string token = req.Query["token"].ToString() ?? "";
                if (token =="valid")
                {
                    return new BadRequestObjectResult("token not valid");
                }

                //log to app insight
                log.LogInformation("SendNotice starts.");

                //1. get logic app connector
                var connectorUrl = "https://prod-02.australiasou.logic.azure.com:443/workflows/9efaf65d/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=jEXeo1cOwcSeYZfc0Vgg9HcKNFyzDiRq16CUfsRJsZ0";

                //2. send notification here
                string noticeText = await new StreamReader(req.Body).ReadToEndAsync();
                var resultMessage = "";
                using (var ahttpClient = new HttpClient())
                {
                    var data4 = new StringContent(noticeText, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await ahttpClient.PostAsync(connectorUrl, data4);
                    resultMessage = response.StatusCode.ToString();
                }

                return new OkObjectResult($"SendNotice - Processed {resultMessage}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "SendNotice");    //log to app insight
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
