using System.Windows.Input;
using TinyIoC;
using Windows.Storage;

namespace QuicKE.Client
{
    public class EvaluationPageViewModel : ViewModel, IEvaluationPageViewModel
    {
        public double Rating { get { return GetValue<double>(); } set { SetValue(value); } }
        public string Review { get { return GetValue<string>(); } set { SetValue(value); } }
        public string TicketID { get { return GetValue<string>(); } set { SetValue(value); } }
        ErrorBucket errors = new ErrorBucket();
       
        public ICommand SubmitCommand
        {
            get; private set;
        }

        public EvaluationPageViewModel()
        {
        }
       

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            SubmitCommand = new DelegateCommand((args) => Evaluate(args as CommandExecutionContext));
        }

        public override void Activated(object args)
        {
            base.Activated(args);

        }


        private async void Evaluate(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
            if(Rating < 0.5 || Review == null)
            {
                await Host.ShowAlertAsync("Please provide a rating and comment");
            }
            else
            {
                //servicecall 
                var proxy = TinyIoCContainer.Current.Resolve<IEvaluateServiceProxy>();
                // call...
                using (EnterBusy())
                {
                    await Host.ToggleProgressBar(true, "Sending your comments...");

                    var result = await proxy.EvaluateAsync(Rating, Review, MFundiRuntime.TicketID);

                    if (!(result.HasErrors))
                    {
                        MFundiRuntime.TicketID = null;
                        await Host.ShowAlertAsync(result.Message);
                        Host.ShowView(typeof(IHomePageViewModel));
                    }
                    else
                    {
                        errors.CopyFrom(result);
                    }

                    await Host.ToggleProgressBar(false);
                }

                if (errors.HasErrors)
                    await this.Host.ShowAlertAsync(errors);
            }
        }



    }
}
