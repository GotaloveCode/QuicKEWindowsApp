using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{

    public class NewPassServiceProxy : ServiceProxy, INewPassServiceProxy
    {
        public NewPassServiceProxy()
            : base("change-password")
        {
        }

        public async Task<NewPassResult> VerifyAsync(string phone_number,string password,string confirm_password,string code)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("phone_number", phone_number);
            input.Add("password", password);
            input.Add("confirm_password", confirm_password);
            input.Add("code", code);

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
