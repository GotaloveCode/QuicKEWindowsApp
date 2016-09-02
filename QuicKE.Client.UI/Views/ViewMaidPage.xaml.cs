

namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IViewMaidPageViewModel))]
    public sealed partial class ViewMaidPage : QuicKEPage
    {
        public ViewMaidPage()
        {
            InitializeComponent();
            this.InitializeViewModel();

        }
        
    }
}
