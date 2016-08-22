﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuicKE.Client.UI
{
    internal static class FrameworkElementExtender
    {
        internal static Page GetParentPage(this FrameworkElement element)
        {
            DependencyObject walk = element;
            while (walk != null)
            {
                if (walk is Page)
                    return (Page)walk;

                if (walk is FrameworkElement)
                    walk = ((FrameworkElement)walk).Parent;
                else
                    break;
            }

            // nothing...
            return null;
        }

        internal static void OpenAppBarsOnPage(this FrameworkElement element, bool sticky)
        {
            // get...
            var page = element.GetParentPage();
            if (page == null)
                return;

            if (page.TopAppBar != null)
            {
                page.TopAppBar.IsSticky = sticky;
                page.TopAppBar.IsOpen = true;
            }
            if (page.BottomAppBar != null)
            {
                page.BottomAppBar.IsSticky = sticky;
                page.BottomAppBar.IsOpen = true;
            }
        }

        internal static void HideAppBarsOnPage(this FrameworkElement element)
        {
            var page = element.GetParentPage();
            if (page == null)
                return;

            if (page.TopAppBar != null)
            {
                page.TopAppBar.IsOpen = false;
                page.TopAppBar.IsSticky = false;
            }
            if (page.BottomAppBar != null)
            {
                page.BottomAppBar.IsOpen = false;
                page.BottomAppBar.IsSticky = false;
            }
        }
    }
}
