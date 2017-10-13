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
        public void DeseneazaLinie(Linie linie, PictureBox pictureBox)
        {
            Graphics g = pictureBox.CreateGraphics();
            PointF pt1D = new PointF();
            PointF pt2D = new PointF();
            pt1D.X = linie.A.X;
            pt1D.Y = linie.A.Y;
            pt2D.X = linie.B.X;
            pt2D.Y = linie.B.Y;

            g.DrawLine(Pens.Black, pt1D, pt2D);
        }
    }
}
