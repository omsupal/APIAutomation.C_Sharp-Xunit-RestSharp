namespace APIAutomation.C_Sharp_Xunit_RestSharp;

public class UnitTest1 : IClassFixture<TestFixture>
{
    public TestFixture mytestfixture;
    public UnitTest1(TestFixture mytestfixture)
    {
        this.mytestfixture = mytestfixture;
    }


    [Fact]
    public void Test1()
    {
        // Initialize OAuthTokenManager to generate OAuth header
        OAuthTokenManager tokenManager = new OAuthTokenManager();

        // Generate the OAuth header
        string oauthHeader = tokenManager.GenerateOAuthHeader(
            "POST",
            "API URL",
            "5002", // Consumer Key
            "SEC-3acde4cd-1da1-4325-8a7a-7e72a1807038", // Consumer Secret
            "APT-e6efe52d-3f23-4e84-a76d-f0157b48c7c7", // Access Token
            "" // Token Secret (if available, otherwise blank)
        );

        // Log the OAuth Header (for debugging purposes)
        Console.WriteLine($"OAuth Authorization Header: {oauthHeader}");

        // Initialize RestSharp client
        var client = new RestClient("API URL");

        // Create the request object
        var request = new RestRequest(Method.POST);

        // Add the OAuth authorization header
        request.AddHeader("Authorization", oauthHeader);
        request.AddHeader("Content-Type", "application/json");

        // Create the request body (JSON)
        var jsonBody = new
        {
            DomainName = "www.traveazy.me",
            AuthCode = "",
            DeviceSignature = "website",
            ExpiryInMinute = 722800
        };

        // Add JSON body to the request
        request.AddJsonBody(jsonBody);

        // Execute the API request
        IRestResponse response = client.Execute(request);

        // Log the response status and content
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine($"Response Content: {response.Content}");
    }

    [Fact]
    public void TestPostmanEchoWithBasicAuthAndXML()
    {
        // Base64 encode the username:password for Basic Authentication
        string basicAuthHeader = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("postman:password"));

        // Create XML request body
        string xmlRequestBody = @"<?xml version=""1.0""?>
                              <Request>
                                  <Message>Hello, Postman Echo</Message>
                              </Request>";

        // Initialize RestSharp client with the API URL
        var client = new RestClient(mytestfixture.POSTMAN_API_URL);
        var request = new RestRequest(Method.POST);

        // Add headers
        request.AddHeader("Authorization", basicAuthHeader);
        request.AddHeader("Content-Type", "application/xml");

        // Add XML body to the request
        request.AddParameter("application/xml", xmlRequestBody, ParameterType.RequestBody);

        // Execute the request
        IRestResponse response = client.Execute(request);

        // Log the response
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine($"Response Content: {response.Content}");
    }

    [Fact]
    public void TestNHTSAVehicleApiWithXMLAndModel()
    {
        // Initialize RestSharp client with the NHTSA Vehicle API URL
        var client = new RestClient(mytestfixture.NHTS_API_URL);
        var request = new RestRequest(Method.GET);

        // Execute the request
        IRestResponse response = client.Execute(request);

        // Log the response status
        Console.WriteLine($"Response Status: {response.StatusCode}");

        // Check if the response status is OK (200)
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        // Parse the XML response into the Response model
        XmlSerializer serializer = new XmlSerializer(typeof(Response));
        Response parsedResponse;

        using (StringReader reader = new StringReader(response.Content))
        {
            parsedResponse = (Response)serializer.Deserialize(reader);
        }

        // Log the parsed data from the Response model
        Console.WriteLine($"Count: {parsedResponse.Count}");
        Console.WriteLine($"Message: {parsedResponse.Message}");
        Console.WriteLine($"Search Criteria: {parsedResponse.SearchCriteria}");

        foreach (var vehicle in parsedResponse.Results)
        {
            Console.WriteLine($"Vehicle Type ID: {vehicle.VehicleTypeId}");
            Console.WriteLine($"Vehicle Type Name: {vehicle.VehicleTypeName}");
        }

        // Assert that the data is as expected
        Assert.NotNull(parsedResponse);
        Assert.Equal(2, parsedResponse.Count);
        Assert.Equal("Response returned successfully", parsedResponse.Message);
    }

    [Fact]
    public void Test_GetDataFromMongo_ReturnsCorrectDocument()
    {
        // Define the key-value pair to search in the MongoDB collection
        string key = "test";
        string value = "test";

        // Call the GetDataFromMongo method
        BsonDocument result = Mongo_Helper.GetDataFromMongo(key, value);

        // Check if the result is not null
        Assert.NotNull(result);

        // Verify that the result contains the correct data
        Assert.Equal("test", result["test"].AsString);
        Assert.Equal("test", result["test"].AsString);
        Assert.Equal("test", result["test"].AsString);
    }

}




[XmlRoot(ElementName = "Response", Namespace = "")]
public class Response
{
    [XmlElement(ElementName = "Count")]
    public int Count { get; set; }

    [XmlElement(ElementName = "Message")]
    public string Message { get; set; }

    [XmlElement(ElementName = "SearchCriteria")]
    public string SearchCriteria { get; set; }

    [XmlArray(ElementName = "Results")]
    [XmlArrayItem(ElementName = "VehicleTypesForMakeIds")]
    public List<VehicleTypesForMakeIds> Results { get; set; }
}

public class VehicleTypesForMakeIds
{
    [XmlElement(ElementName = "VehicleTypeId")]
    public int VehicleTypeId { get; set; }

    [XmlElement(ElementName = "VehicleTypeName")]
    public string VehicleTypeName { get; set; }
}

