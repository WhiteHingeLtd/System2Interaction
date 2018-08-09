using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteHingeFramework.Classes;
using WhiteHingeFramework.Classes.EmployeeData;
using WhiteHingeFramework.Classes.Orders;
using WhiteHingeFramework.Classes.Picking;
using WhiteHingeFramework.Exceptions;

namespace System2Interaction
{
    public static class PicklistFactory
    {
        private const string ApiUrl = @"http://sqlserver.ad.whitehinge.com/sys2";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tabletName"></param>
        /// <param name="employeeId"></param>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        public static async Task<Picklist> GetNewPicklistAsync(NewPickListType type, string tabletName, int employeeId,
            int warehouse)
        {
            var url =
                $"/Picking/GenerateSingleList?type={(int) type}&employeeid={employeeId}&tabletname={tabletName}&warehouse={warehouse}";

            var responseText = await RequestFactory.MakeRequest(new Uri(ApiUrl + url));

            var returnedData = JsonConvert.DeserializeObject<ReturnObject<Picklist>>(responseText);
            var returnedPicklist = JsonConvert.DeserializeObject<Picklist>(returnedData.ReturnData.ToString());
            if (!returnedData.Success)
            {
                throw JsonConvert.DeserializeObject<Exception>(returnedData.ReturnData.ToString());
            }
            return returnedPicklist;
        }

        public static async Task<bool> GenerateAllPicklists(int warehouse)
        {
            var url = $"/Picking/GeneratePicklists?warehouseEnum={warehouse}";

            var responseText = await RequestFactory.MakeRequest(new Uri(ApiUrl + url));

            var returnedData = JsonConvert.DeserializeObject<ReturnObject<bool>>(responseText);
            if (!returnedData.Success)
            {
                throw JsonConvert.DeserializeObject<Exception>(returnedData.ReturnData.ToString());
            }
            return (bool) returnedData.ReturnData;
            
        }

        public static async void StartPicklist(int employeeId,Guid picklistName)
        {
            var url = $"/Picking/StartPicklist?picklistname={picklistName}&employeeId={employeeId}";

            var responseText = await RequestFactory.MakeRequest(new Uri(ApiUrl + url));

            if (responseText.Length == 0) throw new IncorrectParametersException();
        }

        public static async void QuitPicklist(Guid picklistName, int employeeId)
        {
            var url = $"/Picking/CompletePicklist?picklistName={picklistName}&employeeId={employeeId}";

            var responseText = await RequestFactory.MakeRequest(new Uri(ApiUrl + url));

            var returnedData = JsonConvert.DeserializeObject<ReturnObject<object>>(responseText);
            if (!returnedData.Success)
            {
                throw JsonConvert.DeserializeObject<Exception>(returnedData.ReturnData.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picklistName"></param>
        /// <param name="orderId"></param>
        /// <param name="sku"></param>
        /// <param name="employeeId"></param>
        /// <param name="orderStatus">Only Prepack and CantFind are Valid for this call</param>
        /// <param name="requiredQuantity"></param>
        /// <returns></returns>
        public static async Task<string> ReportIssue(Guid picklistName, string orderId, string sku, int employeeId,NewOrderStatus orderStatus, int requiredQuantity)
        {
            var url = $"/Picking/ReportIssue?picklistName={picklistName}&orderId={orderId}&sku={sku}&employeeId={employeeId}&orderStatus={orderStatus}&requiredQuantity={requiredQuantity}";
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
            var returnedData = JsonConvert.DeserializeObject<ReturnObject<object>>(responseText);
            if (!returnedData.Success)
            {
                throw JsonConvert.DeserializeObject<Exception>(returnedData.ReturnData.ToString());
            }
            else
            {
                return returnedData.ReturnData.ToString();
            }
        }
    }

    public static class LoginFactory
    {
        private const string ApiUrl = "http://sqlserver.ad.whitehinge.com/Sys2/";
        public static async Task<NewEmployee> LogUserInAsync(int employeeId)
        {
            var url = $"SQLServer/WithUserID?logintext={employeeId}";

            var responseText = await RequestFactory.MakeRequest(new Uri(ApiUrl + url));
            
            var returnedData = JsonConvert.DeserializeObject<ReturnObject<NewEmployee>>(responseText);
            var returnable = JsonConvert.DeserializeObject<NewEmployee>(returnedData.ReturnData.ToString());
            return returnable;
        }
    }
}