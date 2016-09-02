using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface ISignInServiceProxy : IServiceProxy
    {
        Task<RegisterResult> SignInAsync(string phonenumber, string password);
    }
}
