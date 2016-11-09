using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface IForgotPassServiceProxy : IServiceProxy
    {
        Task<NewPassResult> VerifyAsync(string phone_number);
    }
}
