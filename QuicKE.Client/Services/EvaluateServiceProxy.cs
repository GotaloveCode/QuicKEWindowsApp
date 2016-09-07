using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace QuicKE.Client
{
    public class EvaluateServiceProxy : ServiceProxy, IEvaluateServiceProxy
    {
        public EvaluateServiceProxy()
            : base("")
        {
        }

        public async Task<EvaluateResult> EvaluateAsync(double rating,string review,string TicketID)
        {
            Url = MFundiRuntime.ServiceUrlBase + "tickets/" + TicketID + "/user-feedback";

            JsonObject input = new JsonObject();
            input.Add("rating", rating);
            input.Add("review", review);

            var executeResult = await PostAsync(input);


            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];
                string message = (string)executeResult.Output["data"];

                return new EvaluateResult(message,status);
            }
            else
                return new EvaluateResult(executeResult);
        }
    }
}
