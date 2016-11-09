namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IForgotPassPageViewModel))]
    public sealed partial class ForgotPasswordPage : QuicKEPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }

    }
}
