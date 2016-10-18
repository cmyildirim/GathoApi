using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace GetToClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Trying Bearer Token");
            //string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImU5N2I5NDkyMzVmZGRiYzgxNGMxZGE5YmZjNWYwNTlkY2MwOTQ0ZDc3OWMxOTU3OWFhMGY1Yzc1Mjk0ODRjZDgiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE0NzY1NTc4MDksImV4cCI6MTQ3NjU2MTQwOSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwL3Jlc291cmNlcyIsImNsaWVudF9pZCI6Im12YzIiLCJzdWIiOiJlMDQ2ZmYyZjkzMWM3YjFlNDM4NjZmOTFmMzk1NTQwNWY0NTBlMjM3YTFkNDRmZTQ3NTk0MDU2YzEzNTRkZjczIiwiYXV0aF90aW1lIjoxNDc2NTU3ODAzLCJpZHAiOiJHb29nbGUiLCJzY29wZSI6WyJHZXR0b0FwaSIsIm9wZW5pZCIsInByb2ZpbGUiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiZXh0ZXJuYWwiXX0.f0QrwK7ILNVxYIEpJvv9e0bxpMOaZ71djtqOThIHggBKsxIR8o3emay4Ap9d9HRRetsXamd55yCSrYuZpzXig4I1r_k8SJTJ8AlCIaD0maAX9JMAa9wkDuwWBHOsa1PArhEf4xjEz4vJddmfr4ZSfSWzrg4JJohYIAhohVOJVuwQa_xPNZiN3AiiZbiyxIojRp2a6M5oGsfm5pkG1Y9jaZGcB3rrIZbjdieQwMbk2TVvWwdZE7QcfQIfyC_wmbeMqEzJf9rvtSFh5TVtbNh5QfaDdQ8FOgdZNXF6RFKtD4aW10YsJvlgNzTzGbaxPG2P04-vWSOtOFCitOvVHlrjuw";
            //CallApiPreToken(token);
            //Console.ReadKey();
            var disco = Discover().Result;
            var tokenRequest = RequestToken(disco);
            //
            Console.WriteLine("Can do stuff here");
            //
            var tokenResponse = tokenRequest.Result;
            CallApi(tokenResponse);
            Console.ReadKey();

            var tokenPasswordResponse = RequestTokenForPassword(disco).Result;
            CallApi(tokenPasswordResponse);
            Console.ReadKey();

        }
            
        private static async Task<TokenResponse> RequestTokenForPassword(DiscoveryResponse disco)
        {
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "GettoApi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            Console.WriteLine(tokenResponse.Json);
            return tokenResponse;
        }

        private static async Task<TokenResponse> RequestToken(DiscoveryResponse disco)
        {
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "cihangir", "bigSecret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("GettoApi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            Console.WriteLine(tokenResponse.Json);
            return tokenResponse;
        }

        private static async void CallApi(TokenResponse tokenResponse)
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(JArray.Parse(content));
        }

        private static async Task<DiscoveryResponse> Discover()
        {
            // discover endpoints from metadata
            return await DiscoveryClient.GetAsync("http://localhost:5000");
        }

        private static async void CallApiPreToken(string accessToken)
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(JArray.Parse(content));
        }
    }
}
