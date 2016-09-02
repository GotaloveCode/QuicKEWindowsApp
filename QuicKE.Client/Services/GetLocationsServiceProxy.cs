using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class GetLocationsServiceProxy : ServiceProxy, IGetLocationsServiceProxy
    {
        public GetLocationsServiceProxy()
            : base("estates") //change this to an id with time
        {
        }
        public async Task<GetLocationsResult> GetLocationsAsync()
        {            
            Url = MFundiRuntime.ServiceUrlBase + "estates";
            var executeResult = await this.GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {

                List<LocationItem> locations = executeResult.Output["data"]
                    .Select(x => new LocationItem
                    {
                        name = (string)x["name"]
                    }).ToList();

                // return...
                return new GetLocationsResult(locations);
            }
            else
                return new GetLocationsResult(executeResult);
        }
    }
}
