using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
    public class HireServiceProxy : ServiceProxy, IHireServiceProxy
    {
        public HireServiceProxy()
            : base("experts/hire")
        {
            Url = MFundiRuntime.ServiceUrlBase + "experts/hire";
        }

        public async Task<TaskCompleteResult> HireAsync(int ticketID)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("ticket", ticketID);

            // call...
            var executeResult = await PostAsync(input);

            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];
                string message = (string)executeResult.Output["data"];

                return new TaskCompleteResult(status,message);
            }
            else
                return new TaskCompleteResult(executeResult);
        }

    }
}
