
using APIAutomation.Helper;

namespace APIAutomation.C_Sharp_Xunit_RestSharp;

public class UnitTest1
{
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
}
