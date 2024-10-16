using APIAutomation.Helper;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIAutomation
{
    public class TestFixture : IDisposable
    {
        public string JSONPlaceholder_POST_API_URL { get; set; }
        public string API_URL { get; set; }
        public string AFFILIATE_ID { get; set; }
        public string APP_TOKEN { get; set; }
        public string APP_SECRET { get; set; }
        public int TIME_OUT { get; set; }
        public string AccessToken { get; set; }
        public string API_URL_FOREX { get; set; }

        public TestFixture()
        {
            JSONPlaceholder_POST_API_URL = "https://jsonplaceholder.typicode.com/posts";
            if (AccessToken == null || AccessToken == "")
            {
                //AccessToken = Login.GetAccessTokenWithDomain("www.traveazy.me", "5002:APT-e6efe52d-3f23-4e84-a76d-f0157b48c7c7:SEC-3acde4cd-1da1-4325-8a7a-7e72a1807038");
            }
            // string[] config = AccessToken.Split(":");
            // AFFILIATE_ID = config[0];
            // APP_TOKEN = config[1];
            // APP_SECRET = config[2];
            TIME_OUT = 15000;
        }
        public void Dispose()
        {
            // Cleanup
        }


    }
}
