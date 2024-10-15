using Amazon.Lambda;
using Amazon.Lambda.Model;
using System;
using Amazon;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;

namespace APIAutomation.Helper
{
    class Services_Helper : IClassFixture<TestFixture>
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


        public void SetupRequest<T>(Method httpMethod, string affiliateId, string apiUrl, string appToken, string appSecret, string filePath, out RestClient client, out RestRequest request, out T? requestContract) where T : class
        {
            client = new RestClient(apiUrl);
            request = new RestRequest(httpMethod);

            // Generate the OAuth token
            string token = Hme.OAuth.TokenManger.GenerateOAuthToken(httpMethod.ToString(), apiUrl, affiliateId, appToken, appSecret);

            client.AddDefaultHeader("Authorization", token);
            client.Timeout = myTestFixture.TIME_OUT;

            // If filePath is provided, read the request body and deserialize it
            if (!string.IsNullOrEmpty(filePath))
            {
                var req = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, filePath));
                requestContract = JsonConvert.DeserializeObject<T>(req);
            }
            else
            {
                requestContract = null;
            }
        }

        // Usage Example with requestContract
        public void PreReqWithContract()
        {
            RequestContract? requestContract;
            SetupRequest<RequestContract>(
                Method.POST,
                myTestFixture.API_URL,
                myTestFixture.AFFILIATE_ID,
                myTestFixture.APP_TOKEN,
                myTestFixture.APP_SECRET,
                "DataFiles/transferlisting.json",
                out client,
                out request,
                out requestContract
            );
        }

        // Usage Example without requestContract
        public void PreReqWithoutContract()
        {
            SetupRequest<object>(
                Method.POST,
                myTestFixture.API_URL,
                myTestFixture.AFFILIATE_ID,
                myTestFixture.APP_TOKEN,
                myTestFixture.APP_SECRET,
                null, // No request body
                out client,
                out request,
                out _ // Ignoring the requestContract
            );
        }

    }

    //The Request Contract class is for reference,
    //You can create Model for your API Request/Response
    internal class RequestContract
    {
    }
}
