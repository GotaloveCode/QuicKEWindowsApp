using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class GetChargesServiceProxy : ServiceProxy, IGetChargesServiceProxy
    {
        public GetChargesServiceProxy()
            : base("professions/1/services") //change this to an id with time
        {
           Url = MFundiRuntime.ServiceUrlBase + "professions/1/services";
        }

        public async Task<GetServicesResult> GetServicesAsync()
        {
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
