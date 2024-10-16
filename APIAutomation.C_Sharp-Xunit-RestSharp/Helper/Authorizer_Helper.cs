


namespace APIAutomation.Helper
{
    public class OAuthTokenManager
    {
        /// <summary>
        /// Generates an OAuth 1.0a authorization header for making authorized API requests.
        /// </summary>
        /// <param name="httpMethod">The HTTP method used for the request (e.g., GET, POST).</param>
        /// <param name="url">The URL of the API request.</param>
        /// <param name="consumerKey">The OAuth consumer key (client ID).</param>
        /// <param name="consumerSecret">The OAuth consumer secret (client secret).</param>
        /// <param name="accessToken">The OAuth access token for user authorization.</param>
        /// <param name="tokenSecret">The token secret for the access token (optional, may be empty).</param>
        /// <returns>The OAuth 1.0a Authorization header string.</returns>
        public string GenerateOAuthHeader(string httpMethod, string url, string consumerKey, string consumerSecret, string accessToken, string tokenSecret)
        {
            // OAuth parameters
            string oauth_version = "1.0";
            string oauth_signature_method = "HMAC-SHA1";
            string oauth_nonce = GenerateNonce();
            string oauth_timestamp = GenerateTimeStamp();

            // Create the base signature string
            var parameters = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", consumerKey },
                { "oauth_token", accessToken },
                { "oauth_signature_method", oauth_signature_method },
                { "oauth_timestamp", oauth_timestamp },
                { "oauth_nonce", oauth_nonce },
                { "oauth_version", oauth_version }
            };

            // Generate the base string for signature
            string baseString = GenerateBaseString(httpMethod, url, parameters);

            // Generate the OAuth signature
            string signature = GenerateSignature(baseString, consumerSecret, tokenSecret);

            // Add the signature to the parameters
            parameters.Add("oauth_signature", signature);

            // Build the Authorization header
            return GenerateAuthorizationHeader(parameters);
        }

        /// <summary>
        /// Generates a Basic Authentication header for API requests.
        /// </summary>
        /// <param name="username">The username or client ID for authentication.</param>
        /// <param name="password">The password or client secret for authentication.</param>
        /// <returns>The Basic Authorization header string.</returns>
        public string GenerateBasicAuthHeader(string username, string password)
        {
            string credentials = $"{username}:{password}";
            string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
            return $"Basic {base64Credentials}";
        }   
    
        /// <summary>
        /// Generates the base string used for the OAuth 1.0a signature calculation.
        /// </summary>
        /// <param name="httpMethod">The HTTP method (e.g., GET, POST).</param>
        /// <param name="url">The request URL.</param>
        /// <param name="parameters">The sorted dictionary of OAuth parameters.</param>
        /// <returns>The base string used to generate the OAuth signature.</returns>
        private string GenerateBaseString(string httpMethod, string url, SortedDictionary<string, string> parameters)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            foreach (var key in query.AllKeys)
            {
                parameters.Add(key, query[key]);
            }

            StringBuilder parameterString = new StringBuilder();
            foreach (var item in parameters)
            {
                parameterString.AppendFormat("{0}={1}&", item.Key, Uri.EscapeDataString(item.Value));
            }

            // Remove the last '&'
            parameterString.Length--;

            string baseString = $"{httpMethod.ToUpper()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(parameterString.ToString())}";
            return baseString;
        }

        /// <summary>
        /// Generates the OAuth 1.0a signature using HMAC-SHA1.
        /// </summary>
        /// <param name="baseString">The base string to be signed.</param>
        /// <param name="consumerSecret">The consumer secret (client secret).</param>
        /// <param name="tokenSecret">The token secret (can be empty).</param>
        /// <returns>The generated OAuth signature as a Base64 string.</returns>
        private string GenerateSignature(string baseString, string consumerSecret, string tokenSecret)
        {
            string signingKey = $"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";

            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(signingKey)))
            {
                byte[] hashBytes = hasher.ComputeHash(Encoding.ASCII.GetBytes(baseString));
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Builds the OAuth 1.0a Authorization header using the given parameters.
        /// </summary>
        /// <param name="parameters">The sorted dictionary of OAuth parameters including the signature.</param>
        /// <returns>The OAuth Authorization header string.</returns>
        private string GenerateAuthorizationHeader(SortedDictionary<string, string> parameters)
        {
            StringBuilder header = new StringBuilder("OAuth ");
            foreach (var item in parameters)
            {
                header.AppendFormat("{0}=\"{1}\", ", item.Key, Uri.EscapeDataString(item.Value));
            }
            // Remove the last ', '
            header.Length -= 2;
            return header.ToString();
        }

        /// <summary>
        /// Generates a unique nonce for the OAuth 1.0a request.
        /// A nonce is a random string that is used only once for each request to ensure uniqueness.
        /// </summary>
        /// <returns>A unique nonce string.</returns>
        private string GenerateNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        /// <summary>
        /// Generates a timestamp in seconds since the Unix epoch (January 1, 1970).
        /// The timestamp is used to indicate the time the OAuth request is created.
        /// </summary>
        /// <returns>The timestamp as a string.</returns>
        private string GenerateTimeStamp()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
        }
    }
}

