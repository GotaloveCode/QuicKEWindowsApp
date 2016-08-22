using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.UI.Popups;

namespace QuicKE.Client
{
    public class SummaryPageViewModel : ViewModel, ISummaryPageViewModel
    {
        public ICommand ConfirmCommand { get; private set; }
        public string Name { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public int Age { get { return this.GetValue<int>(); } set { this.SetValue(value); } }
        public string IDNumber { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Badge { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public int ExpertID { get { return this.GetValue<int>(); } set { this.SetValue(value); } }
        public string TotalCost { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Location { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string ServiceArray { get; set; }
        public ObservableCollection<ServiceItem> Services { get { return this.GetValue<ObservableCollection<ServiceItem>>(); } set { this.SetValue(value); } }

        ErrorBucket errors = new ErrorBucket();

        public SummaryPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            // setup...
            this.Services = new ObservableCollection<ServiceItem>();
            this.ConfirmCommand = new DelegateCommand((args) => GetSelected(args as CommandExecutionContext));
        }


        private async void GetSelected(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
            string message = string.Format("Send {0} to {1} for {2}", Name, Location, TotalCost);
            MessageDialog dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("Cancel"));
            dialog.Commands.Add(new UICommand("OK", async delegate(IUICommand command)
                {
                   var proxy = TinyIoCContainer.Current.Resolve<ICreateTicketServiceProxy>();
                   // call...
                   using (this.EnterBusy())
                   {
                       ServiceArray = "[" + ServiceArray + "]";
                       string loc = await UserItem.GetValueAsync("location");
                       var result = await proxy.CreateTicketAsync(ServiceArray, ExpertID, loc);
                       if (!(result.HasErrors))
                       {
                           dialog = new MessageDialog("Confirmed!\r\n Our Expert has been sent your address.\r\nThankyou for using our services");
                           await dialog.ShowAsync();
                           //var toast = new ToastNotificationBuilder(new string[] { "Confirmed!", "Our Expert has been sent your address.Thankyou for using our services" });
                           //toast.Update();
                           // show the home page...
                           this.Host.ShowView(typeof(IHomePageViewModel));
                       }
                       else
                           errors.CopyFrom(result);
                   }

                   // errors?
                   if (errors.HasErrors)
                       await this.Host.ShowAlertAsync(errors);
               }));
            await dialog.ShowAsync();
        }



        public override async void Activated(object args)
        {
            base.Activated(args);

           using (this.EnterBusy())
            {
                this.ServiceArray = await SettingItem.GetValueAsync("ServiceArray");
                if (string.IsNullOrEmpty(ServiceArray))
                {
                    errors.AddError("Could not get the selected service!Navigating back to services page");
                    await this.Host.ShowAlertAsync(errors);
                    // show the services page...
                    this.Host.ShowView(typeof(IServicesPageViewModel));
                }
                else
                {
                    List<int> ServiceIds = ServiceArray.Split(',').Select(int.Parse).ToList();
                    var conn = MFundiRuntime.GetSystemDatabase();

                    IEnumerable<ServiceItem> servs = await conn.Table<ServiceItem>().Where(v => ServiceIds.Contains(v.NativeId)).ToListAsync();
                    foreach (ServiceItem s in servs)
                        this.Services.Add(s);
                    this.TotalCost = string.Format("KES. {0}", Services.Select(x => x.Cost).Sum().ToString());
                    string id = await SettingItem.GetValueAsync("ExpertID");
                    int expertid = int.Parse(id);
                    
                    ExpertItem Expert = await conn.Table<ExpertItem>().Where(v => v.NativeId == expertid).FirstOrDefaultAsync();
                    this.ExpertID = Expert.NativeId;
                    this.Name = Expert.name;
                    this.Age = Expert.age;
                    this.Badge = Expert.badge;
                    this.IDNumber = Expert.id_number;
                    this.Location = Expert.destination;
                }
            }

        }

    }
}
