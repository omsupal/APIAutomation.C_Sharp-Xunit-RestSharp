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
    }
}
