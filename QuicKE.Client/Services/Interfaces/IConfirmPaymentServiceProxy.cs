using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface IConfirmPaymentServiceProxy :IServiceProxy
    {
        Task<ConfirmPaymentResult> ConfirmPaymentAsync(string code);
    }
}