using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace QuicKE.Client.UI
{

    [ViewModel(typeof(IHomePageViewModel))]
    public sealed partial class HomePage : QuicKEPage
    {
        public HomePage()
        {
            this.InitializeComponent();

            // obtain a real instance of a model...
            this.InitializeViewModel();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            MySettingsFlyOut.ShowAt(senderElement);

        }




    }
}
