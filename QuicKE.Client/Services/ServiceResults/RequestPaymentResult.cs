namespace QuicKE.Client
{
    public class RequestPaymentResult : ErrorBucket
    {
        public string Data { get; private set; }

        public RequestPaymentResult(string data)
        {
            this.Data = data;
        }

        internal RequestPaymentResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
        
    }
}
