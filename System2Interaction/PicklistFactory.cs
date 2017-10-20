using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteHingeFramework.Classes;
using WhiteHingeFramework.Classes.Picking;

namespace System2Interaction
{
    public static class PicklistFactory
    {
        private const string ApiUrl = @"http://testserver.ad.whitehinge.com/testingapi";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tabletName"></param>
        /// <param name="employeeId"></param>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        public static async Task<Picklist> GetNewPicklistAsync(NewPickListType type,string tabletName, int employeeId, int warehouse)
        {
            var url =
                $"/Picking/GenerateSingleList?type={(int) type}&employeeid={employeeId}&tabletname={tabletName}&warehouse={warehouse}";
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
            ReturnObject<Picklist> returnedData = JsonConvert.DeserializeObject<ReturnObject<Picklist>>(responseText);
            return JsonConvert.DeserializeObject<Picklist>(returnedData.ReturnData.ToString());
        }

        public static async Task<bool> GenerateAllPicklists(int employeeId, int warehouse)
        {
            var url = $"/Picking/GeneratePicklists?employeeId={employeeId}&warehouse={warehouse}";
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
            ReturnObject<bool> returnedData = JsonConvert.DeserializeObject<ReturnObject<bool>>(responseText);
            return (bool) returnedData.ReturnData;
            
        }
    }

    public static class LoginFactory
    {
        private const string ApiUrl = "http://sqlserver.ad.whitehinge.com/Sys2/";
        public static async Task<NewEmployee> LogUserInAsync(int employeeId)
        {
            var url = $"SQLServer/WithUserID?logintext={employeeId}";
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
            ReturnObject<NewEmployee> returnedData = JsonConvert.DeserializeObject<ReturnObject<NewEmployee>>(responseText);
            NewEmployee returnable = JsonConvert.DeserializeObject<NewEmployee>(returnedData.ReturnData.ToString());
            return returnable;
        }
    }
}