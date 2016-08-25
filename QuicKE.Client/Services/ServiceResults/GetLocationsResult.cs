using System.Collections.Generic;

namespace QuicKE.Client
{
    public class GetLocationsResult : ErrorBucket
    {
        internal List<LocationItem> Locations { get; set; }

        internal GetLocationsResult(IEnumerable<LocationItem> items)
        {
            Locations = new List<LocationItem>();
            Locations.AddRange(items);
        }

        internal GetLocationsResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
