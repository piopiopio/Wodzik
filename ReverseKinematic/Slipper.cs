using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace ReverseKinematic
{
    public class Slipper : ViewModelBase
    {

        public bool CentralMRS { get; set; } = true;
        private const int WidthHeight = 1000;
        public double pulleyY
        {
            get { return (WidthHeight/2 - R); }
        }

        public double pulleyX
        {
            get { return (WidthHeight/2 - R); }
        }
        private double r = 100;

        public double D
        {
            get { return 2 * R; }
        }
        public double R
        {
            get { return r; }
            set
            {
                if (value < l)///2)
                {
                    r = value;
                }

                Refresh();
            }
        }

        private double _omega = 1;
        public double Omega
        {
            get { return _omega; }
            set
            {
                _omega = value;
                OnPropertyChanged(nameof(_omega));
            }
        }

        private double _epsilon = 0;
        public double Epsilon
        {
            get { return _epsilon; }
            set
            {
                _epsilon = value;
                OnPropertyChanged(nameof(Epsilon));
            }
        }

        private double l = 200;
        public double L
        {
            get
            {
                return l;
            }
            set
            {
                if (value > r)//*2)
                {
                    l = value;
                }


                Refresh();
            }
        }


        private double _pivotX1 = 100;
        public double PivotX1
        {
            get
            {
                return _pivotX1+ WidthHeight/2;
            }
            set
            {
                _pivotX1 = value;
               Refresh();
            }
        }

        public double LineX2
        {
            get
            {
                return -_pivotX1+ WidthHeight/2;
            }
            set
            {
                _pivotX1 = -value;
               Refresh();
            }
        }

        private double _pivotY1 = 0;
        public double PivotY1
        {
            get
            {
                return -_pivotY1 + WidthHeight/2;
            }
            set
            {
                _pivotY1 = value;
            Refresh();
            }
        }

    
        public double LineY2
        {
            get
            {
                return _pivotY1+ WidthHeight/2;
            }
            set
            {
                _pivotY1 = -value;
              Refresh();
            }
        }


        private double _x = 300;
        public double X
        {
            get
            {
                return _x + WidthHeight / 2;
            }
            set
            {
                _x = value;
              Refresh();
            }
        }

        private double y = 0;
        public double Y
        {
            get
            {
                return y + WidthHeight / 2;
            }
            set
            {
                y = value;
               Refresh();
            }
        }

        public double PistonY
        {
            get
            {
                return y + WidthHeight / 2-15;
            }
            set
            {
                y = value;
                Refresh();
            }
        }
        private List<DataPoint> _position = new List<DataPoint>();
        public List<DataPoint> Position
        {
            get { return _position; }
            set
            {
                _position = value;
                Refresh();
            }
        }

        private List<DataPoint> _velocity = new List<DataPoint>();
        public List<DataPoint> Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity = value;
                Refresh();
            }
        }
        private List<DataPoint> _acceleration = new List<DataPoint>();
        public List<DataPoint> Acceleration
        {
            get { return _acceleration; }
            set
            {
                _acceleration = value;
                Refresh();
            }
        }

        private List<DataPoint> _phase = new List<DataPoint>();
        public List<DataPoint> Phase
        {
            get { return _phase; }
            set
            {
                _phase = value;
                Refresh();
            }
        }


        private double _alpha = 0;

        public void Refresh()
        {
            Kinematics(_alpha, L);

            RefreshProperties();

        }

        public void RefreshProperties()
        {
            OnPropertyChanged(nameof(PistonY));
            OnPropertyChanged(nameof(Y));
            OnPropertyChanged(nameof(L));
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(LineX2));
            OnPropertyChanged(nameof(LineY2));
            OnPropertyChanged(nameof(PivotY1));
            OnPropertyChanged(nameof(PivotX1));
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(pulleyX));
            OnPropertyChanged(nameof(pulleyY));
            OnPropertyChanged(nameof(D));
          //  OnPropertyChanged(nameof(Position));
           // OnPropertyChanged(nameof(Velocity));
           // OnPropertyChanged(nameof(Acceleration));
        }

        public int SampleQuantity { get; set; } =400;
        Random rand = new Random(); //reuse this if you are generating many

        public void RenderFrame(double time)
        {

            double mean = 0;

            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double L_withError =L+mean + Epsilon * randStdNormal; //random normal(mean,stdDev^2)





            Kinematics(Omega*time/1000, L_withError);


            while (_position.Count> SampleQuantity+5)
            {
                _position.RemoveAt(0);
                _velocity.RemoveAt(0);
                
                _acceleration.RemoveAt(0);
                _phase.RemoveAt(0);

            }

            if (CentralMRS)
            {
                _position.Add(new DataPoint(time / 1000, _x- l));
                if (_position.Count > 3)
                {
                    _velocity.Add(new DataPoint(_position[_position.Count - 2].X,
                        (_position[_position.Count - 1].Y - _position[_position.Count - 3].Y) / (_position[_position.Count - 1].X - _position[_position.Count - 3].X)));


                    _phase.Add(new DataPoint(_position.Last().Y, _velocity.Last().Y));
                }


                if (_position.Count >5)
                {
                    _acceleration.Add(new DataPoint(_position[_position.Count - 2].X,
                        
                        (_velocity[_velocity.Count - 1].Y - _velocity[_velocity.Count - 3].Y) /
                        (_velocity[_velocity.Count - 1].X - _velocity[_velocity.Count - 3].X)));

                    //_acceleration.Add(new DataPoint(_position[_position.Count - 1].X, (_position[_position.Count - 1].X-2* _position[_position.Count - 2].X+ _position[_position.Count - 3].X)/((_position[_position.Count - 1].X- _position[_position.Count - 3].X)* (_position[_position.Count - 1].X - _position[_position.Count - 3].X))));
                }

            }
            else
            {
                _position.Add(new DataPoint(time / 1000, _x-l));
                if (_position.Count > 2)
                {
                    _velocity.Add(new DataPoint(_position[_position.Count - 1].X,
                        (_position[_position.Count - 1].Y - _position[_position.Count - 2].Y) / (_position[_position.Count - 1].X - _position[_position.Count - 2].X)));


                    _phase.Add(new DataPoint(_position.Last().Y, _velocity.Last().Y));
                }


                if (_position.Count > 3)
                {
                    _acceleration.Add(new DataPoint(_position[_position.Count - 1].X,
                        (_velocity[_velocity.Count - 1].Y - _velocity[_velocity.Count - 2].Y) /
                         (_velocity[_velocity.Count - 1].X - _velocity[_velocity.Count - 2].X)));

                    //_acceleration.Add(new DataPoint(_position[_position.Count - 1].X, (_position[_position.Count - 1].X-2* _position[_position.Count - 2].X+ _position[_position.Count - 3].X)/((_position[_position.Count - 1].X- _position[_position.Count - 3].X)* (_position[_position.Count - 1].X - _position[_position.Count - 3].X))));
                }
            }
          



            RefreshProperties();

        }
        public void Kinematics(double alpha, double LwE)
        {
            _pivotX1 = R*Math.Cos(alpha);
            _pivotY1 = R*Math.Sin(alpha);

            _x = R * Math.Cos(alpha) + Math.Sqrt(LwE * LwE - R * R * Math.Sin(alpha) * Math.Sin(alpha));


        }

        public Slipper()
        {
            Kinematics(Math.PI / 4, L);
        }


    }
}
