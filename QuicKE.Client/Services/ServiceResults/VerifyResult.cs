namespace QuicKE.Client
{
     public class VerifyResult : ErrorBucket
    {
        public string Status { get; private set; }

        public VerifyResult(string status)
        {
            Status = status;
        }

        internal VerifyResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
