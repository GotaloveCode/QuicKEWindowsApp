using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class GetChargesServiceProxy : ServiceProxy, IGetChargesServiceProxy
    {
        public GetChargesServiceProxy()
            : base("professions/1/services?token=" + MFundiRuntime.LogonToken) //change this to an id with time
        {           
        }
        public async Task<GetServicesResult> GetServicesAsync()
        {
            Url = MFundiRuntime.ServiceUrlBase + "professions/1/services?token=" + MFundiRuntime.LogonToken;
            var executeResult = await this.GetAsync();
           
            // did it work?
            if (!(executeResult.HasErrors))
            {

                IEnumerable<ServiceItem> services = (List<ServiceItem>)executeResult.Output["data"]
                    .Select(x => new ServiceItem {
                     Id = (int)x["id"],
                     Name = (string)x["name"],
                     Cost = (string)x["cost"]
                    }).ToList(); 
                 
                return new GetServicesResult(services);
            }
            else
                return new GetServicesResult(executeResult);
        }
    }
}
