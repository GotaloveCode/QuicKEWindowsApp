using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface IGetChargesServiceProxy : IServiceProxy
    {
        Task<GetServicesResult> GetServicesAsync();
    }
}
