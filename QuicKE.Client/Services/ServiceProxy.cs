using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Networking.Connectivity;

namespace QuicKE.Client
{
    public abstract class ServiceProxy : IServiceProxy
    {
        // the URL that the proxy connects to...
        public static string Url { get; set; }

        internal const string jsonstr = "{\"status\":\"error\",\"error\":{\"code\":\"no_connection\",\"message\":\"No reliable internet connection\"}}";
        JObject output = new JObject();
        protected ServiceProxy(string handler)
        {
            Url = MFundiRuntime.ServiceUrlBase + handler;
        }

        protected void ConfigureInputArgs(JsonObject data)
        {
            // some requests need a token...
            if (MFundiRuntime.HasLogonToken)
            {
                data.Add("token", MFundiRuntime.LogonToken);
            }


        }

        public async Task<ServiceExecuteResult> PostAsync(JsonObject input)
        {
            //no internet
            if (NetworkInformation.GetInternetConnectionProfile() == null)
                return new ServiceExecuteResult(output, "No internet connectivity");

            var json = input.Stringify();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            // some requests need a token...
            if (MFundiRuntime.HasLogonToken)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MFundiRuntime.LogonToken);

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsync(Url, content);

            string outputJson = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                output = JObject.Parse(outputJson);
                return new ServiceExecuteResult(output);
            }
            else
            {
                string error = "";

                JToken jtoken = output["error"];

                if (jtoken != null)
                {
                    if (output["error"]["message"] is JArray)
                    {
                        List<string> errors = output["error"]["message"].Select(jv => (string)jv).ToList();
                        return new ServiceExecuteResult(output, errors);
                    }
                    else
                    {
                        error = (string)output["error"]["message"];
                        return new ServiceExecuteResult(output, error);
                    }
                }
                else
                {
                    return new ServiceExecuteResult(output);
                }
            }

        }

        //Get methods
        public async Task<ServiceExecuteResult> GetAsync()
        {
            //no internet
            if (NetworkInformation.GetInternetConnectionProfile() == null)
                return new ServiceExecuteResult(output, "No internet connectivity");

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(Url);

            string outputJson = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                output = JObject.Parse(outputJson);
                return new ServiceExecuteResult(output);
            }
            else
            {
                string error = "";

                JToken jtoken = output["error"];

                if (jtoken != null)
                {
                    if (output["error"]["message"] is JArray)
                    {
                        List<string> errors = output["error"]["message"].Select(jv => (string)jv).ToList();
                        return new ServiceExecuteResult(output, errors);
                    }
                    else
                    {
                        error = (string)output["error"]["message"];
                        return new ServiceExecuteResult(output, error);
                    }
                }
                else                
                    return new ServiceExecuteResult(output);
                
            }
        }
    }

}
