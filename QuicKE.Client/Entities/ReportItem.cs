using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.Data.Json;
using TinyIoC;
using Newtonsoft.Json;

namespace MFundi.Client
{
    public class ProfessionItem
    {
        // key field...
        [AutoIncrement(), PrimaryKey()]
        public int Id { get; set; }

        // other fields...
        [Unique, JsonProperty("id")]
        public string NativeId { get; set; }

        [JsonProperty("title")]
        public string name { get; set; }





        public ProfessionItem()
        {
        }

        // updates the local cache of the reports...
        public static async Task UpdateCacheFromServerAsync()
        {
            // create a service proxy to call up to the server...
            var proxy = TinyIoCContainer.Current.Resolve<IGetProfessionServiceProxy>();
            var result = await proxy.GetReportsByUserAsync();

            // did it actually work?
            result.AssertNoErrors();

            // update...
            var conn = MFundiRuntime.GetUserDatabase();
            foreach (var report in result.Reports)
            {
                // load the existing one, deleting it if we find it...
                var existing = await conn.Table<ReportItem>().Where(v => v.NativeId == report.NativeId).FirstOrDefaultAsync();
                if (existing != null)
                    await conn.DeleteAsync(existing);

                // create...
                await conn.InsertAsync(report);
            }
        }

        // reads the local cache and populates a collection...
        internal static async Task<IEnumerable<ReportItem>> GetAllFromCacheAsync()
        {
            var conn = StreetFooRuntime.GetUserDatabase();
            return await conn.Table<ReportItem>().ToListAsync();
        }

        // indicates whether the cache is empty...
        internal static async Task<bool> IsCacheEmpty()
        {
            var conn = StreetFooRuntime.GetUserDatabase();
            return (await conn.Table<ReportItem>().FirstOrDefaultAsync()) == null;
        }
    }
}
