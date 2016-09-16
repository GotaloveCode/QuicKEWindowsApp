
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuicKE.Client
{

    public class GetPendingTicketsServiceProxy : ServiceProxy, IGetPendingTicketsServiceProxy
    {
        public GetPendingTicketsServiceProxy()
            : base("tickets?token=" + MFundiRuntime.LogonToken)
        {
        }

        public async Task<PendingTicketsResult> GetTicketAsync()
        {
            var executeResult = await GetAsync();

            // did it work?
            if (!(executeResult.HasErrors))
            {
                // get the reports...

                 List<TicketItem> tickets = (List<TicketItem>)executeResult.Output["data"].Select(x => new TicketItem
                    {
                        Id = (int)x["ticket_id"],
                        Name = (string)x["name"]                        
                    }).ToList();

                // return...
                return new PendingTicketsResult(tickets);
            }
            else
            {
                return new PendingTicketsResult(executeResult);
            }
        }
    }
}


