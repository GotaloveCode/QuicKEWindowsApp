using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;

namespace QuicKE.Client
{
    public class HomePageViewModel:ViewModel, IHomePageViewModel
    {
        public ICommand ProfileCommand { get; private set; }
        public ICommand ViewPikiCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand ViewHouseKeepingCommand { get; private set; }
        public ICommand ShowLocationCommand { get { return GetValue<ICommand>(); } private set { SetValue(value); } }
        

        public const string MyLocationPropertyName = "MyLocation";

        private Geoposition _myLocation = null;

        public Geoposition MyLocation
        {
            get
            {
                return _myLocation;
            }

            set
            {
                if (_myLocation == value)
                {
                    return;
                }


                _myLocation = value;
                SetValue(value);
            }
        }
        //internal static string locationtext = "-1.2883400,36.8063400";
        public static string locationtext{ get ; set ; }
        internal static string MapServiceToken = "Ar8NFFIIihPkrQZ4KyDyWkgeDnLO-Qhf9KLAls27uZGHpqG3yL6mIS5w5sRRqmPk";
       
        
        public HomePageViewModel()
        {

        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
           // var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            ProfileCommand = new NavigateCommand<IProfilePageViewModel>(host);
            ViewPikiCommand = new DelegateCommand((e) =>
            {
                //set servicetypeid
                //localSettings.Values["ServiceId"] = 2;
                MFundiRuntime.ServiceTypeID = 2;
                MFundiRuntime.Title = "Delivery";
                Host.ShowView(typeof(IHomePageViewModel));
            }); 
            this.ViewHouseKeepingCommand = new DelegateCommand((e) =>
            {
                //set servicetypeid
               // localSettings.Values["ServiceId"] = 1;
                MFundiRuntime.Title = "House Keeping";
                MFundiRuntime.ServiceTypeID = 1;
                Host.ShowView(typeof(IServicesPageViewModel));
            }); 
            LogoutCommand = new DelegateCommand(async (e) =>
            { // get the location...
                await LogOut();
            });

            this.ShowLocationCommand = new DelegateCommand(async (e) =>
            { // get the location...
                await GetMyLocation();
            });
        }

        private async Task LogOut()
        {
            ErrorBucket errors = new ErrorBucket();
            var proxy = TinyIoCContainer.Current.Resolve<ILogOutServiceProxy>();
            using (this.EnterBusy())
            {
                var result = await proxy.LogOutAsync();
                if (!(result.HasErrors))
                {
                    var conn = MFundiRuntime.GetSystemDatabase();
                    var setting = (await conn.Table<SettingItem>().Where(v => v.Name == "LogonToken").ToListAsync()).FirstOrDefault();
                    if (setting != null)
                        await conn.DeleteAsync(setting);
                    ApplicationData.Current.LocalSettings.Values["LoggedIn"] = "False";
                    MFundiRuntime.LogonToken = null;
                    errors.AddError("Logged Out Successfully");
                    await this.Host.ShowAlertAsync(errors);
                    this.Host.ShowView(typeof(IRegisterPageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }
            // errors?
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }

        private async Task GetMyLocation()
        {
            using (this.EnterBusy())
            {
                //Geolocator locator = new Geolocator() { DesiredAccuracy = PositionAccuracy.High }; ;
                Geolocator locator = new Geolocator();
                locator.DesiredAccuracyInMeters = 50;

                var location = await locator.GetGeopositionAsync(TimeSpan.FromMilliseconds(1), TimeSpan.FromSeconds(60));
                MyLocation = location;
                
                
                string latitude = location.Coordinate.Point.Position.Latitude.ToString();
                string longitude = location.Coordinate.Point.Position.Longitude.ToString();
                locationtext = latitude + "," + longitude;
                MFundiRuntime.Location = locationtext;
                await UserItem.SetValueAsync("location", locationtext);
                
                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(location.Coordinate.Point);
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    if (result.Locations.Count > 0)
                    {
                        string display = result.Locations[0].Address.BuildingName + " " + result.Locations[0].Address.Street + " " + result.Locations[0].Address.Neighborhood + " " +
                          result.Locations[0].Address.Town;
                        var toast = new ToastNotificationBuilder(new string[] { "Location found",  display });
                        toast.Update();
                    }
                }
                
               
            }
        }
        public override async void Activated(object args)
        {
            base.Activated(args);
            if (MFundiRuntime.UserDatabaseConnectionString == null)
            {
                string LastUserPhoneNumber = await SettingItem.GetValueAsync("LastUserPhoneNumber");            
            if (!string.IsNullOrEmpty(LastUserPhoneNumber)) 
                //set userdbstring to use last logged user
                MFundiRuntime.UserDatabaseConnectionString = string.Format("MFundi-user-{0}.db", LastUserPhoneNumber);
            }

            string loc = await UserItem.GetValueAsync("location");
            locationtext = loc;
            MFundiRuntime.Location = locationtext;
            //await conn.DropTableAsync<ServiceItem>();
            //await conn.CreateTableAsync<ServiceItem>();
            if (string.IsNullOrEmpty(loc))
            {
                var toast = new ToastNotificationBuilder(new string[] { "Getting your location details" });
                toast.Update();
                await GetMyLocation();
            }

        }
    }
}
