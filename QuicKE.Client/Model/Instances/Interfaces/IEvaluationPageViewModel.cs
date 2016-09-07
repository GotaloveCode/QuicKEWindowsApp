using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IEvaluationPageViewModel : IViewModel
    {
        ICommand SubmitCommand { get; }
        double Rating { get; }
        string Review { get; }
    }
}
