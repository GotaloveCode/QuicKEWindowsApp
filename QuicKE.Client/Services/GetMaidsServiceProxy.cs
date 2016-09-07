using System.Threading.Tasks;
using Windows.Storage;

namespace QuicKE.Client
{

    public class GetMaidsServiceProxy : ServiceProxy, IGetMaidsServiceProxy
    {
        public GetMaidsServiceProxy()
            : base("experts/profession/1/day?code=" + ApplicationData.Current.LocalSettings.Values["Code"].ToString() + "&token=" + MFundiRuntime.LogonToken)
        {
        }
        public async Task<GetMaidResult> GetMaidAsync()
        {
            if (MFundiRuntime.ServiceTypeID == 1)
                Url = MFundiRuntime.ServiceUrlBase + "experts/profession/1/day?code=" + ApplicationData.Current.LocalSettings.Values["Code"].ToString() + "&token=" + MFundiRuntime.LogonToken;
            else
                Url = MFundiRuntime.ServiceUrlBase + "experts/profession/1/fulltime?code=" + ApplicationData.Current.LocalSettings.Values["Code"].ToString() + "&token=" + MFundiRuntime.LogonToken;

            var executeResult = await this.GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {
                if (MFundiRuntime.ServiceTypeID == 2)
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
                        remaining = (int)executeResult.Output["data"]["remaining"],
                    };

                    return new GetMaidResult(maid);
                }
                else
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
                    };
                    return new GetMaidResult(maid);
                }

            }
            else
            {
                return new GetMaidResult(executeResult);
            }
        }
    }
}
