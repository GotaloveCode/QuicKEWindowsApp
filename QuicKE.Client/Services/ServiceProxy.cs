using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
    public abstract class ServiceProxy : IServiceProxy
    {
        // the URL that the proxy connects to...
        public static string Url { get; set; }

        // private const string ApiKey = "4f41463a-dfc7-45dd-8d95-bf339f040933";
        private string token { get; set; }
        internal const string jsonstr = "{\"status\":\"error\",\"error\":{\"code\":\"no_connection\",\"message\":\"No reliable internet connection\"}}";
        JObject output = new JObject();
        protected ServiceProxy(string handler)
        {
            ServiceProxy.Url = MFundiRuntime.ServiceUrlBase + handler;
        }

        protected void ConfigureInputArgs(JsonObject data)
        {
            this.token = MFundiRuntime.LogonToken;
            // some requests need a token...//&& !this.Url.Contains("register")
            if (!string.IsNullOrEmpty(token))
            {
                data.Add("token", token);
            }


        }

        public async Task<ServiceExecuteResult> PostAsync(JsonObject input)
        {
            // set the token key...
          //  ConfigureInputArgs(input);

            // package it up as json...
            var json = input.Stringify();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // client...
            var client = new HttpClient();
            this.token = MFundiRuntime.LogonToken;
            // some requests need a token...//&& !this.Url.Contains("register")
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
           
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsync(ServiceProxy.Url, content);
            
            // load it up...
            var outputJson = await response.Content.ReadAsStringAsync();
           
            
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
               output = JObject.Parse(jsonstr);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                output = JObject.Parse(outputJson);
                string error = (string)output["message"];
                if(string.IsNullOrEmpty(error))
                    error = (string)output["error"]["message"];
                return new ServiceExecuteResult(output, error);
            }
            else
            {
                output = JObject.Parse(outputJson);
            }

         

            // did the server return an error?
            String status = (string)output["status"];  
            if (!string.IsNullOrEmpty(status) && status == "success")
                return new ServiceExecuteResult(output);
            else
            {

                string error = "";
                JToken jtoken = output["error"];

                if  (jtoken!= null)
                {
                    
                    if (output["error"]["message"] is JArray)
                    {
                        var m = output["error"]["message"];
                        List<string> errors = output["error"]["message"].Select(jv => (string)jv).ToList();
                        return new ServiceExecuteResult(output, errors);
                    }
                    else
                        error = (string)output["error"]["message"];
                }
                else
                {
                    error = (string)output["error"];
                }
                 
             
                return new ServiceExecuteResult(output, error);
            }
                

                
            }
      
        //Get methods
        public async Task<ServiceExecuteResult> GetAsync()
        {
            // client...
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(ServiceProxy.Url);

            // load it up...
            var outputJson = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                output = JObject.Parse(outputJson);
                string error = (string)output["message"];
                return new ServiceExecuteResult(output, error);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {               
                output = JObject.Parse(jsonstr);
            }
            else 
            {
                output = JObject.Parse(outputJson);
            }
            
            // did the server return an error?
            String status = (string)output["status"];
            if (!string.IsNullOrEmpty(status) && status == "success")
                return new ServiceExecuteResult(output);
            else
            {
                string error = "";


                if (output["error"]["message"] is JArray)
                {
                    var m = output["error"]["message"];
                    List<string> errors = output["error"]["message"].Select(jv => (string)jv).ToList();
                    return new ServiceExecuteResult(output, errors);
                }
                else
                    error = (string)output["error"]["message"];

                return new ServiceExecuteResult(output, error);


            }
        }
    }
    
}
