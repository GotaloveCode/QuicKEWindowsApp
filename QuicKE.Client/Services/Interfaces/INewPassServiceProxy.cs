using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface INewPassServiceProxy : IServiceProxy
    {
        Task<NewPassResult> VerifyAsync(string phone_number, string password, string confirm_password, string code);
    }
}
