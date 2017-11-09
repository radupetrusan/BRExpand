using Expandare.ObiectUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expandare.PictureBoxUtils
{
    class DrawUtils
    {
        private PictureBox _pictureBox;
        private Graphics _graphics;

        public DrawUtils(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;
            _graphics = _pictureBox.CreateGraphics();
        }

        public void DeseneazaLinie(Linie linie)
        {
            PointF pt1D = new PointF();
            PointF pt2D = new PointF();
            pt1D.X = linie.Start.X;
            pt1D.Y = linie.Start.Y;
            pt2D.X = linie.End.X;
            pt2D.Y = linie.End.Y;

            _graphics.DrawLine(Pens.Black, pt1D, pt2D);
        }

        public void DeseneazaPunct(Point punct, Pen pen)
        {
            _graphics.DrawRectangle(pen, new Rectangle(punct, new Size(1, 1)));
        }

        public void DeseneazaVarf(Point punct, int size)
        {
            _graphics.DrawEllipse(Pens.Black, new Rectangle(new Point(punct.X - size, punct.Y - size), new Size(size * 2 + 1, size * 2 + 1)));
        }

        public void ColoreazaInteriorObiect(Obiect obiect, Color color, Brush brush)
        {
            var myPath = new GraphicsPath(FillMode.Winding);
            Point[] points = new Point[obiect.Varfuri.Count];
            var i = 0;
            obiect.Varfuri.ForEach(p => points[i++] = p);
            myPath.AddPolygon(points);

            var myPen = new Pen(color, 2);

            //obiect.Puncte.ForEach(p => DeseneazaPunct(p, pen));

            _graphics.DrawPath(myPen, myPath);
            _graphics.FillPath(brush, myPath);
        }

        public void ColoreazaPath(GraphicsPath path)
        {
            _graphics.DrawPath(Pens.Yellow, path);
            _graphics.FillPath(Brushes.Yellow, path);
        }

        public void ExpandareUniforma(Obiect obiect, decimal size)
        {
            var myPath = new GraphicsPath(FillMode.Winding);
            Point[] points = new Point[obiect.Varfuri.Count];
            var i = 0;
            obiect.Varfuri.ForEach(p => points[i++] = p);
            myPath.AddPolygon(points);

            var originalPath = new GraphicsPath(FillMode.Winding);
            Point[] originalPoints = new Point[obiect.ObiectInitial.Varfuri.Count];
            var ii = 0;
            obiect.ObiectInitial.Varfuri.ForEach(p => originalPoints[ii++] = p);
            originalPath.AddPolygon(originalPoints);

            var myPen = new Pen(Color.Green, (int)size);
            
            //obiect.Puncte.ForEach(p => DeseneazaPunct(p, pen));

            _graphics.DrawPath(myPen, myPath);
            _graphics.FillPath(Brushes.Yellow, myPath);
            _graphics.FillPath(Brushes.Red, originalPath);
        }

        public void ExpandareNeuniforma(Obiect obiect, decimal sizeSus, decimal sizeDreapta, decimal sizeStanga, decimal sizeJos)
        {
            var xMove = (sizeDreapta + sizeStanga) / 2 - sizeDreapta;
            var yMove = (sizeSus + sizeJos) / 2 - sizeSus;

            var expandPath = new GraphicsPath(FillMode.Winding);
            var yellowPath = new GraphicsPath(FillMode.Winding);
            var originalPath = new GraphicsPath(FillMode.Winding);

            Point[] expandPoints = new Point[obiect.Varfuri.Count];
            Point[] yellowPoints = new Point[obiect.Varfuri.Count];
            Point[] originalPoints = new Point[obiect.ObiectInitial.Varfuri.Count];

            var i = 0;
            var ii = 0;

            obiect.Varfuri.ForEach(p => 
            {
                expandPoints[i] = new Point(p.X - (int)xMove, p.Y + (int)yMove);
                yellowPoints[i++] = p;
            });
            obiect.ObiectInitial.Varfuri.ForEach(p => originalPoints[ii++] = p);

            expandPath.AddPolygon(expandPoints);
            yellowPath.AddPolygon(yellowPoints);
            originalPath.AddPolygon(originalPoints);

            var myPen = new Pen(Color.Green, ((int)sizeDreapta + (int)sizeStanga));

            _graphics.DrawPath(myPen, expandPath);
            _graphics.FillPath(Brushes.Yellow, yellowPath);
            _graphics.FillPath(Brushes.Red, originalPath);
        }
    }
}
