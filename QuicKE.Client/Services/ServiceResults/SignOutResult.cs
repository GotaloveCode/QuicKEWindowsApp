using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
     public class SignOutResult : ErrorBucket
    {
        public string Data { get; private set; }
        public string Status { get; private set; }

        public SignOutResult(string data,string status)
        {
            this.Data = data;
            this.Status = status;
        }

        internal SignOutResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
