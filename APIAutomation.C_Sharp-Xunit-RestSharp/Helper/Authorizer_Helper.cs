using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace APIAutomation.Helper
{
    class Authorizer_Helper
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        public Authorizer_Helper(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void Login(string username, string password)
        {
            IWebElement Usernamefield = driver.FindElement(By.Id("logonIdentifier"));
            IWebElement Passwordfield = driver.FindElement(By.Id("password"));
            IWebElement Loginbutton = driver.FindElement(By.Id("next"));
            Usernamefield.SendKeys(username);
            Passwordfield.SendKeys(password);
            Loginbutton.Click();
            Thread.Sleep(500);
        }
    }
    public static class Login
    {
        private static Authorizer_Helper login_Page;
        private static IWebDriver driver;
        static string API_URL = "AuthAPIURL";

        public static IWebDriver WEBDRIVER { get; set; }

        /// <summary>
        /// This Method will be used to setup the headless browser
        /// </summary>
        public static void SETUP()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920x1080");
            options.AddArgument("--disable-gpu");
            new DriverManager().SetUpDriver(new ChromeConfig());
            WEBDRIVER = new ChromeDriver(options);
        }

        /// <summary>
        /// This Method will be used to open the headless browser
        /// It will Navigate to Given Url
        /// return <param name="AuthCode"></param>
        /// </summary
        public static string OpenBroswer(string URL, string userName, string password)
        {
            driver.Navigate().GoToUrl(URL);
            Thread.Sleep(500);
            login_Page.Login(userName, password);
            Thread.Sleep(500);
            string ReturnUrl = driver.Url;
            string[] parts = ReturnUrl.Split('=');
            string url = parts[0];
            string AuthCode = parts[1];
            Console.WriteLine(AuthCode);
            return AuthCode;
        }

        /// <summary>
        /// Method used to get AccessToken ater Login with Username
        /// </summary>
        public static string GetAccessToken(string apptoken, string username, string password, string credentials)
        {
            SETUP();
            driver = WEBDRIVER;
            login_Page = new Authorizer_Helper(driver);
            string URL = $"LOGIN_BASE_URL?apptoken={apptoken}";
            string AuthCode = OpenBroswer(URL, username, password);
            string[] cred = credentials.Split(':');
            Method HTTP_METHOD = Method.POST;
            RestClient client = new RestClient(API_URL);
            RestRequest request = new RestRequest(HTTP_METHOD);
            string token = Hme.OAuth.TokenManger.GenerateOAuthToken(HTTP_METHOD.ToString(), API_URL, cred[0], cred[1], cred[2]);
            client.AddDefaultHeader("Authorization", token);
            //var Request = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "DataFiles/Helper", "Request_STS_API.json"));

            JObject req = new JObject();
            req["DomainName"] = "";
            req["AuthCode"] = "";
            req["DeviceSignature"] = "website";

            req["AuthCode"] = AuthCode;
            request.AddJsonBody(req);

            IRestResponse response = client.Execute(request);
            JObject validResponse = JObject.Parse(response.Content);
            string AccessToken = validResponse.GetValue("Response").SelectToken("AccessToken").ToString();
            WEBDRIVER.Quit();
            return AccessToken;
        }

        /// <summary>
        /// Method used to get AccessToken using Domain Name
        /// </summary>
        public static string GetAccessTokenWithDomain(string DomainName, string credentials)
        {
            string[] cred = credentials.Split(':');
            Method HTTP_METHOD = Method.POST;
            RestClient client = new RestClient(API_URL);
            RestRequest request = new RestRequest(HTTP_METHOD);
            string token = Hme.OAuth.TokenManger.GenerateOAuthToken(HTTP_METHOD.ToString(), API_URL, cred[0], cred[1], cred[2]);
            client.AddDefaultHeader("Authorization", token);
            //var Request = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "DataFiles/Helper", "Request_STS_API.json"));

            var req = new
            {
                AuthCode = "",
                DeviceSignature = "website",
                DomainName = DomainName
            };
            request.AddJsonBody(req);

            IRestResponse response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving access token: {response.StatusCode} - {response.ErrorMessage}");
            }

            JObject validResponse = JObject.Parse(response.Content);
            string AccessToken = validResponse.GetValue("Response").SelectToken("AccessToken").ToString();

            return AccessToken;
        }

        /// <summary>
        /// Method used to get AccessToken without Auth and domain.
        /// </summary>
        public static string GetAccessTokenNoAuth(string credentials)
        {
            string[] cred = credentials.Split(':');
            Method HTTP_METHOD = Method.POST;
            RestClient client = new RestClient(API_URL);
            RestRequest request = new RestRequest(HTTP_METHOD);
            string token = Hme.OAuth.TokenManger.GenerateOAuthToken(HTTP_METHOD.ToString(), API_URL, cred[0], cred[1], cred[2]);
            client.AddDefaultHeader("Authorization", token);
            //var Request = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "DataFiles/Helper", "Request_STS_API.json"));

            JObject req = new JObject();
            req["DomainName"] = "";
            req["AuthCode"] = "";
            req["DeviceSignature"] = "website";
            request.AddJsonBody(req);

            IRestResponse response = client.Execute(request);
            JObject validResponse = JObject.Parse(response.Content);
            string AccessToken = validResponse.GetValue("Response").SelectToken("AccessToken").ToString();

            return AccessToken;
        }
    }
}
