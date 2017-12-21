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

using ClipperLib;

using Path = System.Collections.Generic.List<System.Drawing.Point>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<System.Drawing.Point>>;
using System.Drawing.Drawing2D;

namespace Expandare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            _inProgres = false;
            _obiectInProgres = null;
            _obiectInMove = null;
            _linieInProgres = null;

            _isNearInitialPoint = false;

            _objects = new List<Obiect>();

            _drawUtils = new DrawUtils(pictureBox1);
        }

        private bool _inProgres;
        private Obiect _obiectInProgres;
        private Obiect _obiectInMove;
        private Point _startMovePoint;
        private bool _isMovingObject = false;
        private Linie _linieInProgres;
        private System.Drawing.Point _initialPoint;
        private bool _isNearInitialPoint;
        private List<Obiect> _objects;

        private DrawUtils _drawUtils;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var punct = new System.Drawing.Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);

            if (!_inProgres)
            {
                if (_obiectInMove == null)
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
                    _linieInProgres.End = new System.Drawing.Point(_initialPoint.X, _initialPoint.Y);
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
            _obiectInMove = null;
            _obiectInProgres = null;
            _objects = new List<Obiect>();
            _drawUtils.StergeObiecte();
            _inProgres = false;
            _isMovingObject = false;
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
            else if (!_inProgres)
            {
                var point = new Point(e.X, e.Y);
                var isObject = false;
                foreach (var obiect in _objects)
                {
                    if (obiect.ContinePunct(point))
                    {
                        _obiectInMove = obiect;
                        isObject = true;
                        break;
                    }
                }

                if (isObject)
                {
                    Cursor.Current = Cursors.UpArrow;
                }
                else
                {
                    if (!_isMovingObject)
                    {
                        _obiectInMove = null;
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Checked;
        }

        private void GenerareExpandare(object sender, EventArgs e)
        {
            _drawUtils.StergeObiecte();

            var colturi = ((radioButton1.Checked ? 1 : 0) * 1) + ((radioButton2.Checked ? 1 : 0) * 2) + ((radioButton3.Checked ? 1 : 0) * 3);

            if (checkBox2.Checked)
            {
                _objects.ForEach(o => _drawUtils.ExpandareUniforma(o, numericUpDown2.Value, colturi, concavitateCheckBox.Checked));
            }
            else
            {
                _objects.ForEach(o => _drawUtils.ExpandareNeuniforma(o, numericSus.Value, numericDreapta.Value, numericStanga.Value, numericJos.Value, colturi, concavitateCheckBox.Checked));
            }
            
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            _drawUtils = new DrawUtils(pictureBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _objects.ForEach(o => o.Uniformizare());
            _objects.ForEach(o => _drawUtils.ColoreazaInteriorObiect(o, Color.Yellow, Brushes.Yellow));
            _objects.ForEach(o => _drawUtils.ColoreazaInteriorObiect(o.ObiectInitial, Color.Red, Brushes.Red));
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox2.Checked;

            numericDreapta.Enabled = numericJos.Enabled = numericStanga.Enabled = numericSus.Enabled = !checkBox2.Checked;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_obiectInMove != null)
            {
                _startMovePoint = new Point(e.X, e.Y);
                _isMovingObject = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var endMovePoint = new Point(e.X, e.Y);

            if (_obiectInMove != null && _startMovePoint != null && _startMovePoint != new Point(-1, -1))
            {
                var xDelta = endMovePoint.X - _startMovePoint.X;
                var yDelta = endMovePoint.Y - _startMovePoint.Y;

                var obiect = _objects.First(o => o.Id == _obiectInMove.Id);

                var puncte = new List<Point>();

                obiect.Varfuri.ForEach(p =>
                {
                    puncte.Add(new Point(p.X + xDelta, p.Y + yDelta));
                });

                var puncteInitiale = new List<Point>();

                obiect.ObiectInitial.Varfuri.ForEach(p =>
                {
                    puncteInitiale.Add(new Point(p.X + xDelta, p.Y + yDelta));
                });

                obiect.Varfuri = puncte;
                obiect.ObiectInitial.Varfuri = puncteInitiale;

                GenerareExpandare(null, null);
                _obiectInMove = null;
            }

            _startMovePoint = new Point(-1, -1);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (_obiectInMove != null)
            {
                if (MessageBox.Show("Sunteți sigur că doriți ștergerea acestui obiect?", "Atenție", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _objects.Remove(_obiectInMove);
                    _obiectInMove = null;

                    GenerareExpandare(null, null);
                }
            }
        }
    }
}