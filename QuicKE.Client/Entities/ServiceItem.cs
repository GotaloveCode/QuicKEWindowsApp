using Newtonsoft.Json;

namespace QuicKE.Client
{
    public class ServiceItem : ModelItem
    {

        [JsonProperty("id")]
        public int Id { get;set; }
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
