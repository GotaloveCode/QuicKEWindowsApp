using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class  GetServicesResult :ErrorBucket
    {
        internal List<ServiceItem> Services { get; set; }

        internal GetServicesResult(IEnumerable<ServiceItem> items)
        {
            this.Services = new List<ServiceItem>();
            this.Services.AddRange(items);
        }

        internal GetServicesResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
