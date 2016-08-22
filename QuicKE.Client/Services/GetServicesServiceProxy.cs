using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Data.Json;
using Newtonsoft.Json.Linq;

namespace QuicKE.Client
{
    public class GetServicesServiceProxy : ServiceProxy, IGetServicesServiceProxy
    {
        public GetServicesServiceProxy()
            : base("professions/" + MFundiRuntime.ServiceTypeID + "/services?token=" + MFundiRuntime.LogonToken) //change this to an id with time
        {           
        }
        public async Task<GetServicesResult> GetServicesAsync()
        {
            ServiceProxy.Url = MFundiRuntime.ServiceUrlBase + "professions/" + MFundiRuntime.ServiceTypeID + "/services?token=" + MFundiRuntime.LogonToken;
            var executeResult = await this.GetAsync();
           
            // did it work?
            if (!(executeResult.HasErrors))
            {

                List<ServiceItem> services = (List<ServiceItem>)executeResult.Output["data"]
                    .Select(x => new ServiceItem {
                     NativeId = (int)x["id"],
                     Name = (string)x["name"],
                     Cost = (decimal)x["cost"]
                    }).ToList(); 
                    //output["error"]["message"].Select(jv => (string)jv).ToList();  

                // use JSON.NET to create the reports...
               // var services = JsonConvert.DeserializeObject<List<ServiceItem>>(jarray);

                // return...
                return new GetServicesResult(services);
            }
            else
                return new GetServicesResult(executeResult);
        }
    }
}
