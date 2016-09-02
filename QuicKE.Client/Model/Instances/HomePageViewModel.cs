using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;
using Windows.UI.Popups;

namespace QuicKE.Client
{
    public class HomePageViewModel:ViewModel, IHomePageViewModel
    {
        //public ICommand ProfileCommand { get; private set; }
        public ICommand ViewMonthlyServiceCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand ViewDayServiceCommand { get; private set; }
        public ICommand ChangeLocationCommand { get; private set; }
        public ICommand TaskDoneCommand { get; private set; }
        public int TicketId { get { return GetValue<int>(); } set { SetValue(value); } }
        public bool HasPendingTask { get { return GetValue<bool>(); } set { SetValue(value); } }
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public HomePageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            ViewMonthlyServiceCommand = new DelegateCommand((e) =>
            {
                //set servicetypeid
                MFundiRuntime.ServiceTypeID = 2;
              
                Host.ShowView(typeof(IChargePageViewModel));
            }); 
            ViewDayServiceCommand = new DelegateCommand((e) =>
            {
              
                MFundiRuntime.ServiceTypeID = 1;
                Host.ShowView(typeof(IChargePageViewModel));
            }); 
            LogoutCommand = new DelegateCommand(async (e) =>
            { // get the location...
                await LogOut();
            });

            TaskDoneCommand = new DelegateCommand(async (e) =>
            { // get the location...
                await TaskDone();
            });


        }

        private async Task TaskDone()
        {
            MessageDialog dialog = new MessageDialog("Please Mark Task as done","Task Complete");

            dialog.Commands.Add(new UICommand("Cancel"));

            dialog.Commands.Add(new UICommand("Confirm", async delegate (IUICommand command)
            {
                await SetDone();

            }));

            await dialog.ShowAsync();
        }

        private async Task SetDone()
        {
            var proxy = TinyIoCContainer.Current.Resolve<ITaskCompleteServiceProxy>();

            using (EnterBusy())
            {
                var result = await proxy.TaskCompleteAsync(TicketId);




            }
        }

        private async Task LogOut()
        {
            ErrorBucket errors = new ErrorBucket();
            var proxy = TinyIoCContainer.Current.Resolve<ILogOutServiceProxy>();
            using (EnterBusy())
            {
                var result = await proxy.LogOutAsync();
                if (!(result.HasErrors))
                {

                    localSettings.Values.Remove("LoggedIn");
                    localSettings.Values.Remove("LogonToken");

                    MFundiRuntime.LogonToken = null;
                    
                    await Host.ShowAlertAsync("Logged Out Successfully");

                    Host.ShowView(typeof(IRegisterPageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }
            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }

      
        public override async void Activated(object args)
        {
            base.Activated(args);

            var values = ApplicationData.Current.LocalSettings.Values;

            if (values.ContainsKey("TicketId"))
            {
                TicketId = int.Parse(localSettings.Values["TicketId"].ToString());
                HasPendingTask = true;
            }
            else
                HasPendingTask = false;

            ////remove and leave mfundiruntime location code in live
            if (!values.ContainsKey("Location"))
            {
                await Host.ShowAlertAsync("Please set your Location");
                Host.ShowView(typeof(IUpdateLocationPageViewModel));
            }
           
        }

        public async Task GetCharges()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IGetChargesServiceProxy>();

            using (EnterBusy())
            {
                var result = await proxy.GetServicesAsync();           



                //if (!(result.HasErrors))
                //{
                //    foreach (var item in result.Services)
                //    {
                //        if (item.NativeId == MFundiRuntime.ServiceTypeID)
                //            Cost = item.Cost.ToString();

                //    }

                //}
                //else
                //    errors.CopyFrom(result);
                //if (errors.HasErrors)
                //    await Host.ShowAlertAsync(errors);
            }
        }


    }
}
