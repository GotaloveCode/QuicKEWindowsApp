using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IHomePageViewModel:IViewModel
    {
         ICommand ProfileCommand
        {
            get;
        }

         ICommand LogoutCommand { get; }
         ICommand ViewHouseKeepingCommand
         {
             get;
         }

        ICommand ShowLocationCommand { get; }
    }
}
