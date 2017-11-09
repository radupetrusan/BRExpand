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
                _obiectInProgres.Varfuri.Add(punct);
            }
            else
            {
                if (!_isNearInitialPoint)
                {
                    _obiectInProgres.Varfuri.Add(punct);
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

                    //_obiectInProgres.CalculeazaPuncteInterioare(new Point(0, 0), new Point(pictureBox1.Width, pictureBox1.Height));
                    //_objects.Add(_obiectInProgres);

                    _drawUtils.DeseneazaLinie(_linieInProgres);
                    _drawUtils.ColoreazaInteriorObiect(_obiectInProgres, Color.Red, Brushes.Red);

                    _objects.Add(_obiectInProgres);
                    _obiectInProgres.ObiectInitial = new Obiect(_obiectInProgres);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                _objects.ForEach(o => _drawUtils.ExpandareUniforma(o, numericUpDown2.Value * 2));
            }
            else
            {
                _objects.ForEach(o => _drawUtils.ExpandareNeuniforma(o, numericSus.Value, numericDreapta.Value, numericJos.Value, numericStanga.Value));
            }
            
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            _drawUtils = new DrawUtils(pictureBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _objects.ForEach(o => o.Uniformizare(_drawUtils));
            _objects.ForEach(o => _drawUtils.ColoreazaInteriorObiect(o, Color.Yellow, Brushes.Yellow));
            _objects.ForEach(o => _drawUtils.ColoreazaInteriorObiect(o.ObiectInitial, Color.Red, Brushes.Red));
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox2.Checked;

            numericDreapta.Enabled = numericJos.Enabled = numericStanga.Enabled = numericSus.Enabled = !checkBox2.Checked;
        }
    }
}