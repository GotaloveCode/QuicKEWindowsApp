namespace QuicKE.Client.UI
{
    [ViewModel(typeof(INewPassPageViewModel))]
    public sealed partial class NewPassPage : QuicKEPage
    {
        public NewPassPage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }

        
    }
}
