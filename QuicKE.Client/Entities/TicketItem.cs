using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace QuicKE.Client
{
    public class TicketItem : ModelItem
    {
        [AutoIncrement(), PrimaryKey(), JsonIgnore]
        public int id { get; set; }

        // other fields...
        [Unique, JsonProperty("id")]
        public int NativeId { get { return this.GetValue<int>(); } set { this.SetValue(value); } }
        public string ticket_no { get; set; }
        public string expert { get; set; }
        public string phone { get; set; }
        public string id_number { get; set; }
        public string photo { get; set; }
        public string badge { get; set; }
        public string status { get; set; }
        public string location { get; set; }
        public DateTime booking_date { get; set; }
        public decimal cost { get; set; }
        public string services { get; set; }

        public TicketItem()
        {
        }
        public static async Task UpdateCacheFromServerAsync()
        {
            // create a ticket proxy to call up to the server...
            var proxy = TinyIoCContainer.Current.Resolve<IGetTicketServiceProxy>();
            var result = await proxy.GetTicketsAsync();

            // did it actually work?
            result.AssertNoErrors();

            // update...
            var conn = MFundiRuntime.GetUserDatabase();
            foreach (var ticket in result.Tickets)
            {
                // load the existing one, deleting it if we find it...
                var existing = await conn.Table<TicketItem>().Where(v => v.NativeId == ticket.NativeId).FirstOrDefaultAsync();
                if (existing != null)
                    await conn.DeleteAsync(existing);

                // create...
                await conn.InsertAsync(ticket);
            }
        }

        // reads the local cache and populates a collection...
        internal static async Task<IEnumerable<TicketItem>> GetAllFromCacheAsync()
        {
            var conn = MFundiRuntime.GetUserDatabase();
            return await conn.Table<TicketItem>().ToListAsync();
        }

        // indicates whether the cache is empty...
        internal static async Task<bool> IsCacheEmpty()
        {
            var conn = MFundiRuntime.GetUserDatabase();
            return (await conn.Table<TicketItem>().FirstOrDefaultAsync()) == null;
        }
    }
}