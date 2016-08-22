using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class UserItem
    {
        [AutoIncrement(), PrimaryKey()]
        public int ID { get; set; }

        // other fields...
        [Unique]
        public string Name { get; set; }
        public string Value { get; set; }
        public static async Task SetValueAsync(string name, string value)
        {
            var conn = MFundiRuntime.GetUserDatabase();

            // load an existing value...
            var user = await conn.Table<UserItem>().Where(v => v.Name == name).FirstOrDefaultAsync();
            if (user != null)
            {
                // change and update...
                user.Value = value;
                await conn.UpdateAsync(user);
            }
            else
            {
                user = new UserItem()
                {
                    Name = name,
                    Value = value
                };

                // save...
                await conn.InsertAsync(user);
            }
        }

        public static async Task<string> GetValueAsync(string name)
        {
            var conn = MFundiRuntime.GetUserDatabase();

            // load any existing value...
            var user = (await conn.Table<UserItem>().Where(v => v.Name == name).ToListAsync()).FirstOrDefault();
            if (user != null)
                return user.Value;
            else
                return null;
        }
    }
}
