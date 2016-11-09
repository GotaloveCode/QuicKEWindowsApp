using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IForgotPassPageViewModel: IViewModel
    {
        string PhoneNumber { get; }

        ICommand ForgotCommand { get; }
    }
}
