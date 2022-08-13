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

namespace SMSFunctionApp
{
    public static class SMSFunction
    {
        [FunctionName("SendSMS")]
        public static async Task<IActionResult> SendSMS(
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
                log.LogInformation("SendSMS starts.");

                //1. get logic app connector
                var connectorUrl = "https://prod-02.aus.logic.azure.com:443/workflows/9efa3949d/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=";

                //2. send to sendgrid logic app
                // or implement the sendgrid function here with sendgrid sdk
                string smsText = await new StreamReader(req.Body).ReadToEndAsync();
                var resultMessage = "";
                using (var ahttpClient = new HttpClient())
                {
                    var data4 = new StringContent(smsText, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await ahttpClient.PostAsync(connectorUrl, data4);
                    resultMessage = response.StatusCode.ToString();
                }

                return new OkObjectResult($"SendSMS - Processed {resultMessage}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "SendSMS");    //log to app insight
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
