using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
     public class  GetExpertsResult :ErrorBucket
    {
        internal List<ExpertItem> Experts { get; set; }

        internal GetExpertsResult(IEnumerable<ExpertItem> items)
        {
            this.Experts = new List<ExpertItem>();
            this.Experts.AddRange(items);
        }

        internal GetExpertsResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
