using System.Collections.Generic;

namespace QuicKE.Client
{
    public class PendingTicketsResult : ErrorBucket
    {
        internal List<TicketItem> tickets { get; set; }

        internal PendingTicketsResult(IEnumerable<TicketItem> items)
        {
            tickets = new List<TicketItem>();
            tickets.AddRange(items);
        }

        internal PendingTicketsResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
