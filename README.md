# IdentityServerStepByStep

## This is my try at following the Quick Start Steps:

This is my version of a custom Identity Server and clients created by following the recommended Setup and Overview Instructions on the Identity Server site.
http://docs.identityserver.io/en/release/quickstarts/0_overview.html

## Steps Followed:
### Basic Setup:
a. Create ASP.Net Core 1.1 App.
b. Add OpenID Connect protocol support provided by Nuget the IdentityServer package.
## Add UI for Login, Logout, Consent, and Error plus Add an API.
a. Follow instructions in IdentityServer4.Quickstart.UI.
- https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/tree/release
b. Configure the resources, clients and users.
c. Follow Instructions on adding Client Applications and ApiResources.
- The API Consumer is not a Console App, but instead is an MVC App.
- https://identityserver4.readthedocs.io/en/release/quickstarts/1_client_credentials.html
- The API created is slightly different from the samples: API Name is "MyThings", User Subject values are 1 and 2.
- Since the IdentityServer4.AccessTokenValidation NuGet package was added to the API project, the API will require a valid token be passed to the API.
- http://localhost:5001/api/MyThings - Returns an array of users (does not require Authorization)
- http://localhost:5001/api/MyThings/1 - Returns 401 (requires Authorization)
d. When you run ID4, you can access discovery at:
- http://localhost:5000/.well-known/openid-configuration
- See Comments.md for the response provided by this version of the code.

## Suggested Integrated Test Cases:
Start all 3 Apps, then in the MVCApiConsumer App, goto - http://localhost:6004/Home/UsersThings:
### Simple Logged In/Out Tests:
- When User IS NOT logged in to ID4, then the MVC GetToken call to ID4 WILL NOT return a token, and will pass the request to API without one which will result in 401 Http Response Status.
- Login at http://localhost:5000/account/login - either bob:bob or alice:alice
- When User IS logged in to ID4, then the MVC GetToken call to ID4 WILL return a token, and will pass the request to API with a valid token which will allow the request for UsersThings.
### More suggestions for tests:
- Try to connect to IdentityServer when it is not running (unavailable)
- Try to use an invalid client id or secret to request the token
- Try to ask for an invalid scope during the token request
- Try to call the API when it is not running (unavailable)
- Configure the API to require a different scope than the one in the token