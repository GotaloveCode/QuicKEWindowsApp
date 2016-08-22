using System;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace QuicKE.Client.UI
{
    public static class FrameExtender
    {
        public static void ShowView(this Frame frame, Type viewModelType, object parameter = null)
        {
            // get the concrete handler and as the frame to flip... (note we use the ViewFactory,
            // not the ViewModelFactory here...)

            foreach (var type in typeof(FrameExtender).GetTypeInfo().Assembly.GetTypes())
            {
                var attr = (ViewModelAttribute)type.GetCustomAttribute<ViewModelAttribute>();
                if (attr != null && viewModelType.IsAssignableFrom(attr.ViewModelInterfaceType))
                {
                    // show...
                    frame.Navigate(type, parameter);
                }
            }
        }
    }
}
