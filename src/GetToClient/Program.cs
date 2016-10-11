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
    }
}
