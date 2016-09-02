namespace QuicKE.Client
{
    public class SignOutResult : ErrorBucket
    {
        public string Data { get; private set; }
        public string Status { get; private set; }

        public SignOutResult(string data,string status)
        {
            Data = data;
            Status = status;
        }

        internal SignOutResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
