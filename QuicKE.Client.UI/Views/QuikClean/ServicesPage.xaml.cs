
namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IServicesPageViewModel))]
    public sealed partial class ServicesPage : MFundiPage
    {
        public ServicesPage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
        }


    }
}
