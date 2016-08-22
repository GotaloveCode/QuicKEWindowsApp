using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class GetUserResult:ErrorBucket
    {
        internal List<UserItem> Users { get; set; }

        internal GetUserResult(IEnumerable<UserItem> items)
        {
            this.Users = new List<UserItem>();
            this.Users.AddRange(items);
        }

        internal GetUserResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
