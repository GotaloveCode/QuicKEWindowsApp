using System.Globalization;
using Windows.Globalization;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;


namespace QuicKE.Client.UI
{
    [ViewModel(typeof(ILanguageSettingsPageViewModel))]
    public sealed partial class LanguageSettingsPage : QuicKEPage
    {
        public LanguageSettingsPage()
        {
            InitializeComponent();
            this.InitializeViewModel();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = (e.AddedItems[0] as ComboBoxItem).Content as string;
            
            if (selectedItem == "EN")
            {
                string lang = "en-US";    //default
                var culture = new CultureInfo(lang);
                ApplicationLanguages.PrimaryLanguageOverride = lang;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
              
            else if (selectedItem == "FR")
            {
                string lang = "fr";    //default
                var culture = new CultureInfo(lang);
                ApplicationLanguages.PrimaryLanguageOverride = lang;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            MessageDialog dialog = new MessageDialog("Your settings will be applied upon restarting the app","Language Changed");

            Frame.Navigate(typeof(HomePage));
        }
    }
}
