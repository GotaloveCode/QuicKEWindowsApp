namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IProfilePageViewModel))]
    public sealed partial class ProfilePage : QuicKEPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }

        
    }
}
