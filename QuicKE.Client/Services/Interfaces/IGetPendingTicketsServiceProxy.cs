
using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IGetPendingTicketsServiceProxy : IServiceProxy
    {
        Task<PendingTicketsResult> GetTicketAsync();
    }
}
