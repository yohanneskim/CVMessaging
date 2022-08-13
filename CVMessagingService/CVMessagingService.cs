using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CVMessagingService
{
    public class CVMessagingService
    {
        private TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
        public string teleStr = "InstrumentationKey=d6e91854-7451-416666666654444444;IngestionEndpoint=https://australia-0.in.applicationinsights.azure.com/;LiveEndpoint=https://australia.livediagnostics.monitor.azure.com/";
        private TelemetryClient telemetryClient;
        public CVMessagingService()
        {
            configuration.ConnectionString =teleStr;
            telemetryClient = new TelemetryClient(configuration);
        }
        public async Task<bool> SendGmail(string from, string to, string subject, string body, string cc, string bcc, string attachment)
        {
            try
            {
                await Task.Delay(1);

                //Byte[] bytes = File.ReadAllBytes(@"c:\aaa\test1.pdf"); // 1st attachment
                //String fileStr1 = Convert.ToBase64String(bytes);
                //bytes = File.ReadAllBytes(@"c:\aaa\test2.pdf");  // 2nd attachment
                //String fileStr2 = Convert.ToBase64String(bytes);

                //var attachList = new List<object>();
                //attachList.Add(new { ContentBytes = fileStr1, Name = "test1.pdf" });
                //attachList.Add(new { ContentBytes = fileStr2, Name = "test2.pdf" });

                //var emailStr = JsonConvert.SerializeObject(new
                //{
                //    eTo = "support@cv.com.au; help@gmail.com",
                //    eSubject = DateTime.Now.ToString() + " Test",
                //    eBody = DateTime.Now.ToString() +  " My new test!",
                //    eCC = "help@cv.com.au",
                //    eBCC = "",
                //    fileAttachments = attachList
                //}
                //);

                //var resultStr = "";
                //using (var ahttpClient = new HttpClient())
                //{
                //    var url = $"http://localhost:7207/api/SendToLogicAppGmail?token=xxxx555666";
                //    var jSonStr = new StringContent(emailStr, Encoding.UTF8, "application/json");
                //    HttpResponseMessage response1 = await ahttpClient.PostAsync(url, jSonStr);
                //    resultStr = response1.StatusCode.ToString();
                //}

                return true;
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return false;
            }

        }
    }
}
