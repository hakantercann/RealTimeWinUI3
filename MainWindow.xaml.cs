using App3.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static bool MainPageIsActive = false;
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(MainPage));
        }

        private void nvSample8_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                // Handle the settings page navigation here
               // ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = (args.SelectedItemContainer as NavigationViewItem)?.Tag?.ToString();

                // Navigate to the appropriate page based on the Tag
                switch (navItemTag)
                {
                    case "MainPage":
                        ContentFrame.Navigate(typeof(MainPage));
                        break;

                    case "AnotherPage":
                        ContentFrame.Navigate(typeof(BlankPage1));
                        break;

                    default:
                        // Handle any unknown navigation items or show an error message.
                        break;
                }
            }
        }
    }
}
