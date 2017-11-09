using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expandare.ObiectUtils
{
    class Linie
    {
        public Linie(Linie l)
        {
            Start = new Point(l.Start.X, l.Start.Y);
            End = new Point(l.End.X, l.End.Y);
        }

        public Linie(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Linie()
        {

        }

        public float M { get; set; }
        
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }

        public List<Point> Puncte { get; set; }

        public bool IsVertical { get; private set; }

        public Point Start { get; set; }

        private Point _end;
        public Point End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                if (Start != null)
                {
                    //DeterminaEcuatiaLiniei();
                    Puncte = getPoints(1000);
                }
            }
        }

        private void DeterminaEcuatiaLiniei()
        {
            M = (End.Y - Start.Y) / (End.X - Start.X);
            A = -M;
            B = 1;
            C = Start.Y - M * Start.X;

            IsVertical = Math.Abs(End.X - Start.X) < 0.00001f;
        }

        public bool IntersecteazaSegment(Linie linie)
        {
            if ((Start.Y < linie.Start.Y && Start.Y < linie.End.Y) || (Start.Y > linie.Start.Y && Start.Y > linie.End.Y))
                return false;

            return true;
        }

        public bool IntersecteazaLinie(Linie otherLine, out Point intersectionPoint)
        {
            intersectionPoint = new Point(0, 0);
            if (IsVertical && otherLine.IsVertical)
                return false;
            //if (IsVertical || otherLine.IsVertical)
            //{
            //    intersectionPoint = GetIntersectionPointIfOneIsVertical(otherLine, this);
            //    return true;
            //}
            double delta = A * otherLine.B - otherLine.A * B;
            bool hasIntersection = Math.Abs(delta - 0) > 0.0001f;
            if (hasIntersection)
            {
                double x = (otherLine.B * C - B * otherLine.C) / delta;
                double y = (A * otherLine.C - otherLine.A * C) / delta;
                intersectionPoint = new Point((int)x, (int)y);
            }
            return hasIntersection;
        }

        public List<Point> getPoints(int quantity)
        {
            var points = new Point[quantity];
            var list = new List<Point>();
            int ydiff = End.Y - Start.Y, xdiff = End.X - Start.X;
            double slope = (double)(End.Y - Start.Y) / (End.X - Start.X);
            double x, y;

            --quantity;

            for (double i = 0; i < quantity; i++)
            {
                y = slope == 0 ? 0 : ydiff * (i / quantity);
                x = slope == 0 ? xdiff * (i / quantity) : y / slope;
                var point = new Point((int)Math.Round(x) + Start.X, (int)Math.Round(y) + Start.Y);
                points[(int)i] = point;
                list.Add(point);
            }

            points[quantity] = End;
            list.Add(End);
            list.Add(Start);
            return list ;
        }
    }
}
