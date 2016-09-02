using Newtonsoft.Json;
using SQLite;

namespace QuicKE.Client
{
    public class ServiceItem : ModelItem
    {
        // key field...
        [AutoIncrement(), PrimaryKey(), JsonIgnore]
        public int Id { get;set; }

        // other fields...
        [JsonProperty("id")]
        public int NativeId { get;set; }
        [JsonProperty("name")]
        public string Name { get;set; }
        [JsonProperty("cost")]
        public string Cost
        {
            get
            {
                return GetValue<string>();
            }
            set
            {
                SetValue(value);
            }
        }
      
       

        public ServiceItem()
        {
        }

    

    }
}
