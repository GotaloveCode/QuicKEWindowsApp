using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IHireServiceProxy : IServiceProxy
    {
        Task<HireResult> HireAsync(int ticketid);
    }
}
