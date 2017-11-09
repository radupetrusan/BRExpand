using Expandare.PictureBoxUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Expandare.ObiectUtils
{
    class Obiect
    {
        public Obiect()
        {
            LiniiPerimetru = new List<Linie>();
            Puncte = new List<Point>();
            Varfuri = new List<Point>();

            EConvex = false;

            _obiectCalculator = new ObiectCalculator();
        }

        public Obiect(Obiect obiect)
        {
            LiniiPerimetru = obiect.LiniiPerimetru;
            Puncte = obiect.Puncte;
            Varfuri = new List<Point>();
            obiect.Varfuri.ForEach(p => Varfuri.Add(p));

            EConvex = false;
        }

        private ObiectCalculator _obiectCalculator;

        public Obiect ObiectInitial { get; set; }

        public bool EConvex { get; set; }

        public List<Linie> LiniiPerimetru { get; set; }

        public List<Point> Puncte { get; set; }

        public List<Point> Varfuri { get; set; }

        public void CalculeazaPuncteInterioare(Point min, Point max)
        {
            Puncte = _obiectCalculator.CalculeazaPuncteInterioare(this, min, max);
        }

        internal void Uniformizare(DrawUtils drawUtils)
        {
            //if (Varfuri.Count > 3)
            //{
            //    for (var i = 0; i < Varfuri.Count; i++)
            //    {
            //        var puncte = GetPoints(i);
            //        var myPath = new GraphicsPath(FillMode.Winding);

            //        myPath.AddPolygon(puncte);
            //        drawUtils.ColoreazaPath(myPath);
            //    }
            //}





            //var i = 0;

            //while (i < Varfuri.Count)
            //{
            //    var firstPoint = Varfuri[i];
            //    var secondPoint = Varfuri[(i + 1) % Varfuri.Count];
            //    var thirdPoint = Varfuri[(i + 2) % Varfuri.Count];

            //    var line = new Line();

            //    i++;
            //}

            Varfuri = ConvexHull.GetConvexHull(Varfuri);



            //var i = 0;

            //while (i < LiniiPerimetru.Count)
            //{
            //    var firstLine = LiniiPerimetru[i];
            //    var secondLine = LiniiPerimetru[(i + 1) % LiniiPerimetru.Count];

            //    foreach (var line in LiniiPerimetru)
            //    {
            //        if (line != firstLine && line != secondLine)
            //        {
            //            var point = new Point(0, 0);
            //            if (firstLine.IntersecteazaLinie(line, out point))
            //            {
            //                Varfuri.Remove(firstLine.End);
            //            }
            //        }
            //    }

            //    i++;
            //}

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
    }
}
