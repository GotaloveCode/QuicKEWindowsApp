namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IHistoryPageViewModel))]
    public sealed partial class HistoryPage : QuicKEPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }

        
    }
}
