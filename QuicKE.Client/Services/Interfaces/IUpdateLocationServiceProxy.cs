
using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IUpdateLocationServiceProxy : IServiceProxy
    {
        Task<UpdateLocationResult> UpdateLocationAsync(string location);
    }
}
