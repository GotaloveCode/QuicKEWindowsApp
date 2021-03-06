﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using TinyIoC;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace QuicKE.Client.UI
{
    internal static class PageExtender
    {
        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this IViewModelHost page, ErrorBucket errors)
        {
            return ShowAlertAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this IViewModelHost page, string message)
        {
            // show...
            MessageDialog dialog = new MessageDialog(message != null ? message : string.Empty);
            return dialog.ShowAsync();
        }

        public static async Task ToggleProgressBar(bool toggle, string message = "")
        {
            var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            if (toggle)
            {
                statusBar.ProgressIndicator.Text = message;
                await statusBar.ProgressIndicator.ShowAsync();
            }
            else
            {
                await statusBar.ProgressIndicator.HideAsync();
            }
        }


        internal static void InitializeViewModel(this IViewModelHost host, IViewModel model = null)
        {
            // if we don't get given a model?
            if (model == null)
            {
                var attr = (ViewModelAttribute)host.GetType().GetTypeInfo().GetCustomAttribute<ViewModelAttribute>();
                if (attr != null)
                    model = (IViewModel)TinyIoCContainer.Current.Resolve(attr.ViewModelInterfaceType);
                else
                    throw new InvalidOperationException(string.Format("Page '{0}' is not decorated with ViewModelAttribute."));
            }

            // setup...
            model.Initialize((IViewModelHost)host);
            ((FrameworkElement)host).DataContext = model;
        }
        internal static IViewModel GetModel(this IViewModelHost page)
        {
            return ((Control)page).DataContext as IViewModel;
        }
    }
}
