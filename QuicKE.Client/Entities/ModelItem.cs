using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    public abstract class ModelItem : INotifyPropertyChanged
    {
        private Dictionary<string, object> Values { get; set; }

        protected ModelItem()
        {
            this.Values = new Dictionary<string, object>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected T GetValue<T>([CallerMemberName] string name = null)
        {
            if (this.Values.ContainsKey(name))
                return (T)this.Values[name];
            else
                return default(T);
        }

        protected void SetValue(object value, [CallerMemberName] string name = null)
        {
            // set...
            try
            {
                this.Values[name] = value;

                // notify...
                this.OnPropertyChanged(new PropertyChangedEventArgs(name));
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            

           
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, e);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

            
        }
    }
}
