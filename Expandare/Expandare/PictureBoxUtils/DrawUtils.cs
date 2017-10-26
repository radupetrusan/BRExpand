using Expandare.ObiectUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void ColoreazaInteriorObiect(Obiect obiect, Pen pen)
        {
            obiect.Puncte.ForEach(p => DeseneazaPunct(p, pen));
        }
    }
}
