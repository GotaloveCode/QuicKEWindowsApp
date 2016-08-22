using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using TinyIoC;

namespace QuicKE.Client
{
    public class ServiceItem : ModelItem
    {
        // key field...
        [AutoIncrement(), PrimaryKey(), JsonIgnore]
        public int Id { get; set; }

        // other fields...
        [JsonProperty("id")]
        public int NativeId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cost")]
        public decimal Cost
        {
            get
            {
                return GetValue<decimal>();
            }
            set
            {
                SetValue(value);
            }
        }
        [JsonIgnore]
        public int ServiceTypeId { get; set; }
        [JsonIgnore] //prefer not to have it on db has no value
        public bool IsSelected
        {
            get
            {

                return GetValue<bool>();
            }
            set
            {
                SetValue(value);
            }
        }

        public ServiceItem()
        {
        }

        public static async Task UpdateCacheFromServerAsync(int servicetypeid)
        {
            // create a service proxy to call up to the server...
            var proxy = TinyIoCContainer.Current.Resolve<IGetServicesServiceProxy>();
            var result = await proxy.GetServicesAsync();

            // did it actually work?
            result.AssertNoErrors();

            // update...
            var conn = MFundiRuntime.GetSystemDatabase();
            foreach (var service in result.Services)
            {
                // load the existing one, deleting it if we find it...
                var existing = await conn.Table<ServiceItem>().Where(v => v.NativeId == service.NativeId && v.ServiceTypeId == servicetypeid).FirstOrDefaultAsync();
                if (existing != null)
                    await conn.UpdateAsync(existing);
                else
                {
                    //assign the servicetypeid
                    service.ServiceTypeId = servicetypeid;
                    // create...
                    await conn.InsertAsync(service);
                }
                
            }
        }

        // reads the local cache and populates a collection...
        internal static async Task<IEnumerable<ServiceItem>> GetAllFromCacheAsync(int servicetypeid)
        {
            var conn = MFundiRuntime.GetSystemDatabase();
            return await conn.Table<ServiceItem>().Where(v => v.ServiceTypeId == servicetypeid).ToListAsync();
        }

        // indicates whether the cache is empty...
        internal static async Task<bool> IsCacheEmpty(int servicetypeid)
        {
            var conn = MFundiRuntime.GetSystemDatabase();
            return (await conn.Table<ServiceItem>().Where(v => v.ServiceTypeId == servicetypeid).FirstOrDefaultAsync()) == null;
        }


    }
}
