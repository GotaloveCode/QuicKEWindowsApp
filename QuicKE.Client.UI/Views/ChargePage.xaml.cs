

namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IChargePageViewModel))]
    public sealed partial class ChargePage : QuicKEPage
    {
        public ChargePage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
        }

        
    }
}
