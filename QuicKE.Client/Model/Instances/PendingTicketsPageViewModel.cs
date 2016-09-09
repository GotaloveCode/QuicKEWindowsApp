using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;

namespace QuicKE.Client
{
    public class PendingTicketsPageViewModel : ViewModel, IPendingTicketsPageViewModel
    {
        //public string Name { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand SubmitCommand { get; private set; }

        //public int TicketID { get { return GetValue<int>(); } set { SetValue(value); } }
        public TicketItem SelectedTicket { get; set; }

        public ObservableCollection<TicketItem> Items { get; set; }

        public int TicketID { get { return GetValue<int>(); } set { SetValue(value); } }

        ErrorBucket errors = new ErrorBucket();

        public PendingTicketsPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            SubmitCommand = 
                new DelegateCommand(async (e) => {

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

            Items = new ObservableCollection<TicketItem>();
            SelectedTicket = new TicketItem();

        }

        //private async Task MarkComplete(e)
        //{
            
            

        //    if (!string.IsNullOrEmpty(SelectedTicket.Name))
        //    {
        //        TicketID = SelectedTicket.Id;

        //    }

        //    var proxy = TinyIoCContainer.Current.Resolve<ITaskCompleteServiceProxy>();

        //    using (EnterBusy())
        //    {
        //        var result = await proxy.TaskCompleteAsync(TicketID);

        //        if (!(result.HasErrors))
        //        {
        //            if (result.Status != "success")
        //            {
        //                errors.CopyFrom(result);
        //            }
        //            else
        //            {
        //                var toast = new ToastNotificationBuilder(new string[] { result.Message });
        //                toast.Update();
        //                Host.ShowView(typeof(IEvaluationPageViewModel));
        //            }
        //        }
        //        else
        //        {
        //            errors.CopyFrom(result);
        //        }

        //        if (errors.HasErrors)
        //            await Host.ShowAlertAsync(errors.GetErrorsAsString());
        //    }
        //}

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

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors.GetErrorsAsString());
            }
        }


    }
}
