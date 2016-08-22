using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class SignInResult : ErrorBucket
    {
        public string Token { get; private set; }

        public SignInResult(string token)
        {
            this.Token = token;
        }

        internal SignInResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
