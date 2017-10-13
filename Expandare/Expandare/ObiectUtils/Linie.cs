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
            this.A = new Point(l.A.X, l.A.Y);
            this.B = new Point(l.B.X, l.B.Y);
        }

        public Linie()
        {

        }

        public Point A { get; set; }

        public Point B { get; set; }
    }
}
