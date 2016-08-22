using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TinyIoC;
using Windows.Networking.PushNotifications;
using System.Diagnostics;

namespace QuicKE.Client
{
    public static class MFundiRuntime
    {
        public static string Module { get; private set; }
        // holds a reference to the logon token...
        public static string LogonToken { get; set; }
        internal static string Location { get; set; }
        public static int ServiceTypeID { get; set; }
        public static string Title { get; set; }

        // holds references to the database connections...
        internal const string SystemDatabaseConnectionString = "MFundi-system.db";
        internal static string UserDatabaseConnectionString = null;

        // defines the base URL of our services...
        internal const string ServiceUrlBase = "http://139.59.186.10/mfundi/public/api/";


        // starts the application/sets up state...
        public static async void Start(string module)
        {
            Module = module;

            // initialize TinyIoC...
            TinyIoCContainer.Current.AutoRegister();

            // initialize the system database... a rare move to do this synchronously as we're booting up...
            //create all tables standard for all users
            var conn = GetSystemDatabase();
            await conn.CreateTableAsync<SettingItem>();
            await conn.CreateTableAsync<ServiceItem>();
            await conn.CreateTableAsync<ExpertItem>();

            //grab last registered/logged in user   
            string LastUserPhoneNumber = await SettingItem.GetValueAsync("LastUserPhoneNumber");
            //user had registered/logged in before
            if (!string.IsNullOrEmpty(LastUserPhoneNumber))
            {   
                //set userdbstring to use last logged user
                UserDatabaseConnectionString = string.Format("MFundi-user-{0}.db", LastUserPhoneNumber);
                //get location
                Location = await UserItem.GetValueAsync("Location");
            }

            LogonToken = await SettingItem.GetValueAsync("LogonToken");
            
        }

        public static bool HasLogonToken
        {
            get
            {
                return !(string.IsNullOrEmpty(LogonToken));
            }
        }

        internal static async Task SignUpAsync(string fullname, string phonenumber, string email, string password, string token)
        {
            // set the database to be a user specific one... phonenumber
            // - for production you may prefer to use a hash)...
            UserDatabaseConnectionString = string.Format("MFundi-user-{0}.db", phonenumber);

            // store the logon token...
            LogonToken = token;

            //// initialize the database - has to be done async...
            var conn = GetUserDatabase();
             await conn.CreateTableAsync<UserItem>();
             await UserItem.SetValueAsync("token", token);
             await UserItem.SetValueAsync("phonenumber", phonenumber);
             await UserItem.SetValueAsync("email", email);
             await UserItem.SetValueAsync("fullname", fullname);
             await UserItem.SetValueAsync("password", password);
             await conn.CreateTableAsync<TicketItem>();
             await conn.CreateTableAsync<ProfileItem>();

             conn = GetSystemDatabase();
             await SettingItem.SetValueAsync("LastUserPhoneNumber", phonenumber);
             await SettingItem.SetValueAsync("LogonToken", token);
            //will check this value before sign in to detrmine how to recreate local table if user is not created on device
            //but has account already
             await SettingItem.SetValueAsync("LastEmail", email);

        }




        internal static SQLiteAsyncConnection GetSystemDatabase()
        {
            return new SQLiteAsyncConnection(SystemDatabaseConnectionString);
        }

        internal static SQLiteAsyncConnection GetUserDatabase()
        {
            return new SQLiteAsyncConnection(UserDatabaseConnectionString);
        }

        
    }
}
