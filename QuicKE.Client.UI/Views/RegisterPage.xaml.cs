namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IRegisterPageViewModel))]
    public sealed partial class RegisterPage : QuicKEPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }
      }
}
