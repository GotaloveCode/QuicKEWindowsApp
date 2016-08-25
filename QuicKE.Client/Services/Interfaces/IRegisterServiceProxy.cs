using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface IRegisterServiceProxy : IServiceProxy
    {
        Task<RegisterResult> RegisterAsync(string fullname, string phonenumber, string email, string password);
    }
}
    