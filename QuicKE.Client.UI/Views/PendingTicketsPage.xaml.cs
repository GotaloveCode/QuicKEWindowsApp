
namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IPendingTicketsPageViewModel))]
    public sealed partial class PendingTicketsPage : QuicKEPage
    {
        public PendingTicketsPage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
        }


    }
}
