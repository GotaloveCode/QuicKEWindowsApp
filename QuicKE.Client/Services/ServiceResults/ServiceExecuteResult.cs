using Newtonsoft.Json.Linq;
using System.Collections.Generic;


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
