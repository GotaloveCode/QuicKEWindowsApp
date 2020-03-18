using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Json;
using Windows.Networking.Connectivity;

namespace QuicKE.Client
{
    public abstract class ServiceProxy : IServiceProxy
    {
        ResourceLoader res = ResourceLoader.GetForCurrentView();
        // the URL that the proxy connects to...
        public string Url { get; set; }

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
            if (!HasInternetConnection())
                return new ServiceExecuteResult(output, res.GetString("NoInternet"));

            var json = input.Stringify();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            // some requests need a token...
            if (MFundiRuntime.HasLogonToken)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MFundiRuntime.LogonToken);
            //json response
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //no caching

            client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;

            HttpResponseMessage response = await client.PostAsync(Url, content);


            string outputJson = await response.Content.ReadAsStringAsync();



            output = JObject.Parse(outputJson);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine("POST - 200:" + Url + ": " + outputJson);
                return new ServiceExecuteResult(output);
            }
            else if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                Debug.WriteLine("POST - 405:" + Url + ": " + outputJson);

                return new ServiceExecuteResult(output, (string)output["message"]);
            }
            else
            {
                Debug.WriteLine("POST- Error: " + response.StatusCode.ToString() + ": " + Url + ": " + outputJson);
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
            if (!HasInternetConnection())
                return new ServiceExecuteResult(output, res.GetString("NoInternet"));

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // some requests need a token...
            if (MFundiRuntime.HasLogonToken)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MFundiRuntime.LogonToken);
            //no caching           
            client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.Now;

            HttpResponseMessage response = await client.GetAsync(Url);


            string outputJson = await response.Content.ReadAsStringAsync();

            output = JObject.Parse(outputJson);

            string error = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine("GET - 200:" + Url + ": " + outputJson);
                if ((string)output["status"] == "error")
                {
                    if (output["error"]["message"] is JArray)
                    {
                        List<string> errors = output["error"]["message"].Select(jv => (string)jv).ToList();
                        return new ServiceExecuteResult(output, errors);
                    }
                    else
                    {
                        return new ServiceExecuteResult(output, (string)output["error"]["message"]);
                    }

                }
                else
                {
                    return new ServiceExecuteResult(output);
                }
            }
            else if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                Debug.WriteLine("GET - 405:" + Url + ": " + outputJson);
                error = (string)output["message"];
                return new ServiceExecuteResult(output, error);
            }
            else
            {
                Debug.WriteLine("GET - Error: " + response.StatusCode.ToString() + ": " + Url + ": " + outputJson);
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


        public bool HasInternetConnection()
        {
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }

    }

}
