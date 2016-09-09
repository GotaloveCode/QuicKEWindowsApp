using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
    public class RegisterServiceProxy : ServiceProxy, IRegisterServiceProxy
    {
        public RegisterServiceProxy()
            : base("register")
        {
        }

        public async Task<RegisterResult> RegisterAsync(string fullname, string phone_number, string password, string location, string code, string email)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("name", fullname);
            input.Add("phone_number", phone_number);          
            input.Add("password", password);
            input.Add("location", location);
            input.Add("code", code);
            input.Add("email", email);


            // call...
            var executeResult = await PostAsync(input);

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
