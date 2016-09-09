
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
            MFundiRuntime.LogonToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjEwNywiaXNzIjoiaHR0cDpcL1wvMTM5LjU5LjE4Ni4xMFwvbWZ1bmRpXC9wdWJsaWNcL2FwaVwvYXV0aCIsImlhdCI6MTQ3MzMzOTk3OCwiZXhwIjoxNTA0ODc1OTc4LCJuYmYiOjE0NzMzMzk5NzgsImp0aSI6ImRmZDA2MzE2MjU0M2VkNWQ1ZTNkNzVlZGMxYjQzYmM1In0.A1MCNgzCHmAeKu7QV8i6M7NY8y_7c-TaQhetqL0RAN4";

            Url = MFundiRuntime.ServiceUrlBase + "tickets?token=" + MFundiRuntime.LogonToken;
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


