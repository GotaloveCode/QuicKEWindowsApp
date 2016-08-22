using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client.Services
{

    public class GetMyProfileServiceProxy : ServiceProxy, IGetMyProfileServiceProxy
    {
        public GetMyProfileServiceProxy()
            : base("users/me?token=" + MFundiRuntime.LogonToken)
        {
        }

        public async Task<GetProfileResult> GetProfileAsync()
        {
            var executeResult = await this.GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {
                // get the reports...

                ProfileItem prof = new ProfileItem
                {
                    NativeId = (int)executeResult.Output["data"]["id"],
                    name = (string)executeResult.Output["data"]["name"],
                    phone = (string)executeResult.Output["data"]["phone"],
                    email = (string)executeResult.Output["data"]["email"],
                    user_type = (string)executeResult.Output["data"]["user_type"],
                    location = (string)executeResult.Output["data"]["location"],
                    photo = (string)executeResult.Output["data"]["photo"]
                };
                               

                // return...
                return new GetProfileResult(prof);
            }
            else
                return new GetProfileResult(executeResult);
        }
    }
}
