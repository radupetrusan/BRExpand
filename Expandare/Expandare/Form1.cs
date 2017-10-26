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
            InitializeComponent();

            _inProgres = false;
            _obiectInProgres = null;
            _linieInProgres = null;

            _isNearInitialPoint = false;

            _objects = new List<Obiect>();

            _drawUtils = new DrawUtils(pictureBox1);
        }

        private bool _inProgres;
        private Obiect _obiectInProgres;
        private Linie _linieInProgres;
        private Point _initialPoint;
        private bool _isNearInitialPoint;
        private List<Obiect> _objects;

        private DrawUtils _drawUtils;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var punct = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);

            if (!_inProgres)
            {
                _obiectInProgres = new Obiect();
                _linieInProgres = new Linie();
                _linieInProgres.Start = punct;

                _initialPoint = punct;

                if (checkBox1.Checked)
                {
                    _drawUtils.DeseneazaVarf(punct, (int)numericUpDown1.Value);
                }

                _inProgres = true;
            }
            else
            {
                if (!_isNearInitialPoint)
                {
                    _linieInProgres.End = punct;
                    _obiectInProgres.LiniiPerimetru.Add(new Linie(_linieInProgres));

                    _drawUtils.DeseneazaLinie(_linieInProgres);
                    if (checkBox1.Checked)
                    {
                        _drawUtils.DeseneazaVarf(punct, (int)numericUpDown1.Value);
                    }

                    _linieInProgres = new Linie();
                    _linieInProgres.Start = punct;
                }
                else
                {
                    _linieInProgres.End = new Point(_initialPoint.X, _initialPoint.Y);
                    _obiectInProgres.LiniiPerimetru.Add(new Linie(_linieInProgres));

                    _obiectInProgres.CalculeazaPuncteInterioare(new Point(0, 0), new Point(pictureBox1.Width, pictureBox1.Height));
                    _objects.Add(_obiectInProgres);

                    _drawUtils.ColoreazaInteriorObiect(_obiectInProgres, Pens.Black);
                    _drawUtils.DeseneazaLinie(_linieInProgres);

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Checked;
        }
    }
}
