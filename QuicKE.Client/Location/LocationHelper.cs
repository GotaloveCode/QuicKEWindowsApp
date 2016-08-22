using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.System;
using QuickMVVM;
using Newtonsoft.Json;
namespace QuicKE.Client
{
    public static class LocationHelper
    {
        
        public static async Task<LocationResult> GetCurrentLocationAsync()
        {
            try
            {
                var locator = new Geolocator();
                locator.DesiredAccuracy = PositionAccuracy.High;
                var position = await locator.GetGeopositionAsync();

                // return...
                return new LocationResult(position);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine("Geolocation access denied: " + ex.ToString());                
                return new LocationResult(LocationResultCode.AccessDenied);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Geolocation failure: " + ex.ToString());
                return new LocationResult(LocationResultCode.UnknownError);
            }
        }



        public static async Task<string> RetrieveFormatedAddress(string lat, string lng)
        {
            string baseUri = "http://maps.googleapis.com/maps/api/" +
                         "geocode/json?latlng={0},{1}&sensor=false";
            string requestUri = string.Format(baseUri, lat, lng);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(requestUri);
                string outputJson = await response.Content.ReadAsStringAsync();


                MapObject mapobj = JsonConvert.DeserializeObject<MapObject>(outputJson);
                Result res = new Result();
                res = mapobj.results.FirstOrDefault();

                return res.formatted_address;
            }
        }
        
    }
}
