using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuicKE.Client.Services;

namespace QuicKE.Client
{
    public class GetTicketServiceProxy : ServiceProxy, IGetTicketServiceProxy
    {
        public GetTicketServiceProxy()
            : base("tickets")
        {
        }

        public async Task<GetTicketsResult> GetTicketsAsync()
        {

            // call...
            var executeResult = await this.GetAsync();

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string status = (string)executeResult.Output["status"];

                IEnumerable<TicketItem> tickets = (IEnumerable<TicketItem>)executeResult.Output["data"]
                    .Select(x => new TicketItem
                    {
                        NativeId = (int)x["id"],
                        ticket_no = (string)x["ticket_no"],
                        expert = (string)x["expert"],
                        phone = (string)x["phone"],
                        id_number = (string)x["id_number"],
                        photo = (string)x["photo"],                        
                        badge = (string)x["badge"],
                        status = (string)x["status"],
                        location = (string)x["location"],
                        booking_date = (DateTime)x["booking_date"],
                        cost = (decimal)x["cost"],
                        //services = ((List<string>)x["services"]["id"]).split(),
                    });

                return new GetTicketsResult(tickets);
            }
            else
                return new GetTicketsResult(executeResult);
        }
    }
}
