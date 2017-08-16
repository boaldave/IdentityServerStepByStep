using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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

        [Authorize]
        public async Task<IActionResult> UsersClaims()
        {
            ViewData["Message"] = "UserClaims - Try this both logged-in and logged out.";
            ViewData["TokenResponseJson"] = "";
            ViewData["APIResponseCode"] = "";
            ViewData["UsersClaimsJson"] = "";

            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "MVCApiConsumer","MVCApiConsumer.Secret", AuthenticationStyle.PostValues);
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "MyThings", AuthenticationStyle.PostValues);

            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("MyThings");

            //var tokenResponseAuthCode = await tokenClient.RequestAuthorizationCodeAsync("123", "redirectUrl", "codeVerifier", (object)("extra"), new System.Threading.CancellationToken());

            // See also:
            // tokenClient.RequestClientCredentialsAsync - Requests a token using client creds
            // tokenClient.RequestResourceOwnerPasswordAsync - Requests a token using res owner password creds.
            // tokenClient.RequestAuthorizationCodeAsync - Requests a token using authorization code.
            // tokenClient.RequestAuthorizationCodePopAsync - Requests a PoP token using an authorization code.
            // tokenClient.RequestRefreshTokenAsync
            // tokenClient.RequestRefreshTokenPopAsync
            // tokenClient.RequestAssertionAsync
            // tokenClient.RequestCustomGrantAsync - Requests a token using a custom grant.
            // tokenClient.RequestRefreshTokenAsync - Requests a token using a custom request

            // See also:
            // All the same methods on TokenClientExtensions.

            //if (tokenResponse.IsError)
            //{
            //    ViewData["Message"] = tokenResponse.Error;
            //    Console.WriteLine(tokenResponse.Error);
            //}
            //else{
            //    ViewData["TokenResponseJson"] = tokenResponse.Json.ToString();
            //}

            //var client = new HttpClient();
            //if (!tokenResponse.IsError)
            //{
            //    client.SetBearerToken(tokenResponse.AccessToken);
            //}

            // In this Hybrid Version, the token can be retrieved from the HTTP context instead of using the  DiscoveryClient and TokenClient like the previous version of this code did. The general idea is the same in both which is to get a token, use the token as part of a request to the API application, and finally display the response in a view.
            var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            // Api Endpoint
            var response = await client.GetAsync("http://localhost:5001/api/mythings");
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

        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            // it is not enough to clear the local application cookies. In addition you also need to make a roundtrip to IdentityServer to clear the central single sign-on session.
            await HttpContext.Authentication.SignOutAsync("oidc");
            // This will clear the local cookie and then redirect to IdentityServer. IdentityServer will clear its cookies and then give the user a link to return back to the MVC application.
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
