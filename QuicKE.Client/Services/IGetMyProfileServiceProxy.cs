using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IGetMyProfileServiceProxy : IServiceProxy
    {
        Task<GetProfileResult> GetProfileAsync();
    }
}
