
namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IServicesPageViewModel))]
    public sealed partial class ServicesPage : QuicKEPage
    {
        public ServicesPage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
        }


    }
}
