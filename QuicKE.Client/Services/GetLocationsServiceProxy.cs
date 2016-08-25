using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class GetLocationsServiceProxy : ServiceProxy, IGetLocationsServiceProxy
    {
        public GetLocationsServiceProxy()
            : base("estates?token=" + MFundiRuntime.LogonToken) //change this to an id with time
        {
        }
        public async Task<GetLocationsResult> GetLocationsAsync()
        {
            ServiceProxy.Url = MFundiRuntime.ServiceUrlBase + "estates?token=" + MFundiRuntime.LogonToken;
            var executeResult = await this.GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {

                List<LocationItem> locations = (List<LocationItem>)executeResult.Output["data"]
                    .Select(x => new LocationItem
                    {
                        id = (int)x["id"],
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
