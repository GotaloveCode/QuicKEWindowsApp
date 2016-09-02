using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
    public class VerifyServiceProxy : ServiceProxy, IVerifyServiceProxy
    {
        public VerifyServiceProxy()
            : base("phone/verify")
        {
        }

        public async Task<VerifyResult> VerifyAsync(string phone_number)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("phone_number", phone_number);

            // call...
            var executeResult = await PostAsync(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];
                return new VerifyResult(status);
            }
            else
                return new VerifyResult(executeResult);
        }

    }
}
