using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IGetMyProfileServiceProxy : IServiceProxy
    {
        Task<GetProfileResult> GetProfileAsync();
    }
}
