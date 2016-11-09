using System.Threading.Tasks;
using Windows.Storage;

namespace QuicKE.Client
{
    public class LoadTicketServiceProxy : ServiceProxy, ILoadHistoryServiceProxy
    {
        public LoadTicketServiceProxy()
            : base(string.Format("tickets/{0}/history", ApplicationData.Current.LocalSettings.Values["ticketID"].ToString()))
        {
            Url = MFundiRuntime.ServiceUrlBase + string.Format("tickets/{0}/history", ApplicationData.Current.LocalSettings.Values["ticketID"].ToString());
        }
        public async Task<GetMaidResult> GetMaidAsync()
        {          
            
           var executeResult = await GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {
                     MaidItem maid = new MaidItem()
                    {
                        ticketID = (int)executeResult.Output["data"]["ticketID"],
                        id = (int)executeResult.Output["data"]["expert"]["id"],
                        name = (string)executeResult.Output["data"]["expert"]["name"],
                        email = (string)executeResult.Output["data"]["expert"]["email"],
                        id_number = (string)executeResult.Output["data"]["expert"]["id_number"],
                        phone = (string)executeResult.Output["data"]["expert"]["phone"],
                        photo = (string)executeResult.Output["data"]["expert"]["photo"],
                        age = (int)executeResult.Output["data"]["expert"]["age"],
                        badge = (string)executeResult.Output["data"]["expert"]["badge"],
                        profession = (string)executeResult.Output["data"]["expert"]["profession"],
                        location = (string)executeResult.Output["data"]["expert"]["location"],                        
                        expert_type = (string)executeResult.Output["data"]["expert_type"]
                    };

                    
                if (maid.expert_type == "Full Time")
                {
                    maid.remaining = (int)executeResult.Output["data"]["remaining"];
                    MFundiRuntime.ServiceTypeID = 2;
                }
                else
                {
                    maid.remaining = (int)executeResult.Output["data"]["remaining"];
                    MFundiRuntime.ServiceTypeID = 1;
                }


                return new GetMaidResult(maid);
            }
            else
            {
                return new GetMaidResult(executeResult);
            }
        }
    }
}

