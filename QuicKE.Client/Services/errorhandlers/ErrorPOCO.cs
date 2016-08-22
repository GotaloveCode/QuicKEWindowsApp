using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class Error
    {
        public string code { get; set; }
        public List<string> message { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public Error error { get; set; }
    }
}