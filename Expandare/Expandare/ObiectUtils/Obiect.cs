using System.Collections.Generic;
using System.Drawing;

namespace Expandare.ObiectUtils
{
    class Obiect
    {
        public Obiect()
        {
            LiniiPerimetru = new List<Linie>();
            Puncte = new List<Point>();
            Varfuri = new List<Point>();

            _obiectCalculator = new ObiectCalculator();
        }

        private ObiectCalculator _obiectCalculator;

        public List<Linie> LiniiPerimetru { get; set; }

        public List<Point> Puncte { get; set; }

        public List<Point> Varfuri { get; set; }

        public void CalculeazaPuncteInterioare(Point min, Point max)
        {
            Puncte = _obiectCalculator.CalculeazaPuncteInterioare(this, min, max);
        }
    }
}
