

namespace QuicKE.Client.UI
{
    [ViewModel(typeof(ILocationPageViewModel))]
    public sealed partial class LocationPage : MFundiPage
    {
        public LocationPage()
        {
            this.InitializeComponent();
            // obtain a real instance of a model...
            this.InitializeViewModel();
        }

        
    }
}
