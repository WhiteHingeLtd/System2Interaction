using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteHingeFramework.Classes;
using WhiteHingeFramework.Classes.Picking;

namespace System2Interaction
{
    public static class PickingFactory
    {
        private const string ApiUrl = @"http://10.20.0.153/WebApiPrecomp";
        public static async Task<PickItemInformation> PickItem(string scannedSku, string order, Guid picklistId, int employeeId,int quantityPicked, string trayName, string expectedSku)
        {
            var url = $"/Picking/PickItem?scannedSku={scannedSku}&order={order}&picklistId={picklistId}&employeeId={employeeId}&quantityPicked={quantityPicked}&trayName={trayName}&expectedSku={expectedSku}";
            
            var request = (HttpWebRequest)WebRequest.Create(new Uri(ApiUrl + url));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.ContentLength = 0;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string responseText;
            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    responseText = await new StreamReader(stream).ReadToEndAsync();
                }
            }
            ReturnObject<string> returnedData = JsonConvert.DeserializeObject<ReturnObject<string>>(responseText);
            if (returnedData.Success)
            {
                var information =
                    JsonConvert.DeserializeObject<PickItemInformation>(returnedData.ReturnData.ToString());

                return information;
            }
            else
            {
                var exception = JsonConvert.DeserializeObject<Exception>(returnedData.ReturnData.ToString());
                throw exception;
            }
        }
    }
}