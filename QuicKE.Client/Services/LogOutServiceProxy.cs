using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client.Services
{

     public class LogOutServiceProxy : ServiceProxy, ILogOutServiceProxy
    {
        public LogOutServiceProxy()
             : base("logout")
        {
        }

        public async Task<SignOutResult> LogOutAsync()
        {
            JsonObject input = new JsonObject();
            // call...
            var executeResult = await this.PostAsync(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];
                string data = (string)executeResult.Output["data"][0];

                return new SignOutResult(data,status);
            }
            else
                return new SignOutResult(executeResult);
        }
    }
}
