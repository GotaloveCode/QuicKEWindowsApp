using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
    public class SignInServiceProxy : ServiceProxy, ISignInServiceProxy
    {
        public SignInServiceProxy()
            : base("auth")
        {
            Url = MFundiRuntime.ServiceUrlBase + "auth";
        }

        public async Task<RegisterResult> SignInAsync(string username, string password)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("username", username);
            input.Add("password", password);

            // call...
            var executeResult = await this.PostAsync(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string token = (string)executeResult.Output["token"];
                string status = (string)executeResult.Output["status"];
                return new RegisterResult(token,status);
            }
            else
                return new RegisterResult(executeResult);
        }
    
    }
}
