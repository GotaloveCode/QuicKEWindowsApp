
namespace QuicKE.Client
{
    public class GetProfileResult:ErrorBucket
    {
        internal ProfileItem Profile { get; set; }

        internal GetProfileResult(ProfileItem item)
        {
            this.Profile = item;
        }

        internal GetProfileResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}

