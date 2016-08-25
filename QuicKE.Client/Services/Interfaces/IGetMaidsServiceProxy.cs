
using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IGetMaidsServiceProxy : IServiceProxy
    {
        Task<GetProfileResult> GetProfileAsync();
    }
}