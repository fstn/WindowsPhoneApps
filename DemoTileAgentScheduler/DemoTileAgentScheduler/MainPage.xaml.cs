using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;

namespace DemoTileAgentScheduler
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            //start background agent 
            PeriodicTask periodicTask = new PeriodicTask("PeriodicAgent");

            periodicTask.Description = "My live tile periodic task";
            periodicTask.ExpirationTime = System.DateTime.Now.AddDays(1);
            
            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTask.Name) != null)
            {
                ScheduledActionService.Remove("PeriodicAgent");
            }

            //not supported in current version
            //periodicTask.BeginTime = DateTime.Now.AddSeconds(10);

            //only can be called when application is running in foreground
            ScheduledActionService.Add(periodicTask);


            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
            
            //test if Tile was created
            if (TileToFind == null)
            {
                StandardTileData NewTileData = new StandardTileData
                {
                    BackgroundImage = new Uri("Red.jpg", UriKind.Relative),
                    Title = "First",
                    Count = 1,
                    BackTitle = "Second",
                    BackContent = "Second content",
                    BackBackgroundImage = new Uri("Blue.jpg", UriKind.Relative)
                };

                ShellTile.Create(new Uri("/MainPage.xaml?TileID=2", UriKind.Relative), NewTileData);
            }
            //else
            //{
            //    ShellTileSchedule schedule = new ShellTileSchedule(TileToFind);
            //    schedule.StartTime = DateTime.Now.AddSeconds(20);
            //    schedule.Interval = UpdateInterval.EveryHour;
                
            //}

           
        }
    }
}