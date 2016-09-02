using System.Collections.Generic;
using System.Windows.Input;

namespace QuicKE.Client
{
    // exposes the map of public binding properties on RegisterPage's view-model...
    public interface IRegisterPageViewModel : IViewModel
    {
        string FullName { get; }

        string Code { get; }

        string Password { get; }

        string Confirm { get; }

        string PhoneNumber { get; }

        ICommand SignUpCommand { get; }

        ICommand SignInCommand { get; }

        ICommand VerifyCommand { get; }        

        List<string> Locations { get; }

        string SelectedLocation { get; }



    }
}
