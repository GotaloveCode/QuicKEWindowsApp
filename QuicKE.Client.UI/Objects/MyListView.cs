﻿using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuicKE.Client.UI
{
    public class MyListView : ListView
    {
        // as per the grid...
        public static readonly DependencyProperty ItemClickedCommandProperty =
            DependencyProperty.Register("ItemClickedCommand", typeof(ICommand), typeof(MyListView),
            new PropertyMetadata(null, (d, e) => ((MyListView)d).ItemClickedCommand = (ICommand)e.NewValue));

        public MyListView()
        {
            this.ItemClick += MyListView_ItemClick;
        }

      
        void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ItemClickedCommand == null)
                return;

            // ok...
            var clicked = e.ClickedItem;
            if (this.ItemClickedCommand.CanExecute(clicked))
                this.ItemClickedCommand.Execute(clicked);
        }


        public ICommand ItemClickedCommand
        {
            get { return (ICommand)GetValue(ItemClickedCommandProperty); }
            set { SetValue(ItemClickedCommandProperty, value); }
        }
    }
}
