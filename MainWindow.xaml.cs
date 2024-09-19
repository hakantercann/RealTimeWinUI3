using App3.Classes;
using App3.Pages;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Preview;
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
        Plc _client;
        Thread mainThread;
        Thread plcThread;
        bool flashor = false;
        int ping_ok = 2;
        public static bool MainPageIsActive = false;
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(MainPage));
            _client = new Plc(CpuType.S71200, "127.0.0.1", 0, 1);
            mainThread = new Thread(Main_Loop);
            plcThread = new Thread(Loop1);

           mainThread.Start();
            plcThread.Start();
        }
        public Stopwatch sw1 = new Stopwatch();
        public long maksimuma1 = long.MinValue, minimuma1 = long.MaxValue;
        private void Loop1(object obj)
        {
            Random random = new Random();
            while (true)
            {
                long deger = 0;
                deger = sw1.ElapsedMilliseconds ;
                
                Thread.Sleep(200);
                if (MainPageIsActive)
                {
                    DispatcherQueue.TryEnqueue(() => {
                        var mainPage = ContentFrame.Content as MainPage;
                        var degSecond = (double)deger / 1000;
                        mainPage.cycleTime.Text = degSecond.ToString("0.000");
                        if (random.Next(0, 5) == 2)
                        {
                            //mainPage.alarmList.Add(new Alarm
                            //{
                            //    Name = "Alarm",
                            //    Description = "Test"
                            //});
                        }
                        sw1.Reset();
                        sw1.Start();
                    });

                    // var mainPage = ContentFrame.Content as MainPage;
                }
            }
        }

        private void Main_Loop(object obj)
        {
            while (true)
            {
                try
                {
                    flashor = !flashor;
                   
                  
                    if (_client.IsConnected)
                    {


                        if (flashor)
                        {
                            DispatcherQueue.TryEnqueue(() => { plcConnStatus.Background = new SolidColorBrush(Colors.Green); });
                            
                        }
                        else
                        {
                            DispatcherQueue.TryEnqueue(() => { plcConnStatus.Background = new SolidColorBrush(Colors.Lime); });
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    else
                    {
                        #region ping
                        if (ping_ok == 1)
                        {
                           // PingState.Text = "PÝNG OK";
                            try
                            {
                                _client.Open();
                            }
                            catch { }
                            ping_ok = 0;
                        }
                        else if (ping_ok == 2)
                        {
                           // PingState.Text = "PÝNG NOT OK";
                        }
                        #endregion
                        if (!_client.IsConnected)
                        {
                            Ping(_client.IP, 1, 500, 1);
                        }

                        if (flashor)
                        {
                            DispatcherQueue.TryEnqueue(() => { plcConnStatus.Background = new SolidColorBrush(Colors.Red); });
                        }
                        else
                        {
                            DispatcherQueue.TryEnqueue(() => { plcConnStatus.Background = new SolidColorBrush(Colors.Orange); });
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                   
                    //lbl_tarih.Text = DateTime.Now.ToString("d");
                    //lbl_saat.Text = DateTime.Now.ToString("T");

                    Thread.Sleep(500);
                }
                catch { }
            }
        }
        #region Functions
        public void Ping(string host, int attempts, int timeout, int plc)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        Ping ping = new Ping();
                        switch (plc)
                        {
                            case 1:
                                ping.PingCompleted += new PingCompletedEventHandler(PingCompleted1);
                                break;
                            //case 3:
                            //ping.PingCompleted += new PingCompletedEventHandler(PingCompleted3);
                            //break;
                            default:
                                break;
                        }
                        ping.SendAsync(host, timeout, host);
                    }
                    catch { }
                }).Start();
            }
        }
        private void PingCompleted1(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                ping_ok = 1;
            }
            else ping_ok = 2;
        }
        private bool[] WordParserToBoolArray(byte[] gelenler)
        {
            var list = new bool[gelenler.Length * 8];

            for (int i = 0; i < gelenler.Length; i++)
            {
                list[i * 8] = gelenler[i].SelectBit(0);
                list[i * 8 + 1] = gelenler[i].SelectBit(1);
                list[i * 8 + 2] = gelenler[i].SelectBit(2);
                list[i * 8 + 3] = gelenler[i].SelectBit(3);
                list[i * 8 + 4] = gelenler[i].SelectBit(4);
                list[i * 8 + 5] = gelenler[i].SelectBit(5);
                list[i * 8 + 6] = gelenler[i].SelectBit(6);
                list[i * 8 + 7] = gelenler[i].SelectBit(7);
            }
            return list;
        }
        #endregion

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
