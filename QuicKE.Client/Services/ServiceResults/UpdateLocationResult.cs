namespace QuicKE.Client
{

    public class UpdateLocationResult : ErrorBucket
    {

        public string Status { get; private set; }

        public UpdateLocationResult(string status)
        {

            this.Status = status;
        }

        internal UpdateLocationResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
