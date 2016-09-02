using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyIoC;

namespace QuicKE.Client
{
    public class ProfileItem
    {
        [AutoIncrement(), PrimaryKey(),JsonIgnore]
        public int id { get;set;}
        [Unique, JsonProperty("id")]
        public int NativeId { get;set;}
        
        public string name { get;set;}
        [Unique]
        public string phone { get;set;}       
        public string user_type { get;set;}
        public string location { get;set;}
        public string photo { get;set;}
        public ProfileItem()
        {
        }
        
    }
}
