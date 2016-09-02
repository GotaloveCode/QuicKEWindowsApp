﻿using System.Threading.Tasks;

namespace QuicKE.Client
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
