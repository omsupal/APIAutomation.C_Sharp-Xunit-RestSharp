using Amazon.Lambda;
using Amazon.Lambda.Model;
using System;
using Amazon;
using System.Threading.Tasks;

namespace APIAutomation.Helper
{
    class AWS_Helper
    {
        public static async Task<int?> GetLambdaEnvVariableNamesAsync(string functionName, string variableName)
        {
            var config = new AmazonLambdaConfig
            {
                RegionEndpoint = RegionEndpoint.APSoutheast1
            };

            using var lambdaClient = new AmazonLambdaClient(config);
            var request = new GetFunctionConfigurationRequest
            {
                FunctionName = functionName
            };

            try
            {
                GetFunctionConfigurationResponse response = await lambdaClient.GetFunctionConfigurationAsync(request);

                if (response.Environment.Variables.TryGetValue(variableName, out string value))
                {
                    return Convert.ToInt32(value);
                }
                else
                {
                    // Return null or throw an exception to indicate the variable was not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error retrieving environment variable: {ex.Message}");
                return null;
            }
        }
    }
}
