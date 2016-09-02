namespace QuicKE.Client.UI
{
    [ViewModel(typeof(IEvaluationPageViewModel))]
    public sealed partial class EvaluationPage : QuicKEPage
    {
        public EvaluationPage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
        }

       
    }
}
