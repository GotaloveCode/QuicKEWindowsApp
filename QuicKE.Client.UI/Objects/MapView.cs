using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;


namespace QuicKE.Client.UI
{
    public class MapView : Grid, INotifyPropertyChanged
    {


        private MapControl _map;


        public MapView()
        {

            _map = new MapControl();


            this.Children.Add(_map);
        }

        public double Zoom
        {
            get
            {
                return _map.ZoomLevel;
            }
            set
            {
                _map.ZoomLevel = value;
                OnPropertyChanged("Zoom");
            }
        }

        public Geopoint Center
        {
            get
            {
                return _map.Center;

            }
            set
            {

                _map.Center = value;


                OnPropertyChanged("Center");
            }
        }

        public string Credentials
        {
            get
            {
                return string.Empty;
            }
            set
            {


                OnPropertyChanged("Credentials");
            }
        }

        public string MapServiceToken
        {
            get
            {
                return _map.MapServiceToken;

            }
            set
            {

                if (!string.IsNullOrEmpty(value))
                {
                    _map.MapServiceToken = value;
                }


                OnPropertyChanged("MapServiceToken");
            }
        }

        public bool ShowTraffic
        {
            get
            {
                return _map.TrafficFlowVisible;
            }
            set
            {

                _map.TrafficFlowVisible = value;

                OnPropertyChanged("ShowTraffic");
            }
        }

        public void SetView(BasicGeoposition center, double zoom)
        {

            _map.Center = new Geopoint(center);
            _map.ZoomLevel = zoom;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public void AddPushpin(BasicGeoposition location, string text)
        {
            var pin = new Grid()
            {
                Width = 24,
                Height = 24,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 24,
                Height = 24
            });

            pin.Children.Add(new TextBlock()
            {
                Text = text,
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center
            });

            MapControl.SetLocation(pin, new Geopoint(location));
            _map.Children.Add(pin);
        }
    }

}