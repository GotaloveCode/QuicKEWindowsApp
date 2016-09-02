using System.Windows.Input;

namespace QuicKE.Client
{
    public class EvaluationPageViewModel : ViewModel, IEvaluationPageViewModel
    {
        public EvaluationPageViewModel()
        {
        }
        public ICommand ExitCommand
        {
            get; private set;
        }


        public ICommand SubmitCommand
        {
            get; private set;
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
        }

        public override void Activated(object args)
        {
            base.Activated(args);

        }

    }
}
