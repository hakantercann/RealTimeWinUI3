using App3.Classes;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            List<Alarm> alarmList = new List<Alarm>();
            alarmList.Add(new Alarm
            {
                Name = "Alarm1",
                Description = "ST20 xxxxxxxxxxxx",
            });
            alarmList.Add(new Alarm
            {
                Name = "Alarm2",
                Description = "ST20 yyyyyyyyyyyyy",
            }); alarmList.Add(new Alarm
            {
                Name = "Alarm3",
                Description = "ST20 zzzzzzzzzzzzz",
            }); alarmList.Add(new Alarm
            {
                Name = "Alarm4",
                Description = "ST20 kkkkkkkkkkkkkk",
            });
            AlarmListView.ItemsSource = alarmList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.MainPageIsActive = true;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.MainPageIsActive = false;
        }
    }
}
