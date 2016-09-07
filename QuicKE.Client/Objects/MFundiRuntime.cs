using TinyIoC;
using Windows.Storage;

namespace QuicKE.Client
{
    public static class MFundiRuntime
    {
        public static string Module { get; private set; }
        // holds a reference to the logon token...
        public static string LogonToken { get; set; }
        internal static string Location { get; set; }
        public static int ServiceTypeID { get; set; }
        public static string TicketID { get; set; }


        // defines the base URL of our services...
        internal const string ServiceUrlBase = "http://139.59.186.10/mfundi/public/api/";

        

        // starts the application/sets up state...
        public static void Start(string module)
        {
            Module = module;

            // initialize TinyIoC...
            TinyIoCContainer.Current.AutoRegister();

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var values = ApplicationData.Current.LocalSettings.Values;

            //grab last registered/logged in user 
            if (values.ContainsKey("LastUserPhoneNumber"))
            {
                string LastUserPhoneNumber = localSettings.Values["LastUserPhoneNumber"].ToString();               
            }

            //grab token from db if user was logged in before
            if (values.ContainsKey("LogonToken"))
            {
                LogonToken = localSettings.Values["LogonToken"].ToString();
            }

            
        }

        public static bool HasLogonToken
        {
            get
            {
                return !(string.IsNullOrEmpty(LogonToken));
            }
        }



        

        
    }
}
