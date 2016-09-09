
using System.Collections.Generic;


namespace QuicKE.Client
{
    public class  GetServicesResult :ErrorBucket
    {
        internal List<ServiceItem> Services { get; set; }

        internal GetServicesResult(IEnumerable<ServiceItem> items)
        {
            Services = new List<ServiceItem>();
            Services.AddRange(items);
        }

        internal GetServicesResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
