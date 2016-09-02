using System;
using System.Collections.Generic;
using System.Linq;
namespace QuicKE.Client
{

     public class HireResult : ErrorBucket
    {

        public string Status { get; private set; }

        public HireResult(string status)
        {

            this.Status = status;
        }

        internal HireResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
