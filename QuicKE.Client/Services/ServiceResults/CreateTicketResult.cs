using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class CreateTicketResult : ErrorBucket
    {
         internal TicketItem ticket { get; set; }

        //public decimal cost { get; private set; }
        //public string ticket_no { get; private set; }

         public CreateTicketResult(TicketItem item)
        {
            this.ticket = item;
        }
        //public TicketResult(decimal cost, string ticket_no)
        //{
        //    this.cost = cost;
        //    this.ticket_no = ticket_no;
        //}

         internal CreateTicketResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
