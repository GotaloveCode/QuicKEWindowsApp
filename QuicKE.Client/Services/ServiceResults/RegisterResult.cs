using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuicKE.Client;

namespace QuicKE.Client
{
    public class RegisterResult : ErrorBucket
    {
        public string status { get; private set; }
        public string token { get; private set; }

        public RegisterResult(string token,string status)
        {
            this.token = token;
            this.status = status;
        }

        internal RegisterResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
   
}
