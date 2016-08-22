using System;  
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<RegisterResult> RegisterAsync(string fullname, string phone_number, string email, string password)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("name", fullname);
            input.Add("phone_number", phone_number);
            input.Add("email", email);
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
