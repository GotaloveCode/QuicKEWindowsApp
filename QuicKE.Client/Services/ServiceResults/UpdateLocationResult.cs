namespace QuicKE.Client
{

    public class UpdateLocationResult : ErrorBucket
    {

        public string Status { get; private set; }
        public string Message { get; private set; }

        public UpdateLocationResult(string status,string message)
        {
            Message = message;
            Status = status;
        }

        internal UpdateLocationResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
