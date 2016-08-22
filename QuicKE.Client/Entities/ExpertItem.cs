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
    public class ExpertItem : ModelItem
    {
        // key field...
        [AutoIncrement(), PrimaryKey(), JsonIgnore]
        public int Id { get; set; }

        // other fields...
        [Unique, JsonProperty("id")]
        public int NativeId { get { return this.GetValue<int>(); } set { this.SetValue(value); } }

        public string name { get { return this.GetValue<string>(); } set { this.SetValue(value); } }

        public string id_number { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string email { get { return this.GetValue<string>(); } set { this.SetValue(value); } }

        public string phone { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string badge { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string profession { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string destination { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string distance { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string time { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public int age { get { return this.GetValue<int>(); } set { this.SetValue(value); } }


       

        public ExpertItem()
        {
        }

        public static async Task UpdateCacheFromServerAsync()
        {
            // create a expert proxy to call up to the server...
            var proxy = TinyIoCContainer.Current.Resolve<IGetExpertsServiceProxy>();
            var result = await proxy.GetExpertsAsync();

            // did it actually work?
            result.AssertNoErrors();

            // update...
            var conn = MFundiRuntime.GetSystemDatabase();
            foreach (var expert in result.Experts)
            {
                // load the existing one, deleting it if we find it...
                var existing = await conn.Table<ExpertItem>().Where(v => v.NativeId == expert.NativeId).FirstOrDefaultAsync();
                if (existing != null)
                    await conn.DeleteAsync(existing);

                // create...
                await conn.InsertAsync(expert);
            }
        }

        // reads the local cache and populates a collection...
        internal static async Task<IEnumerable<ExpertItem>> GetAllFromCacheAsync()
        {
            var conn = MFundiRuntime.GetSystemDatabase();
            return await conn.Table<ExpertItem>().ToListAsync();
        }

        // indicates whether the cache is empty...
        internal static async Task<bool> IsCacheEmpty()
        {
            var conn = MFundiRuntime.GetSystemDatabase();
            return (await conn.Table<ExpertItem>().FirstOrDefaultAsync()) == null;
        }


    }
}
