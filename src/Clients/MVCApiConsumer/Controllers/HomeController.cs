using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MVCClientConsumesAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> UsersClaims()
        {
            ViewData["Message"] = "UserClaims - Try this both logged-in and logged out.";
            ViewData["TokenResponseJson"] = "";
            ViewData["APIResponseCode"] = "";
            ViewData["UsersClaimsJson"] = "";

            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "MVCApiConsumer", "MVCApiConsumer.Secret");
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "MyClaims", AuthenticationStyle.PostValues);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("MyClaims");

            if (tokenResponse.IsError)
            {
                ViewData["Message"] = tokenResponse.Error;
                Console.WriteLine(tokenResponse.Error);
            }
            else{
                ViewData["TokenResponseJson"] = tokenResponse.Json.ToString();
            }
            
            var client = new HttpClient();
            if (!tokenResponse.IsError)
            {
                client.SetBearerToken(tokenResponse.AccessToken);
            }

            var response = await client.GetAsync("http://localhost:5000/identity");
            ViewData["APIResponseCode"] = response.StatusCode.ToString();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
                ViewData["UsersClaimsJson"] = content;
            }

            return View();
        }

         public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
