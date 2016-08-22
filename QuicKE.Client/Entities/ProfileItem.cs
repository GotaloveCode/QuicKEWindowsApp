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
    public class ProfileItem
    {
        [AutoIncrement(), PrimaryKey(),JsonIgnore]
        public int id { get; set; }
        [Unique, JsonProperty("id")]
        public int NativeId { get; set; }
        
        public string name { get; set; }
        [Unique]
        public string phone { get; set; }
        [Unique]
        public string email { get; set; }
        public string user_type { get; set; }
        public string location { get; set; }
        public string photo { get; set; }
        public ProfileItem()
        {
        }

        public static async Task UpdateCacheFromServerAsync()
        {
            // create a expert proxy to call up to the server...
            var proxy = TinyIoCContainer.Current.Resolve<IGetMyProfileServiceProxy>();
            var result = await proxy.GetProfileAsync();

            // did it actually work?
            result.AssertNoErrors();

            // update...
            var conn = MFundiRuntime.GetUserDatabase();
            var existing = await conn.Table<ProfileItem>().Where(v => v.NativeId == result.Profile.NativeId).FirstOrDefaultAsync();
            if (existing != null)
                await conn.UpdateAsync(existing);
            else
                await conn.InsertAsync(result.Profile);

        }

        // reads the local cache and populates a collection...
        internal static async Task<IEnumerable<ProfileItem>> GetAllFromCacheAsync()
        {
            var conn = MFundiRuntime.GetUserDatabase();
            return await conn.Table<ProfileItem>().ToListAsync();
        }

        // indicates whether the cache is empty...
        internal static async Task<bool> IsCacheEmpty()
        {
            var conn = MFundiRuntime.GetUserDatabase();
            return (await conn.Table<ProfileItem>().FirstOrDefaultAsync()) == null;
        }

    }
}
