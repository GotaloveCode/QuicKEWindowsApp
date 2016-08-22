using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public class TicketPageViewModel : ViewModel, ITicketPageViewModel
    {
        public ObservableCollection<TicketItem> Items { get { return this.GetValue<ObservableCollection<TicketItem>>(); } set { this.SetValue(value); } }
        public TicketPageViewModel()
        {
        }
        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            // setup...
            this.Items = new ObservableCollection<TicketItem>();

        }
        private async Task DoRefresh(bool force)
        {
            // run...
            using (this.EnterBusy())
            {
                // update the local cache...
                if (force || await TicketItem.IsCacheEmpty())
                    await TicketItem.UpdateCacheFromServerAsync();

                // reload the items...
                await this.ReloadTicketsFromCacheAsync();
            }
        }

        private async Task ReloadTicketsFromCacheAsync()
        {
            // setup a load operation to populate the collection from the cache...
            using (this.EnterBusy())
            {
                var tickets = await TicketItem.GetAllFromCacheAsync();

                // update the model...
                this.Items.Clear();
                foreach (TicketItem ticket in tickets)
                    this.Items.Add(ticket);
            }
        }
        public override async void Activated(object args)
        {
            base.Activated(args);

            // update...
            await DoRefresh(false);
        }
    }
}
