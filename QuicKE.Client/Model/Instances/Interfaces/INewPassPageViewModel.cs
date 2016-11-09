using System.Windows.Input;

namespace QuicKE.Client
{

    public interface INewPassPageViewModel : IViewModel
    {
        string PhoneNumber { get; }
        string Code { get; }
        string Password { get; }
        string Confirm { get; }
        ICommand SubmitCommand { get; }
    }
}
