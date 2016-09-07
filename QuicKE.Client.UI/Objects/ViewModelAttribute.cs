using System;

namespace QuicKE.Client.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewModelAttribute : Attribute
    {
        public Type ViewModelInterfaceType { get; private set; }

        public ViewModelAttribute(Type viewModelInterfaceType)
        {
            this.ViewModelInterfaceType = viewModelInterfaceType;
        }
    }
}
