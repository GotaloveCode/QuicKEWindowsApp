using System.Threading.Tasks;

namespace QuicKE.Client
{

     public class GetMaidsServiceProxy : ServiceProxy, IGetMaidsServiceProxy
    {
        public GetMaidsServiceProxy()
            : base("")
        {
            if (MFundiRuntime.ServiceTypeID == 1)
                Url = MFundiRuntime.ServiceUrlBase + "experts/profession/1/day?token=" + MFundiRuntime.LogonToken + "&location=" + MFundiRuntime.Location;
            else
                Url = MFundiRuntime.ServiceUrlBase + "experts/profession/1/fulltime?token=" + MFundiRuntime.LogonToken + "&location=" + MFundiRuntime.Location;
        }
        public async Task<GetMaidResult> GetMaidAsync()
        {
         
            var executeResult = await this.GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {

                MaidItem maid = new MaidItem()
                {
                    id = (int)executeResult.Output["data"]["id"],
                    name = (string)executeResult.Output["data"]["name"],
                    email = (string)executeResult.Output["data"]["email"],
                    id_number = (string)executeResult.Output["data"]["id_number"],
                    phone = (string)executeResult.Output["data"]["phone"],
                    photo = (string)executeResult.Output["data"]["photo"],
                    age = (int)executeResult.Output["data"]["age"],
                    badge = (string)executeResult.Output["data"]["badge"],
                    profession = (string)executeResult.Output["data"]["profession"],
                    location = (string)executeResult.Output["data"]["location"],
                };
                    
                    
                return new GetMaidResult(maid);
            }
            else
                return new GetMaidResult(executeResult);
        }
    }
}
