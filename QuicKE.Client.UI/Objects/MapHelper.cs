using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace QuicKE.Client.UI
{
     public static class MapHelper
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.RegisterAttached(
            "Center",
            typeof(Geopoint),
            typeof(MapHelper),
            new PropertyMetadata(0.0, new PropertyChangedCallback(CenterChanged))
        );
        //public static readonly DependencyProperty ZoomProperty = DependencyProperty.RegisterAttached(
        //           "Center",
        //           typeof(Geopoint),
        //           typeof(MapHelper),
        //           new PropertyMetadata(0.0, new PropertyChangedCallback(CenterChanged))
        //       );

        public static void SetCenter(DependencyObject obj, Geopoint value)
        {
            obj.SetValue(CenterProperty, value);
        }
 
        public static Geopoint GetCenter(DependencyObject obj)
        {
            return (Geopoint)obj.GetValue(CenterProperty);
        }
 
        private static void CenterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MapControl map = (MapControl)obj;
            if (map != null)
                map.Center = (Geopoint) args.NewValue;
        }
    }
}
//    public class MyMap : ContentControl
//    {
//        // containerized map...
//        private MyMap InnerMap { get; set; }
//        // as per the grid...

//        // dependency properties...
//        //public static readonly DependencyProperty PushpinPointProperty =
//        //    DependencyProperty.Register("PushpinPoint", typeof(AdHocMappablePoint), typeof(MyMap),
//        //    new PropertyMetadata(null, (d, e) => ((MyMap)d).SetPushpinPoint((AdHocMappablePoint)e.NewValue)));
//        //public static readonly DependencyProperty ShowTrafficProperty =
//        //    DependencyProperty.Register("ShowTraffic", typeof(bool), typeof(MyMap),
//        //    new PropertyMetadata(null, (d, e) => ((MyMap)d).InnerMap.ShowTraffic = (bool)e.NewValue));

//        // credentials... *** CHANGE THIS TO YOUR OWN KEY ***
//        private const string BingMapsApiKey = "AnDcCmjz_ocEbQ6PRT1H30G9pHmZFHGMRMIYwnXkmX9bDEEFX0AdMMsO9-UQVMZS";

//        // defines a standard zoom into street level...
//        private const int StandardZoom = 15;

//        public MyMap()
//        {
//            //this.ItemClick += MapView_ItemClick;
//        }


//        public object MapView_ItemClick { get; set; }
//    }
//}
//}