using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.Identity.Client;

//using WebApiWithGraph.Models;
//using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PA_Custom_Connector_3.Services
{
    public static class MicrosoftGraphClient
    {
        public static async Task<GraphServiceClient> GetGraphServiceClient()
        {
            var authentication = new
            {
                Authority = "https://graph.microsoft.com",
                Directory = WebConfigurationManager.AppSettings["ida:TenantId"],
                Application = WebConfigurationManager.AppSettings["ida:ClientId"],
                ClientSecret = WebConfigurationManager.AppSettings["ida:ClientSecret"]
            };

            var app = ConfidentialClientApplicationBuilder.Create(authentication.Application)
                .WithClientSecret(authentication.ClientSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, authentication.Directory)
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var authenticationResult = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync();

            var graphServiceClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(x =>
                {
                    x.Headers.Authorization = new AuthenticationHeaderValue(
                        "Bearer", authenticationResult.AccessToken);

                    return Task.FromResult(0);
                }));
            return graphServiceClient;
        }
        public static async Task<GraphServiceClient> GetGraphServiceClient2()
        {
            var authentication = new
            {
                Authority = "https://graph.microsoft.com/",
                Directory = WebConfigurationManager.AppSettings["ida:TenantId"],
                Application = WebConfigurationManager.AppSettings["ida:ClientId"],
                ClientSecret = WebConfigurationManager.AppSettings["ida:ClientSecret"],
                GraphResourceEndPoint = "v1.0",
                Instance = WebConfigurationManager.AppSettings["ida:AADInstance"],
                Domain = WebConfigurationManager.AppSettings["ida:Domain"]
            };
            var graphAPIEndpoint = $"{authentication.Authority}{authentication.GraphResourceEndPoint}";
            var newAuth = $"{authentication.Instance}{authentication.Directory}";
            //var newAuth2 = $"{authentication.Instance}{authentication.Domain}";

            AuthenticationContext authenticationContext = new AuthenticationContext(newAuth);
            Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential clientCred 
                = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(authentication.Application, authentication.ClientSecret);
            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult authenticationResult 
                = await authenticationContext.AcquireTokenAsync(authentication.Authority, clientCred);
            var token = authenticationResult.AccessToken;
            var delegateAuthProvider = new DelegateAuthenticationProvider((requestMessage) =>{
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.ToString());
                return Task.FromResult(0);
            });

            var graphClient = new GraphServiceClient(graphAPIEndpoint, delegateAuthProvider);
            return graphClient;
        }
    }
}