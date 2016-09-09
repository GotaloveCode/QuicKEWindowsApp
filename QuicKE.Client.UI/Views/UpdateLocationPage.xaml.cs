

namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IUpdateLocationPageViewModel))]
    public sealed partial class UpdateLocationPage : QuicKEPage
    {
        public UpdateLocationPage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }

       
    }
}
