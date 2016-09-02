using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using QuicKE.Client.UI.Common;
using Windows.UI.Xaml.Navigation;

namespace QuicKE.Client.UI
{
    public class QuicKEPage : Page, IViewModelHost
    {
        private NavigationHelper navigationHelper;

        public QuicKEPage()
        {
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }

       

        #region NavigationHelper registration
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    this.navigationHelper.OnNavigatedTo(e);
        //}

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        Task IViewModelHost.ShowAlertAsync(string message)
        {
            return PageExtender.ShowAlertAsync(this, message).AsTask();
        }

        Task IViewModelHost.ShowAlertAsync(ErrorBucket errors)
        {
            return PageExtender.ShowAlertAsync(this, errors).AsTask();
        }

        public void ShowView(Type viewModelType, object parameter = null)
        {
            this.Frame.ShowView(viewModelType, parameter);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.navigationHelper.OnNavigatedTo(e);

            // ok...
            this.GetModel().Activated(e.Parameter);
        }
        public void GoBack()
        {
            if (Frame != null && Frame.CanGoBack)
                Frame.GoBack();
        }

        protected virtual void GoBack(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        Task IViewModelHost.ToggleProgressBar(bool toggle, string message)
        {
            return PageExtender.ToggleProgressBar(toggle, message);            
        }

    }
}
