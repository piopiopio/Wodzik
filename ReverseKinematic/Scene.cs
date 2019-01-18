using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xaml;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace ReverseKinematic
{
    public class Scene : ViewModelBase
    {
        public bool timeCorrectionOn = true;
        private Slipper _slipper = new Slipper();

        public Slipper Slipper1
        {
            get { return _slipper; }
            set
            {
                _slipper = value;
                OnPropertyChanged(nameof(Slipper1));
            }
        }

        private double _simulationTime = 5;
        public double SimulationTime
        {
            get { return _simulationTime; }
            set
            {
                _simulationTime = value;
                OnPropertyChanged();
            }
        }



        public ObservableCollection<RectangleObstacle> ObstaclesCollection { get; private set; }



        public Scene()
        {

        }

        private double time = 0;
        private DispatcherTimer timer;
        //private DateTimeOffset StartTime;
        private Stopwatch stopWatch;
        private OxyPlot.Wpf.Plot _positionPlot, _velocityPlot, _accelerationPlot, _phasePlot;
        public void StartSimulation(OxyPlot.Wpf.Plot positionPlot, OxyPlot.Wpf.Plot velocityPlot, OxyPlot.Wpf.Plot accelerationPlot, OxyPlot.Wpf.Plot phasePlot)
        {
            time = 0;
            Slipper1.Position.Clear();
            Slipper1.Velocity.Clear();
            Slipper1.Acceleration.Clear();
            Slipper1.Phase.Clear();
            _positionPlot = positionPlot;
            _velocityPlot = velocityPlot;
            _accelerationPlot = accelerationPlot;
            _phasePlot = phasePlot;

            //StartTime = DateTimeOffset.Now;
            stopWatch = new Stopwatch();
            stopWatch.Start();
            lastTicks = stopWatch.ElapsedTicks;
            TurnOnAnimation();

            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Tick += TimerOnTick;
            timer.Interval = TimeSpan.FromMilliseconds(10);

            timer.Start();

        }

        private List<Point[]> PointsList;
        int i =0;
        private long lastTicks;

        private void TimerOnTick(object sender, EventArgs e)
        {


            i++;
            // Slipper1.RenderFrame(stopWatch.ElapsedMilliseconds);
            // Slipper1.RenderFrame((double)stopWatch.ElapsedTicks/10000);

            if (timeCorrectionOn)
            {
                Slipper1.RenderFrame(time);
            }
            else
            {
                //   Slipper1.RenderFrame((double)stopWatch.ElapsedMilliseconds);
                //   Slipper1.RenderFrame((double)stopWatch.ElapsedTicks / 10000);
                Slipper1.RenderFrame((double) stopWatch.ElapsedTicks / (Stopwatch.Frequency / 1000));
            }


            _positionPlot.InvalidatePlot(true);
            _velocityPlot.InvalidatePlot(true);
            _accelerationPlot.InvalidatePlot(true);
            _phasePlot.InvalidatePlot(true);


            time += 20;


               

            
        }

        public void StopSimulation()
        {
            timer.Stop();
            TurnOffAnimation();
        }


    }
}
