using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class GetExpertsServiceProxy : ServiceProxy, IGetExpertsServiceProxy
    {
        public GetExpertsServiceProxy()
            : base("experts/profession/1?location=" + HomePageViewModel.locationtext + "&token=" + MFundiRuntime.LogonToken) //change this to an id with time
        {
        }

        public async Task<GetExpertsResult> GetExpertsAsync()
        {

            var executeResult = await this.GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {

                List<ExpertItem> experts = (List<ExpertItem>)executeResult.Output["data"]
                    .Select(x => new ExpertItem
                    {
                        NativeId = (int)x["id"],
                        name = (string)x["name"],
                        id_number = (string)x["id_number"],
                        email = (string)x["email"],
                        phone = (string)x["phone"],
                        badge = (string)x["badge"],
                        profession = (string)x["profession"],
                        distance = (string)x["distance"],
                        destination = (string)x["destination"],
                        age =(int)x["age"],
                        time = (string)x["time"],
                    }).ToList();

                return new GetExpertsResult(experts);
            }
            else
                return new GetExpertsResult(executeResult);
        }
    }
}
