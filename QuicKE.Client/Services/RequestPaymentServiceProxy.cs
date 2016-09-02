using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
    public class RequestPaymentServiceProxy : ServiceProxy, IRequestPaymentServiceProxy
    {
        public RequestPaymentServiceProxy()
            : base("request-payment")
        {
            Url = MFundiRuntime.ServiceUrlBase + "request-payment";
        }

        public async Task<RequestPaymentResult> RequestPaymentAsync(int serviceid)
        {
            JsonObject input = new JsonObject();
            input.Add("service", serviceid);           

            var executeResult = await PostAsync(input);


            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string data = (string)executeResult.Output["data"];
                
                return new RequestPaymentResult(data);
            }
            else
                return new RequestPaymentResult(executeResult);
        }
    }
}
