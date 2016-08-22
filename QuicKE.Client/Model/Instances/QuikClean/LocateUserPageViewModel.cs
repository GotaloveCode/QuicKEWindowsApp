using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;


namespace QuicKE.Client
{
    public class LocateUserPageViewModel : ViewModel, ILocateUserPageViewModel
    {
        public ICommand GetPositionCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public Geopoint MyLocation { get { return this.GetValue<Geopoint>(); } private set { this.SetValue(value); } }
        public ObservableCollection<string> Locations { get { return this.GetValue<ObservableCollection<string>>(); } private set { this.SetValue(value); } }

        public string LatLong { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Places { get { return GetValue<string>(); } set { SetValue(value); } }
        
        ErrorBucket e = new ErrorBucket();

        public LocateUserPageViewModel()
        {

        }
        public async override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

             LocationResult Lr  = await LocationHelper.GetCurrentLocationAsync();
             this.MyLocation = Lr.Location.Coordinate.Point;
             this.LatLong = "(" + MyLocation.Position.Latitude.ToString() + ", " + MyLocation.Position.Longitude.ToString() + ")";
             this.Locations = new ObservableCollection<string>();
             this.Places = "I am here";
            await GetAddresses();
            this.GetPositionCommand = new DelegateCommand(async (e) =>
            { // get the location...
                await this.GetMyLocation();
            });
            //this.GetPositionCommand = new NavigateCommand<ITicketPageViewModel>(host);
            //Geolocator geolocator = new Geolocator();
            //Geoposition pos = await geolocator.GetGeopositionAsync();
            //Geopoint myLocation = pos.Coordinate.Point;
            //this.MyLocation = myLocation;
            ////myLocation.Position.Latitude

            //e.AddError("Lat:" + myLocation.Position.Latitude.ToString() + " Long:" + myLocation.Position.Longitude.ToString());
            //await this.Host.ShowAlertAsync(e);
            

        }
        private async Task GetMyLocation()
        {
            LocationResult Lr = await LocationHelper.GetCurrentLocationAsync();
            if (Lr.Code != LocationResultCode.Ok)
            {
                e.AddError("Could not access location.Please turn on gps settings");
            }
            MyLocation = Lr.Location.Coordinate.Point;

            await GetAddresses();

        }

        private async Task GetAddresses()
        {
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(MyLocation);
            if (result.Status == MapLocationFinderStatus.Success)
            {
                
                foreach (MapLocation mapLocation in result.Locations)
                {
                    // create a display string of the map location
                      //mapLocation.Address.RegionCode + "  " +
                      //mapLocation.Address.PostCode + ", " +
                      //mapLocation.Address.StreetNumber + " " +
                      //mapLocation.Address.Street + ", " +
                    string display = mapLocation.Address.Town + ", " +
                      mapLocation.Address.Country;
                    // Add the display string to the location list.
                    
                    if(!string.IsNullOrEmpty(display))
                        this.Locations.Add(display);
                }
                if(this.Locations.Count>0)
                    this.Places = this.Locations.FirstOrDefault();
                  
                // Bind the location list to the ListView control.
                //lvLocations.ItemsSource = locations;
            }
            else
            {
                
                // Tell the user to try again.
            }
        }
    }
}
