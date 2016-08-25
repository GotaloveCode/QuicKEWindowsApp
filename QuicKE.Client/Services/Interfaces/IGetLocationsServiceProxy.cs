
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface IGetLocationsServiceProxy : IServiceProxy
    {
        Task<GetLocationsResult> GetLocationsAsync();
    }
    
}
