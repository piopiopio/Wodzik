﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using OxyPlot;

namespace ReverseKinematic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point position = new Point();
        private Point moveVector = new Point();
        private MainViewModel _mainViewModel = new MainViewModel();
        RectangleObstacle tempRectangle = new RectangleObstacle();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = _mainViewModel;
            var line = new Line();
            line.Stroke = Brushes.Black;
            line.X1 = 0;
            line.Y1 = 0;
            line.X2 = 100;
            line.Y2 = 100;
            line.StrokeThickness = 2;
            //   this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            // MainCanvas.Children.Add(line);
            //_mainViewModel.Scene.ObstaclesCollection.Add(new RectangleObstacle(500,500,500,500));
            _mainViewModel.Scene.TurnOnAnimationModeReverseKinematic += TurnOnAnimMode;
            _mainViewModel.Scene.TurnOffAnimationModeReverseKinematic += TurnOffAnimMode;


        }


        private void TurnOffAnimMode(object sender, PropertyChangedEventArgs e)
        {
            TurnOffAnimtionMode(true);
        }


        void TurnOnAnimMode(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            TurnOffAnimtionMode(false);

            //glControl.Invalidate();
        }
        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //TODO: Zrobić automatyczne skalowanie.

            MainWindow1.Height = MainWindow1.Width * 9 / 16 + 24;
            





        }

        private void MainCanvas_OnRightMouseDown(object sender, MouseButtonEventArgs e)
        {




        }

        private void MainCanvas_OnLeftMouseUp(object sender, MouseButtonEventArgs e)
        {


        }


        private void MainCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            var currentPosition = rescalePoint(e.GetPosition(MainViewbox));

            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {


             

            }
            else
            {

                var newPosition = rescalePoint(e.GetPosition(MainViewbox));

                var Width = newPosition.X - position.X;
                var Height = newPosition.Y - position.Y;
                if (Width < 0) tempRectangle.From = new Point(newPosition.X, tempRectangle.From.Y);
                if (Height < 0) tempRectangle.From = new Point(tempRectangle.From.X, newPosition.Y);
                tempRectangle.Size = new Point(Math.Max(Math.Abs(Width), 10), Math.Max(Math.Abs(Height), 10));
            }
            moveVector = currentPosition;
        }

        public Point rescalePoint(Point p1)
        {
          
            double a = 1000;
            var b = MainViewbox.ActualWidth;
            var scale = b / a;
            return new Point(p1.X / scale, p1.Y / scale);

        }


        private void MainCanvas_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {



        }





        private void OnPrievewKeyDown(object sender, KeyEventArgs e)
        {
        }


        private void StartAnimation_OnClick(object sender, RoutedEventArgs e)
        {
          
            TurnOffAnimtionMode(false);
            _mainViewModel.Scene.StartSimulation(PositionGraph, VelocityGraph, AccelerationGraph, PhaseGraph);

        }

        private void TurnOffAnimtionMode(bool input)
        {
           
       

            StartAnimation.IsEnabled = input;
            AnimationLength.IsEnabled = input;
            StopAnimation.IsEnabled = !input;
            L.IsEnabled = input;
            R.IsEnabled = input;
            Omega.IsEnabled = input;
            Epsilon.IsEnabled = input;
        }

        private void StopAnimation_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.Scene.StopSimulation();
        }


}
}
