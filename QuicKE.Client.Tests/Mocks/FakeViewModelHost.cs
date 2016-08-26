using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace QuicKE.Client.Tests
{
    internal class FakeViewModelHost : IViewModelHost

    {

        public int NumMessagesShown { get; private set; }

        public string LastMessage { get; private set; }

        internal FakeViewModelHost()

        {

        }

        Task IViewModelHost.ShowAlertAsync(ErrorBucket errors)
        {
            return ShowAlertAsync(errors.GetErrorsAsString());
        }

        Task IViewModelHost.ShowAlertAsync(string message)
        {
            throw new NotImplementedException();
        }
       

        public IViewModelHost ShowAlertAsync(string message)

        {

            // update the number of messages...

            this.NumMessagesShown++;

            this.LastMessage = message;

            // return...

            return Task.FromResult<IUICommand>(null).AsAsyncOperation<IUICommand>();

        }

        public void ShowView(Type viewModelInterfaceType, object args = null)

        {

            throw new NotImplementedException("This operation has not been implemented.");

        }

        public void ShowAppBar()

        {

            throw new NotImplementedException("This operation has not been implemented.");

        }

        public void HideAppBar()

        {

            throw new NotImplementedException("This operation has not been implemented.");

        }

        public void GoBack()

        {

            throw new NotImplementedException("This operation has not been implemented.");

        }

      
    }
}
