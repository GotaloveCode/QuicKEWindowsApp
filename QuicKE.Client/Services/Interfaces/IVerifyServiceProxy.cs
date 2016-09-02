using System.Threading.Tasks;

namespace QuicKE.Client
{
    interface IVerifyServiceProxy : IServiceProxy
    {
        Task<VerifyResult> VerifyAsync(string phone_number);
    }
}
