

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TinyIoC;

namespace QuicKE.Client
{
    public class LocationPageViewModel : ViewModel, ILocationPageViewModel
    {
        // defines the username settings key...
        
        public ICommand SubmitCommand { get; private set; }
        public ObservableCollection<string> Locations { get; set; }
        public string SelectedLocation { get; set; }

        public LocationPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            Locations = new ObservableCollection<string>();
            SubmitCommand = new DelegateCommand((args) => Submit(args as CommandExecutionContext));

        }

 
        
        

        private async void Submit(CommandExecutionContext context)
        {
            await SettingItem.SetValueAsync("Location", SelectedLocation);
            // validate...
            ErrorBucket errors = new ErrorBucket();

            // get a handler...
            var proxy = TinyIoCContainer.Current.Resolve<ISignInServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.SignInAsync(this.Username, this.Password);
                if (!(result.HasErrors))
                {
                    
                    // show the reports page...
                    if (!errors.HasErrors)
                        this.Host.ShowView(typeof(IServicesPageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }


            // errors?
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }

       

        public override async void Activated(object args)
        {
            base.Activated(args);


            //if (MFundiRuntime.ServiceTypeID > 0)
            //{
            //    ServiceTypeId = MFundiRuntime.ServiceTypeID;
            //}

            //// update...
            //await DoRefresh(true, ServiceTypeId);
        }

    }
}
