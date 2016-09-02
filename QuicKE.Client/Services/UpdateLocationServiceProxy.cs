
using System.Threading.Tasks;
using Windows.Data.Json;

namespace QuicKE.Client
{
     public class UpdateLocationServiceProxy : ServiceProxy, IUpdateLocationServiceProxy
    {
        public UpdateLocationServiceProxy()
            : base("users/me/update-location")
        {
            Url = MFundiRuntime.ServiceUrlBase + "users/me/update-location";
        }

        public async Task<UpdateLocationResult> UpdateLocationAsync(string location)
        {
            JsonObject input = new JsonObject();
            input.Add("location", location);

            var executeResult = await PostAsync(input);


            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];

                return new UpdateLocationResult(status);
            }
            else
                return new UpdateLocationResult(executeResult);
        }
    }
}
