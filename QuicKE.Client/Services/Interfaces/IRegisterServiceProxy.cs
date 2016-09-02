using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface IRegisterServiceProxy : IServiceProxy
    {
        Task<RegisterResult> RegisterAsync(string fullname, string phonenumber, string password, string location, string code);
    }
}
    