using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;

namespace QuicKE.Client.Model.Instances
{
      // concrete implementation of the RegisterPage's view-model...
    public class UpdateLocationPageViewModel : ViewModel, IUpdateLocationPageViewModel
    {

        public ICommand SubmitCommand { get; private set; }
       

        public List<string> Locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public string SelectedLocation { get { return GetValue<string>(); } set { SetValue(value); } }
        ErrorBucket errors = new ErrorBucket();



        public UpdateLocationPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            Locations = new List<string>();
            SubmitCommand = new DelegateCommand((args) => Submit(args as CommandExecutionContext));            
        }

        //verify number
        private async void Submit(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            if (string.IsNullOrEmpty(SelectedLocation))
                await Host.ShowAlertAsync("Select a Location");
            else
            {
                var proxy = TinyIoCContainer.Current.Resolve<IVerifyServiceProxy>();


                using (EnterBusy())
                {
                    var result = await proxy.VerifyAsync(SelectedLocation);

                    // ok?
                    if (!(result.HasErrors))
                    {
                        if (result.Status != "success")
                            errors.CopyFrom(result);
                        await SettingItem.SetValueAsync("Location", SelectedLocation);
                    }
                    else
                        errors.CopyFrom(result);

                    if (errors.HasErrors)
                        await Host.ShowAlertAsync(errors.GetErrorsAsString());
                }
            }

        }
      
            
        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(SelectedLocation))
                errors.AddError("Select a Location");

        }

        public async override void Activated(object args)
        {
            base.Activated(args);
            using (EnterBusy())
            {
              await LoadLocations();
               
            }
     
        }

        async Task LoadLocations()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IGetLocationsServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.GetLocationsAsync();
                if (!(result.HasErrors))
                {
                    foreach (var item in result.Locations)
                        Locations.Add(item.name);
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
