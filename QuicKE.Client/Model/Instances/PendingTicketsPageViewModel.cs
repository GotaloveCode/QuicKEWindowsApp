using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.ApplicationModel.Resources;

namespace QuicKE.Client
{
    public class PendingTicketsPageViewModel : ViewModel, IPendingTicketsPageViewModel
    {

        public ICommand SubmitCommand { get; private set; }

        public TicketItem SelectedTicket { get; set; }

        public ObservableCollection<TicketItem> Items { get; set; }

        public int TicketID { get { return GetValue<int>(); } set { SetValue(value); } }

        ResourceLoader res = ResourceLoader.GetForCurrentView();



        public PendingTicketsPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            ErrorBucket errors = new ErrorBucket();
            Items = new ObservableCollection<TicketItem>();
            SelectedTicket = new TicketItem();
            SubmitCommand = new DelegateCommand(async (e) =>
            {
                TicketID = (int)e;

                MFundiRuntime.TicketID = TicketID.ToString();
                
                var proxy = TinyIoCContainer.Current.Resolve<ITaskCompleteServiceProxy>();

                using (EnterBusy())
                {
                    var result = await proxy.TaskCompleteAsync(TicketID);

                    if (!(result.HasErrors))
                    {
                        if (result.Status != "success")
                        {
                            errors.CopyFrom(result);
                        }
                        else
                        {
                            var toast = new ToastNotificationBuilder(new string[] { result.Message });
                            toast.Update();
                            Host.ShowView(typeof(IEvaluationPageViewModel));
                        }
                    }
                    else
                    {
                        errors.CopyFrom(result);
                    }

                    if (errors.HasErrors)
                        await Host.ShowAlertAsync(errors.GetErrorsAsString());
                }
            });
        }


        public async override void Activated(object args)
        {
            base.Activated(args);
            await GetCurrentTickets();             
            
        }

        private async Task GetCurrentTickets()
        {

            ErrorBucket errors = new ErrorBucket();

            var proxy = TinyIoCContainer.Current.Resolve<IGetPendingTicketsServiceProxy>();

            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, res.GetString("Loading"));

                var result = await proxy.GetTicketAsync();

                if (!(result.HasErrors))
                {
                    foreach (var item in result.tickets)
                    {
                        Items.Add(item);
                    }

                }
                else
                {
                    errors.CopyFrom(result);
                }
                await Host.ToggleProgressBar(false);

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors.GetErrorsAsString());
            }
        }

       
    }
}
