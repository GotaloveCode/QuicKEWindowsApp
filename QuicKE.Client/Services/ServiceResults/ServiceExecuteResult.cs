using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuicKE.Client
{
    public class ServiceExecuteResult : ErrorBucket
    {
        public JObject Output { get; private set; }

        internal ServiceExecuteResult(JObject output)
        {
            this.Output = output;
        }

        internal ServiceExecuteResult(JObject output, string error)
            : this(output)
        {
            this.AddError(error);
        }

        internal ServiceExecuteResult(JObject output, List<string> errorlist)
            : this(output)
        {
            this.Copy(errorlist);
        }
    }
}
