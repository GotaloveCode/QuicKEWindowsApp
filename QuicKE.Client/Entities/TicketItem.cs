using Newtonsoft.Json;

namespace QuicKE.Client
{
    public class TicketItem
    {
        [JsonProperty("ticket_id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
