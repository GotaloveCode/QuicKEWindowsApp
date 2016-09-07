using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace QuicKE.Client.Services
{
    public class TaskCompleteServiceProxy : ServiceProxy, ITaskCompleteServiceProxy
    {
        public TaskCompleteServiceProxy()
            : base(string.Format("expert/tickets/{0}/complete", ApplicationData.Current.LocalSettings.Values["TicketID"].ToString()))
        {
            
        }

        public async Task<TaskCompleteResult> TaskCompleteAsync()
        {
            Url = MFundiRuntime.ServiceUrlBase + string.Format("expert/tickets/{0}/complete", ApplicationData.Current.LocalSettings.Values["TicketID"].ToString());

            JsonObject input = new JsonObject();

            var executeResult = await PostAsync(input);


            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];
                string message = (string)executeResult.Output["message"];

                return new TaskCompleteResult(status, message);
            }
            else
                return new TaskCompleteResult(executeResult);
        }
    }
}
