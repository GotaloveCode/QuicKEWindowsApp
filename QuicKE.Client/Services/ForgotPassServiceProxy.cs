using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{

    public class ForgotPassServiceProxy : ServiceProxy, IForgotPassServiceProxy
    {
        public ForgotPassServiceProxy ()
            : base("forgot-password")
        {
        }

        public async Task<NewPassResult> VerifyAsync(string phone_number)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("phone_number", phone_number);

            // call...
            var executeResult = await PostAsync(input);

            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];
                string message = (string)executeResult.Output["message"];

                return new NewPassResult(status,message);
            }
            else
                return new NewPassResult(executeResult);
        }

    }
}