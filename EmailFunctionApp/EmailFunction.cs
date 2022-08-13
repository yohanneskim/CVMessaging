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

namespace EmailFunctionApp
{
    public static class EmailFunction
    {
        [FunctionName("SendToLogicAppGmail")]
        public static async Task<IActionResult> SendToLogicAppGmail(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                //1. verify time sensitive token, IP is already checked from API management
                string token = req.Query["token"].ToString() ?? "";
                if (token =="valid")
                {
                    return new BadRequestObjectResult("token not valid");
                }
                //2. log to app insight
                log.LogInformation("SendToLogicAppGmail starts.");

                //3. get logic app connector
                var connectorUrl = "https://prod-02.austrau.logic.azure.com:443/workflows/9e89899c7f6bd295d/trigg/invoke?api-version=2016-10-01&sp=%2Ftrigers%2Fm2Frun&sv=1.0jEXeo1cOwcSeYZfc0Vg908809FyzDiRq16CUfsRJsZ0";
              
                //4. send to gmail logic app
                string emailText = await new StreamReader(req.Body).ReadToEndAsync();
                var resultMessage = "";
                using (var ahttpClient = new HttpClient())
                {
                    //var aurl = "https://prod-02.australiasast.logic.azure.com:443/workflows/9efa989b2a2dfd9999349d69d/triggers/manual/paths/invoke?api-version=2010-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=jEXeo1cOwcS899889898989eYZfc0Vgg9HcKNsRJsZ0";
                    //connectorUrl =aurl;
                    var data4 = new StringContent(emailText, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await ahttpClient.PostAsync(connectorUrl, data4);
                    resultMessage = response.StatusCode.ToString();
                }

                return new OkObjectResult($"SendToLogicAppGmail - Processed {resultMessage}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "SendToLogicAppGmail");    //log to app insight
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
