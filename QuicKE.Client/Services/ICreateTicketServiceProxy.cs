using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface ICreateTicketServiceProxy : IServiceProxy
    {
        Task<CreateTicketResult> CreateTicketAsync(string services, int expertid, string latlong);
    }
}
