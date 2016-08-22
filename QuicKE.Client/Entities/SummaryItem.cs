using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public class SummaryItem: WrappingModelItem<ServiceItem>
    {
        public SummaryItem(ServiceItem item)
            : base(item)
        {
        }

        public List<ServiceItem> Services { get { return this.GetValue<List<ServiceItem>>(); } private set { this.SetValue(value); } }
        public ExpertItem SelectedExpert
        {
            get
            {
                this.OnPropertyChanged("SelectExpertCommand");
                return this.GetValue<ExpertItem>();
            }
            set { this.SetValue(value); }
        }
        public int Id { get { return this.InnerItem.Id; } }
        public int NativeId { get { return this.InnerItem.NativeId; } }
        public string Name { get { return this.InnerItem.Name; } }
        public decimal Cost { get { return this.InnerItem.Cost; } }
    }
}
