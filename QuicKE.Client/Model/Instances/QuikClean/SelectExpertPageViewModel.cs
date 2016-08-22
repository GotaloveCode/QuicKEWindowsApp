using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public class SelectExpertPageViewModel : ViewModel, ISelectExpertPageViewModel
    {
         public ObservableCollection<ExpertItem> Experts { get; set; }
         public ExpertItem SelectedExpert { get; set; }
            
        public ICommand SelectExpertCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        public SelectExpertPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            // setup...
            this.Experts = new ObservableCollection<ExpertItem>();
            this.SelectedExpert = new ExpertItem();


            this.SelectExpertCommand = new DelegateCommand(async (e) => {
                var conn = MFundiRuntime.GetSystemDatabase();

            SelectedExpert = await conn.Table<ExpertItem>().Where(v => v.NativeId == (int)e).FirstOrDefaultAsync();
            await SettingItem.SetValueAsync("ExpertID", SelectedExpert.NativeId.ToString());
            this.Host.ShowView(typeof(ISummaryPageViewModel));
            });
            
        }

    
       private async Task DoRefresh(bool force)
        {
            // run...
            using (this.EnterBusy())
            {
                // update the local cache...
                if (force || await ExpertItem.IsCacheEmpty())
                    await ExpertItem.UpdateCacheFromServerAsync();

                // reload the items...
                await this.ReloadServicesFromCacheAsync();
            }
        }

        private async Task ReloadServicesFromCacheAsync()
        {
            // setup a load operation to populate the collection from the cache...
            using (this.EnterBusy())
            {
                var experts = await ExpertItem.GetAllFromCacheAsync();

                // update the model...
                if (this.Experts.Count > 0)
                    this.Experts.Clear();
                foreach (ExpertItem expert in experts)
                    this.Experts.Add(expert);
            }
        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            // update...
            await DoRefresh(true);
        }
    }
}
