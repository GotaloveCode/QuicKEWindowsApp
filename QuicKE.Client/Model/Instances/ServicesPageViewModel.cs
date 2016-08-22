using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;

namespace QuicKE.Client
{
    public class ServicesPageViewModel : ViewModel, IServicesPageViewModel
    {
        public ObservableCollection<ServiceItem> Items { get; set; }     
        public ICommand ContinueCommand { get; private set; }
        public ICommand ItemClickedCommand { get; private set; }
        private int ServiceTypeId { get; set; }

        public string Title { get; set; }

        
        public ServicesPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            // setup...
            this.Items = new ObservableCollection<ServiceItem>();
            this.Title = MFundiRuntime.Title;
            
            this.ContinueCommand = new DelegateCommand((args) => GetSelected(args as CommandExecutionContext));
         }

    
        private async void GetSelected(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
          
            decimal TotalCost = this.Items.Where(x => x.IsSelected == true).Select(x=>x.Cost).Sum();
            List<int> serviceids = this.Items.Where(x => x.IsSelected == true).Select(x => x.NativeId).ToList();
            string ServiceArray = string.Join(",", serviceids);

            await SettingItem.SetValueAsync("TotalCost", TotalCost.ToString());
            await SettingItem.SetValueAsync("ServiceArray", ServiceArray);
           
            if (serviceids.Count() > 0)
            {
                var toast = new ToastNotificationBuilder(new string[] { "Total Cost:" + TotalCost });
                toast.ImageUri = "ms-appx:///Assets/Toast.jpg";
                toast.Update();
            }
            //Quickpiki?
            if (MFundiRuntime.ServiceTypeID == 2)
            {
                Host.ShowView(typeof(IHomePageViewModel));
            }
             ErrorBucket errors = new ErrorBucket();
              // get a handler...
            var proxy = TinyIoCContainer.Current.Resolve<IGetExpertsServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.GetExpertsAsync();
                if (!(result.HasErrors))
                {
                   // show the experts page...
                    Host.ShowView(typeof(ISelectExpertPageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }


            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);


        }

        private async Task DoRefresh(bool force,int servicetypeid=1)
        {
            // run...
            using (EnterBusy())
            {
                // update the local cache...
                if (force || await ServiceItem.IsCacheEmpty(servicetypeid))
                    await ServiceItem.UpdateCacheFromServerAsync(servicetypeid);

                // reload the items...
                await ReloadServicesFromCacheAsync(servicetypeid);
            }
        }

        private async Task ReloadServicesFromCacheAsync(int servicetypeid)
        {
            // setup a load operation to populate the collection from the cache...
            using (this.EnterBusy())
            {
                var services = await ServiceItem.GetAllFromCacheAsync(servicetypeid);

                // update the model...
                if (Items.Count > 0)
                    Items.Clear();
                foreach (ServiceItem service in services)
                    Items.Add(service);
            }
        }

        public override async void Activated(object args)
        {
            base.Activated(args);


            if(MFundiRuntime.ServiceTypeID>0)
            {
                ServiceTypeId = MFundiRuntime.ServiceTypeID;
            }

            // update...
            await DoRefresh(true, ServiceTypeId);
        }

    }

}
