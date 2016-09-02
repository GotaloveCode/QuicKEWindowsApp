using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IRequestPaymentServiceProxy : IServiceProxy
    {
        Task<RequestPaymentResult> RequestPaymentAsync(int serviceid);
    }
}
