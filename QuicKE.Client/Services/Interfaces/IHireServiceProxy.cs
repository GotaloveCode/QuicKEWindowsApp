using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IHireServiceProxy : IServiceProxy
    {
        Task<TaskCompleteResult> HireAsync(int ticketID);
    }
}
