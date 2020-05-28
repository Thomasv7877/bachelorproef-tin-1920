using Microsoft.Graph;
using Microsoft.Identity.Client;
using PA_Custom_Connector_3.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PA_Custom_Connector_3.Controllers
{
    public class FilterController : ApiController
    {
        // GET api/values/5
        public async Task<string> Get(string filter)
        {
            try
            {
                GraphServiceClient client = await MicrosoftGraphClient.GetGraphServiceClient2();
                //var resultaat = await client.Users["db4fef52-9274-49e2-846c-1f325c4b9d7c"].Drive.Root.Children.Request().GetAsync();
                var resultaat = await client.Users["db4fef52-9274-49e2-846c-1f325c4b9d7c"].Request().GetAsync();
                //var resultaat = await client.Request("https://graph.microsoft.com/v1.0/me/").GetAsync(); 

                Debug.WriteLine(resultaat.ToString());
                return resultaat.ToString();
            }
            catch (MsalUiRequiredException)
            {
                return "request fout";
                // The application does not have sufficient permissions
                // - did you declare enough app permissions in during the app creation?
                // - did the tenant admin needs to grant permissions to the application.
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                return "request fout";
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: change the scope to be as expected !
            }
        }
    }
}
