using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IEvaluationPageViewModel : IViewModel
    {
        ICommand SubmitCommand { get; }
        ICommand ExitCommand { get; }

    }
}
