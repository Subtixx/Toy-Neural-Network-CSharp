using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DoodleClassification
{
    public sealed class DrawControl : Control
    {
        private bool _isDrawing;

        private Point _startLocation;
        private Point _endLocation;

        public Image Image { get; private set; }

        private Graphics _graphics;

        private Pen _pen;

        public DrawControl()
        {
            MouseUp += ControlMouseUp;
            MouseDown += ControlMouseDown;
            MouseMove += ControlMouseMove;

            DoubleBuffered = true;

            _pen = new Pen(new SolidBrush(Color.White), 10.0f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
            };
            Image = new Bitmap(280, 280);

            BackgroundImage = Image;

            _graphics = Graphics.FromImage(Image);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Clear();
        }

        public void Clear()
        {
            _graphics.Clear(Color.Black);
            Refresh();
        }

        private void ControlMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _endLocation == new Point(int.MinValue, int.MinValue))
            {
                _graphics.DrawEllipse(_pen, _startLocation.X, _startLocation.Y, 10, 10);
                Refresh();
                return;
            }

            _isDrawing = false;
            _endLocation = new Point(int.MinValue, int.MinValue);
        }

        private void ControlMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing) return;
            
            _endLocation = e.Location;

            if(e.Button == MouseButtons.Left)
                _graphics.DrawLine(_pen, _startLocation, _endLocation);
            else
            {
                _graphics.DrawLine(new Pen(new SolidBrush(Color.Black), 12.0f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                }, _startLocation, _endLocation);
            }

            Refresh();

            _startLocation = _endLocation;
        }

        private void ControlMouseDown(object sender, MouseEventArgs e)
        {
            _startLocation = e.Location;
            _endLocation = new Point(int.MinValue, int.MinValue);
            _isDrawing = true;
        }
    }
}