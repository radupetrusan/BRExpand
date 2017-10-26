using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expandare.ObiectUtils
{
    class ObiectCalculator
    {
        public List<Point> CalculeazaPuncteInterioare(Obiect obiect, Point min, Point max)
        {
            var list = new List<Point>();
            for (var y = min.Y; y < max.Y; y++)
            {
                for (var x = min.X; x < max.X; x++)
                {
                    Point p = new Point(x, y);
                    //daca punctul e in interior, adauga
                    if (PunctInInteriorulObiectului(p, obiect, max.X))
                    {
                        list.Add(p);
                    }
                }
            }
            return list;
        }

        private bool PunctInInteriorulObiectului(Point punct, Obiect obiect, int x)
        {
            var linie = new Linie(punct, new Point(x, punct.Y));
            var count = 0;

            foreach (var linieObiect in obiect.LiniiPerimetru)
            {
                if (linie.IntersecteazaSegment(linieObiect))
                {
                    count++;
                }
            }

            if (count % 2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
