using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{

    public class ConfirmPaymentServiceProxy : ServiceProxy, IConfirmPaymentServiceProxy
    {
        public ConfirmPaymentServiceProxy()
            : base("confirm-payment")
        {
        }

        public async Task<ConfirmPaymentResult> ConfirmPaymentAsync(string Code)
        {
            
            JsonObject input = new JsonObject();
            input.Add("code", Code);

            var executeResult = await PostAsync(input);


            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];              

                return new ConfirmPaymentResult(status);
            }
            else
                return new ConfirmPaymentResult(executeResult);
        }
    }
}
