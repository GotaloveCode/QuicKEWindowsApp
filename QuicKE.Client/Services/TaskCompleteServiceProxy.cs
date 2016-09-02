using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client.Services
{
    public class TaskCompleteServiceProxy : ServiceProxy, ITaskCompleteServiceProxy
    {
        public TaskCompleteServiceProxy()
            : base("")
        {
            
        }

        public async Task<TaskCompleteResult> TaskCompleteAsync(int ticketid)
        {
            Url = MFundiRuntime.ServiceUrlBase + string.Format("expert/tickets/{id}/complete", ticketid);
            

            var executeResult = await GetAsync();


            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];

                return new TaskCompleteResult(status);
            }
            else
                return new TaskCompleteResult(executeResult);
        }
    }
}
