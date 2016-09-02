namespace QuicKE.Client
{
    public class GetMaidResult : ErrorBucket
    {
        internal MaidItem Maid { get; set; }

        internal GetMaidResult(MaidItem item)
        {
            Maid = item;
        }

        internal GetMaidResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}

