using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuicKE.Client
{
    public class GetTicketsResult : ErrorBucket
    {
        internal List<TicketItem> Tickets { get; set; }

        internal GetTicketsResult(IEnumerable<TicketItem> items)
        {
            this.Tickets = new List<TicketItem>();
            this.Tickets.AddRange(items);
        }

        internal GetTicketsResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
