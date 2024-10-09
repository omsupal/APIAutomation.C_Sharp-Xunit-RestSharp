using Amazon.Lambda;
using Amazon.Lambda.Model;
using System;
using Amazon;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;

namespace APIAutomation.Helper
{
    class Services_Helper: IClassFixture<TestFixture>
    {
        private readonly TestFixture myTestFixture;
        RestRequest request;
        RestClient client;

        public Services_Helper(TestFixture _myFixture)
        {
            myTestFixture = _myFixture;
        }
        public double GetROE(string from, string to)
        {
            Method HTTP_METHOD = Method.GET;
            var URL = $"{myTestFixture.API_URL_FOREX}?from={from}&to={to}";
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest(HTTP_METHOD);
            string token = Hme.OAuth.TokenManger.GenerateOAuthToken(HTTP_METHOD.ToString(), URL, myTestFixture.AFFILIATE_ID, myTestFixture.APP_TOKEN, myTestFixture.APP_SECRET);
            client.AddDefaultHeader("Authorization", token);
            client.Timeout = myTestFixture.TIME_OUT;
            IRestResponse Api_Response = client.Execute(request);
            JObject validResponse = JObject.Parse(Api_Response.Content);
            double roe = validResponse["Response"][0]["Value"].ToObject<double>();
            return roe;
        }
    }
}
