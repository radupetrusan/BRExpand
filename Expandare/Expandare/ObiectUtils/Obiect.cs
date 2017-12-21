using Expandare.PictureBoxUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Expandare.ObiectUtils
{
    class Obiect
    {
        public Obiect()
        {
            Id = Guid.NewGuid();
            LiniiPerimetru = new List<Linie>();
            Puncte = new List<Point>();
            Varfuri = new List<Point>();

            EConvex = false;

            _obiectCalculator = new ObiectCalculator();
        }

        public Obiect(Obiect obiect)
        {
            Id = obiect.Id;
            LiniiPerimetru = obiect.LiniiPerimetru;
            Puncte = obiect.Puncte;
            Varfuri = new List<Point>();
            obiect.Varfuri.ForEach(p => Varfuri.Add(p));

            EConvex = false;
        }

        private ObiectCalculator _obiectCalculator;

        public Guid Id { get; set; }

        public Obiect ObiectInitial { get; set; }

        public bool EConvex { get; set; }

        public List<Linie> LiniiPerimetru { get; set; }

        public List<Point> Puncte { get; set; }

        public List<Point> Varfuri { get; set; }

        public void CalculeazaPuncteInterioare(Point min, Point max)
        {
            Puncte = _obiectCalculator.CalculeazaPuncteInterioare(this, min, max);
        }

        internal void Uniformizare()
        {
            Varfuri = ConvexHull.GetConvexHull(Varfuri);
        }

        internal void UndoUniformizare()
        {
            Varfuri = new List<Point>();
            ObiectInitial.Varfuri.ForEach(p => Varfuri.Add(p));
        }

        private Point[] GetPoints(int i)
        {
            var list = new List<Point>();
            Varfuri.ForEach(v => list.Add(v));
            list.RemoveAt(i);

            var points = new Point[Varfuri.Count - 1];
            var j = 0;
            list.ForEach(p => points[j++] = p);
            return points;
        }

        public bool ContinePunct(Point p)
        {
            var poly = new Point[ObiectInitial.Varfuri.Count];
            var index = 0;
            ObiectInitial.Varfuri.ForEach(punct => poly[index++] = punct);

            Point p1, p2;

            bool inside = false;

            if (poly.Length < 3)
            {
                return inside;
            }

            var oldPoint = new Point(
                poly[poly.Length - 1].X, poly[poly.Length - 1].Y);


            for (int i = 0; i < poly.Length; i++)
            {
                var newPoint = new Point(poly[i].X, poly[i].Y);


                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;

                    p2 = newPoint;
                }

                else
                {
                    p1 = newPoint;

                    p2 = oldPoint;
                }


                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long)p1.Y) * (p2.X - p1.X)
                    < (p2.Y - (long)p1.Y) * (p.X - p1.X))
                {
                    inside = !inside;
                }


                oldPoint = newPoint;
            }


            return inside;
        }
    }
}
