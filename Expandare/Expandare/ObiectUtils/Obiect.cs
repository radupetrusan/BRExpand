using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expandare.ObiectUtils
{
    class Obiect
    {
        public Obiect()
        {
            this.LiniiPerimetru = new List<Linie>();
        }

        public List<Linie> LiniiPerimetru { get; set; }
    }
}
