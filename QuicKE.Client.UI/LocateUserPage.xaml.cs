using QuicKE.Client.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace QuicKE.Client.UI
{
     [ViewModel(typeof(ILocateUserPageViewModel))]
    public sealed partial class LocateUserPage : MFundiPage
    {
        // public Geopoint MyLocation { get; set; }
        public LocateUserPage()
        {
            this.InitializeComponent();
            // get...
            this.InitializeViewModel();
           // LoadMap();

           

        }
        //private async void SetMapView(Geopoint point)
        //{
        //    MyLocationPushpin.Visibility = Visibility.Visible;
        //    await MainMap.TrySetViewAsync(point, 15, 0, 0, Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
        //}

        //private async Task LoadMap()
        //{
            //LocationResult Lr = await LocationHelper.GetCurrentLocationAsync();            
            //MyLocation = Lr.Location.Coordinate.Point;
            //MainMap.Center = MyLocation;
            //MainMap.ZoomLevel = 18;            
            //mapIcon.Image = RandomAccessStreamReference.CreateFromUri(
            //  new Uri("ms-appx:///Assets/img_location_pin.png"));
            //mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
            //mapIcon.Location = Lr.Location.Coordinate.Point;
            //mapIcon.Title = "You are here";
            //MainMap.MapElements.Add(mapIcon);
        //    await GetAddresses();

        //}

        //private async Task GetAddresses()
        //{
        //    MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync("", MyLocation, 5);
        //    if (result.Status == MapLocationFinderStatus.Success)
        //    {
        //        List<string> locations = new List<string>();
        //        foreach (MapLocation mapLocation in result.Locations)
        //        {
        //            // create a display string of the map location
        //            string display = mapLocation.Address.StreetNumber + " " +
        //              mapLocation.Address.Street + Environment.NewLine +
        //              mapLocation.Address.Town + ", " +
        //              mapLocation.Address.RegionCode + "  " +
        //              mapLocation.Address.PostCode + Environment.NewLine +
        //              mapLocation.Address.CountryCode;
        //            // Add the display string to the location list.
        //            locations.Add(display);
        //        }

        //        // Bind the location list to the ListView control.
        //        lvLocations.ItemsSource = locations;
        //    }
        //    else
        //    {
        //        // Tell the user to try again.
        //    }
        //}


        private async void MainMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            // Find the address of the tapped location.
            MapLocationFinderResult result =
              await MapLocationFinder.FindLocationsAtAsync(args.Location);
            if (result.Status == MapLocationFinderStatus.Success)
            {
                if (result.Locations.Count > 0)
                {
                    string display = result.Locations[0].Address.StreetNumber + " " +
                      result.Locations[0].Address.Street;
                    
                    if(!string.IsNullOrEmpty((display.Trim())))
                    MyMapIcon.Title = display;
                }
            }
            if(args.Location!=null)
                MyMapIcon.Location = args.Location;
        }

        private void MainMap_Loaded(object sender, RoutedEventArgs e)
        {
            MapIcon MyMapIcon = new MapIcon();
            MyMapIcon.Image = RandomAccessStreamReference.CreateFromUri(
             new Uri("ms-appx:///Assets/img_location_pin.png"));
            MyMapIcon.NormalizedAnchorPoint = new Point(0, 1);
            MainMap.MapElements.Add(MyMapIcon);
        }




         
        
    }   
      
}
