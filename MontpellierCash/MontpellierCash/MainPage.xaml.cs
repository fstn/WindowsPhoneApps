using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using FstnUserControl.Camera;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MontpellierCash.Resources;
using Windows.Phone.Media.Capture;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using FstnUserControl.Error;
using FstnUserControl;
using Microsoft.Expression.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Maps.Controls;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Device.Location;
using System.Globalization;

namespace MontpellierCash
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MapLayer MyLayer;
        private MapLayer PtLayer;
        private MapOverlay MyPin;
        private GeoCoordinateWatcher PosWatcher;
        private GeoCoordinate MontpellierGeo;
        // Constructor
        public MainPage()
        {
            InitAppBar();
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "176fd150-92cf-4ce3-8a9c-afcae2e4198c";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "KnZJJPGazfy6cewBpb-gJg";
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MontpellierGeo = new GeoCoordinate(43.5822454960498, 3.92993819945295);
            MyMap.Center = new GeoCoordinate(43.5822454960498, 3.92993819945295);
            MyMap.ZoomLevel = 12;
            MyLayer = new MapLayer();
            PtLayer = new MapLayer();
            MyMap.Layers.Add(MyLayer);
            MyMap.Layers.Add(PtLayer);
            Dispatcher.BeginInvoke(() =>
            {
                ReadFeux();
            });

            MyPin = new MapOverlay();
            MyLayer.Add(MyPin);
            Grid MyContent = new Grid();

            BitmapImage meBitmap = new BitmapImage(new Uri("Assets/Images/me.png", UriKind.Relative));
            MyContent.Children.Add(new Image() { Source = meBitmap });
            MyPin.Content = MyContent;
             PosWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            PosWatcher.MovementThreshold = 20; // 20 meters
            PosWatcher.StatusChanged +=
            new EventHandler<GeoPositionStatusChangedEventArgs>(OnStatusChanged);
            PosWatcher.PositionChanged +=
                new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(OnPositionChanged);
            PosWatcher.Start();
 
        }

        private void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            MyPin.GeoCoordinate=new GeoCoordinate( e.Position.Location.Latitude, e.Position.Location.Longitude);
        }

        private void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            MyPin.GeoCoordinate = new GeoCoordinate(PosWatcher.Position.Location.Latitude, PosWatcher.Position.Location.Longitude);
            MyMap.Center=new GeoCoordinate(PosWatcher.Position.Location.Latitude, PosWatcher.Position.Location.Longitude);
            double distance=MyPin.GeoCoordinate.GetDistanceTo(MontpellierGeo);
            int zoom=1;
            if( distance<1000)
                zoom=19;            
            else if( distance<10000)
                zoom=14;     
            else if( distance<50000)
                zoom = 10;
            else if (distance < 150000)
                zoom = 7;
            else if (distance < 300000)
                zoom = 5;

            MyMap.ZoomLevel=zoom;
        }

        private void ReadFeux()
        {
            Stream testpath = Application.GetResourceStream(new Uri("Data/CashMachines.csv", UriKind.Relative)).Stream;
            StreamReader reader = new StreamReader(testpath);
            string line;
            BitmapImage euroBitmap = new BitmapImage(new Uri("Assets/Images/euro.png", UriKind.Relative));
            while ((line = reader.ReadLine()) != null)
            {
                string pattern = "[\"]([0-9]+),([0-9]+)[\"]";
                string result = Regex.Replace(line, pattern, "$1.$2");
                string[] elements = result.Split(',');
                MapOverlay PtOverlay = new MapOverlay();
                try
                {
                    PtOverlay.GeoCoordinate = new GeoCoordinate(double.Parse(elements[5], CultureInfo.InvariantCulture),
                                                                double.Parse(elements[4], CultureInfo.InvariantCulture));
                    PtOverlay.PositionOrigin = new Point(0, 0.5);
                    PtLayer.Add(PtOverlay);

                    Grid MyGrid = new Grid();
                    PtOverlay.Content = MyGrid;
                    MyGrid.RowDefinitions.Add(new RowDefinition());
                    MyGrid.RowDefinitions.Add(new RowDefinition());
                    MyGrid.Background = new SolidColorBrush(Colors.Transparent);

                    Image euro = new Image();
                    euro.Source = euroBitmap;
                    euro.Tap += (o, e) =>
                    {
                        var bankName = elements[2].Replace("  ", "");
                        if(bankName!="")
                            MessageBox.Show(bankName);
                    };

                    //Adding the Polygon to the Grid
                    MyGrid.Children.Add(euro);
                    System.Diagnostics.Debug.WriteLine("cash=" + elements[2].Replace("  ", "") + "," + elements[4].Replace("  ", "") + "," + elements[5].Replace("  ", ""));
                }
                catch (Exception e)
                {
                }
                MyMap.Visibility = Visibility.Visible;
                /* var values = lines.Select(l => new { FirstColumn = l.Split(',').First(), Values = l.Split(',').Skip(1).Select(v => int.Parse(v)) });
             foreach (var value in values)
             {
                 Console.WriteLine(string.Format("Column '{0}', Sum: {1}, Average {2}", value.FirstColumn, value.Values.Sum(), value.Values.Average()));
             }*/
            }
        }


        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.share.png", Msg.Save, AskToShare);
        }
        private void AskToShare(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "Va retirer de l'argent!";
            shareStatusTask.Show();
        }

    }
}