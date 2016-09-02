namespace QuicKE.Client
{
     public class ConfirmPaymentResult : ErrorBucket
    {

        public string Status { get; private set; }

        public ConfirmPaymentResult( string status)
        {

            this.Status = status;
        }

        internal ConfirmPaymentResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}