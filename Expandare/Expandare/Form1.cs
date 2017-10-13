using Expandare.ObiectUtils;
using Expandare.PictureBoxUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expandare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            _inProgres = false;
            _obiectInProgres = null;
            _linieInProgres = null;

            _isNearInitialPoint = false;

            _drawUtils = new DrawUtils();

            InitializeComponent();
        }

        private bool _inProgres;
        private Obiect _obiectInProgres;
        private Linie _linieInProgres;
        private Point _initialPoint;
        private bool _isNearInitialPoint;

        private DrawUtils _drawUtils;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!_inProgres)
            {
                _obiectInProgres = new Obiect();
                _linieInProgres = new Linie();
                _linieInProgres.A = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);

                _initialPoint = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);

                _inProgres = true;
            }
            else
            {
                if (!_isNearInitialPoint)
                {
                    _linieInProgres.B = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
                    _obiectInProgres.LiniiPerimetru.Add(new Linie(_linieInProgres));

                    _drawUtils.DeseneazaLinie(_linieInProgres, this.pictureBox1);

                    _linieInProgres = new Linie();
                    _linieInProgres.A = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
                }
                else
                {
                    _linieInProgres.B = new Point(_initialPoint.X, _initialPoint.Y);
                    _obiectInProgres.LiniiPerimetru.Add(new Linie(_linieInProgres));

                    _drawUtils.DeseneazaLinie(_linieInProgres, this.pictureBox1);

                    _linieInProgres = null;
                    _obiectInProgres = null;
                    _inProgres = false;
                    _isNearInitialPoint = false;
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.InitialImage = null;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_inProgres && _initialPoint != null)
            {
                if (e.X < _initialPoint.X + 8 && e.X > _initialPoint.X - 8
                    && e.Y < _initialPoint.Y + 8 && e.Y > _initialPoint.Y - 8)
                {
                    _isNearInitialPoint = true;
                    Cursor.Current = Cursors.Hand;
                }
                else
                {
                    _isNearInitialPoint = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
    }
}
