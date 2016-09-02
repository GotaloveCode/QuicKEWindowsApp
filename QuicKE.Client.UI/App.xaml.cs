using System;
using TinyIoC;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace QuicKE.Client.UI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        //private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            //this.Resuming += App_Resuming;
        }
        //async void App_Resuming(object sender, object e)
        //{
        //    if (MFundiRuntime.HasLogonToken)
        //        await MFundiRuntime.SetupNotificationChannelAsync();
        //}
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            // start...
             MFundiRuntime.Start("Client");


            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                Common.SuspensionManager.RegisterFrame(rootFrame, "appFrame");

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    await Common.SuspensionManager.RestoreAsync();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the home page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (ApplicationData.Current.LocalSettings.Values["LoggedIn"] != null &&
                    ApplicationData.Current.LocalSettings.Values["LoggedIn"].ToString() == "True")
                {
                        if (!rootFrame.Navigate(typeof(HomePage), e.Arguments))
                            throw new Exception("Failed to create initial page");

                }
                else
                {
                    if (!rootFrame.Navigate(typeof(RegisterPage), e.Arguments))
                      throw new Exception("Failed to create initial page");
                }
               
            }

            // Ensure the current window is active
            Window.Current.Activate();
            // settings...
            
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
               /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            await Common.SuspensionManager.SaveAsync();

            deferral.Complete();
        }
    }
}