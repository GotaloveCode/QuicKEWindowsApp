
namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IMaidsPageViewModel))]
    public sealed partial class MaidsPage : MFundiPage
    {      

        public MaidsPage()
        {
            InitializeComponent();
            // obtain a real instance of a model...
            this.InitializeViewModel();
        }

     
    }
}
