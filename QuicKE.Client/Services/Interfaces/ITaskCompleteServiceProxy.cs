using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface ITaskCompleteServiceProxy : IServiceProxy
    {
        Task<TaskCompleteResult> TaskCompleteAsync(int TicketID);
    }
}
