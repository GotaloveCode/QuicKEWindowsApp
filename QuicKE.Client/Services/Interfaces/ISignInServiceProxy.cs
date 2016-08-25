using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public interface ISignInServiceProxy : IServiceProxy
    {
        Task<RegisterResult> SignInAsync(string username, string password);
    }
}
