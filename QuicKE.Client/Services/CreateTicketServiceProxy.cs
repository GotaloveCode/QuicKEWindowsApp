using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace QuicKE.Client.Services
{
    public class CreateTicketServiceProxy : ServiceProxy, ICreateTicketServiceProxy
    {
        public CreateTicketServiceProxy()
            : base("tickets/create")
        {
        }

        public async Task<CreateTicketResult> CreateTicketAsync(string services, int expertid, string latlong)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.AddArray("services", services);
            input.Add("expert", expertid);
            input.Add("location", latlong);


            // call...
            var executeResult = await this.PostAsync(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                //var serviceid = (JArray)executeResult.Output["data"]["services"];
                //var servs = serviceid.ToString();
                //JObject jObject = JObject.Parse(servs);
                var ids = from x in executeResult.Output["data"]["services"] select (int)x["id"];
                string serviceids = "";
                foreach (var item in ids)
                {
                    serviceids += item.ToString() + ",";
                }
                serviceids = serviceids.Remove(serviceids.Length - 1);

                TicketItem ticket =  new TicketItem
                   {
                       NativeId = (int)executeResult.Output["data"]["id"],
                       ticket_no = (string)executeResult.Output["data"]["ticket_no"],
                       expert = (string)executeResult.Output["data"]["expert"],
                       phone = (string)executeResult.Output["data"]["phone"],
                       id_number = (string)executeResult.Output["data"]["id_number"],
                       photo = (string)executeResult.Output["data"]["photo"],
                       badge = (string)executeResult.Output["data"]["badge"],
                       status = (string)executeResult.Output["data"]["status"],
                       location = (string)executeResult.Output["data"]["location"],
                       booking_date = DateTime.Parse((string)executeResult.Output["data"]["booking_date"]),
                       cost = (decimal)executeResult.Output["data"]["cost"],
                       services = serviceids,
                   };
                //var conn = MFundiRuntime.GetUserDatabase();
                //await UserItem.SetValueAsync("TicketID", ticket.NativeId.ToString());
                //await UserItem.SetValueAsync("TicketCost", ticket.cost.ToString());
                ApplicationData.Current.LocalSettings.Values["TicketID"] = ticket.NativeId.ToString();
                ApplicationData.Current.LocalSettings.Values["TicketCost"] = ticket.cost.ToString();
                ApplicationData.Current.LocalSettings.Values["Token"] = MFundiRuntime.LogonToken;
                return new CreateTicketResult(ticket);
            }
            else
                return new CreateTicketResult(executeResult);
        }
    }



}
