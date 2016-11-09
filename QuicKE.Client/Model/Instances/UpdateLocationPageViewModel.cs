using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace QuicKE.Client
{

    public class UpdateLocationPageViewModel : ViewModel, IUpdateLocationPageViewModel
    {

        public ICommand SubmitCommand { get; private set; }
        public ICommand UpdateListCommand { get; private set; }
        private List<string> locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public List<string> Locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public string SelectedLocation { get { return GetValue<string>();} set { SetValue(value); }}
        ResourceLoader res = ResourceLoader.GetForCurrentView();
        

        public UpdateLocationPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            Locations = new List<string>();
            locations = new List<string>();
            
            SubmitCommand = new DelegateCommand((args) => Submit(args as CommandExecutionContext));
            UpdateListCommand = new DelegateCommand((args) => Suggest(args as CommandExecutionContext));
        }

        //autosuggest filter
        private void Suggest(CommandExecutionContext context) 
        {
            if (context == null)
                context = new CommandExecutionContext();
            if (!string.IsNullOrEmpty(SelectedLocation))
                Locations = locations.Where(x => x.ToLower().Contains(SelectedLocation.ToLower())).ToList();
        }

        //verify number
        private async void Submit(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            if (string.IsNullOrEmpty(SelectedLocation))
            {                
                await Host.ShowAlertAsync(res.GetString("cmbSelectLocation/PlaceholderText"));               
            }                
            else
            {
                var proxy = TinyIoCContainer.Current.Resolve<IUpdateLocationServiceProxy>();


                using (EnterBusy())
                {
                    var result = await proxy.UpdateLocationAsync(SelectedLocation);
                    ErrorBucket errors = new ErrorBucket();

                    // ok?
                    if (!(result.HasErrors))
                    {
                        if (result.Status != "success")
                        {
                            errors.CopyFrom(result);
                        }
                        else
                        {
                            await Host.ShowAlertAsync(result.Message);
                            ApplicationData.Current.LocalSettings.Values["Location"] = SelectedLocation;
                            Host.ShowView(typeof(IHomePageViewModel));
                        }
                           
                    }
                    else
                        errors.CopyFrom(result);

                    if (errors.HasErrors)
                        await Host.ShowAlertAsync(errors.GetErrorsAsString());
                }
            }

        }
      
     
        public async override void Activated(object args)
        {
            base.Activated(args);

            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, res.GetString("Loading"));
                await LoadLocations();
                await Host.ToggleProgressBar(false);                
            }
        }

        public async Task LoadLocations()
        {
            ErrorBucket errors = new ErrorBucket();

            var proxy = TinyIoCContainer.Current.Resolve<IGetLocationsServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.GetLocationsAsync();
                if (!(result.HasErrors))
                {                   
                    foreach (var item in result.Locations)
                    {
                        Locations.Add(item.name);
                        locations.Add(item.name);
                    }
                    
                }
                else
                    errors.CopyFrom(result);
            }

            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);

        }

    }
}
