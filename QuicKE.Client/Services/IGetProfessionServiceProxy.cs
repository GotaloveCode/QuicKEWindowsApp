using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFundi.Client
{
    public interface IGetProfessionServiceProxy : IServiceProxy
    {
        Task<GetReportsByUserResult> GetReportsByUserAsync();
    }
}
