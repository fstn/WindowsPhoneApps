using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BrokeThePig.UC.Weapons;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BrokeThePig.UC
{
    public delegate void UpdatedHandleEvent(PointNumber number);
    public delegate void ExplodedHandleEvent();
    public partial class Pig : UserControl
    {
        public event UpdatedHandleEvent Updated;
        public event ExplodedHandleEvent Exploded;
        private List<GraphicComponent> ListOfParts;
        public BrokeThePigWeapon Arme { get; set; }
        public PointNumber CurrentNumber
        {
            get;
            set;
        }


        public Pig()
        {
            this.DataContext = this;
            Arme = new SimpleWeapon();
            CurrentNumber = new PointNumber(1000);
            InitializeComponent();
            this.Loaded += Pig_Loaded;
            CrackImg.Tap += CrackImg_Tap;

        }

        void CrackImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            gc_OnTap(new PointNumber(0));
        }

        void Pig_Loaded(object sender, RoutedEventArgs e)
        {

            ListOfParts = (from c in Body_before.Children
                           where c.GetType() == typeof(GraphicComponent)
                           select c).Cast<GraphicComponent>().ToList();
            foreach (GraphicComponent gc in ListOfParts)
            {
                gc.OnTap += gc_OnTap;
            }
        }

        void gc_OnTap(PointNumber newNumber)
        {
            if (Updated != null)
            {
                Updated(newNumber);
            }
            Debugger.Log(0, "", "tap");
            foreach (GraphicComponent gc in ListOfParts)
            {
                gc.Update(CurrentNumber.Number);
            }
            if (newNumber.Number < 0)
            {
                foreach (GraphicComponent gc in ListOfParts)
                {
                    gc.OnTap -= gc_OnTap;
                }
                Storyboard storyboard = new Storyboard();
                DoubleAnimation da = new DoubleAnimation();
                da.From = CrackMask.RadiusX;
                da.To = 600;
                storyboard.Children.Add(da);
                Storyboard.SetTarget(da, CrackMask);
                Storyboard.SetTargetProperty(da, new PropertyPath(EllipseGeometry.RadiusXProperty));
                storyboard.Completed += storyboard_Completed;
                storyboard.Begin();
                storyboard.SpeedRatio = 0.5;
            }
               
        }

        void storyboard_Completed(object sender, EventArgs e)
        {
            Body_broke.Clip = null;
            Body_broke.Visibility = Visibility.Visible;
            CrackImg.Visibility = Visibility.Collapsed;
            Body_before.Visibility = Visibility.Collapsed;
            explode.Completed += explode_Completed;
            explode.Begin();
        }

        void explode_Completed(object sender, EventArgs e)
        {
            Body_broke.Visibility = Visibility.Collapsed;
            SoundController sc = new SoundController();
            sc.PlaySound("Sounds/win.wav");
            if (Exploded != null)
            {
                Exploded();
            }
        }
    }
}
