using System;
using System.Drawing;
using System.Windows.Forms;
using ToyNN;

namespace DoodleClassification
{
    public class NeuralNetworkVisualizer : Control
    {
        private Graphics _graphics;
        private Form _parentForm;
        private NeuralNetwork _lastNeuralNetworkState;

        public NeuralNetworkVisualizer(Form form)
        {
            DoubleBuffered = true;

            _parentForm = form;
            Size = _parentForm.Size;
            var img = new Bitmap(Size.Width, Size.Height);
            BackgroundImage = img;
            _graphics = Graphics.FromImage(img);
            _graphics.Clear(Color.Black);
        }

        public void Refresh(NeuralNetwork nn)
        {
            _lastNeuralNetworkState = nn;
            _graphics.Clear(Color.Black);
            DrawNeuralNetwork(_graphics, new[] {nn.WeightsInputHidden, nn.WeightsHiddenOutput}, 1, 1, Width - 2,
                Height - 2);
        }

        private void DrawNeuralNetwork(Graphics graphics, Matrix[] weights, int x, int y, int width, int height)
        {
            var pen = new Pen(new SolidBrush(Color.FromArgb(255, 60, 60, 60)));
            graphics.DrawRectangle(pen, new Rectangle(x, y, width, height));

            for (var i = 0; i < weights.Length; i++)
            {
                DrawLayer(graphics, weights[i], x + i * (width / (weights.Length + 1)), y, width / (weights.Length + 1),
                    height);
            }

            var layer = weights.Length;
            DrawLayer(graphics, new Matrix(0, 1), x + layer * (width / (weights.Length + 1)), y,
                width / (weights.Length + 1), height);
        }

        private void DrawLayer(Graphics graphics, Matrix weights, int x, int y, int width, int height)
        {
            var numNeurons = weights.Columns;
            var nextNeuronsX = new float[weights.Rows];
            var nextNeuronsY = new float[weights.Rows];

            var neuronSize = Math.Min(width / 3, height / numNeurons);
            var maxStroke = Math.Max(1, neuronSize / 6);

            //compute min and max values for the weights (used for color and stroke)
            float? min = null;
            float? max = null;

            foreach (var t in weights.Data)
            {
                foreach (var t1 in t)
                {
                    if (!min.HasValue || min > t1)
                        min = t1;
                    if (!max.HasValue || max < t1)
                        max = t1;
                }
            }

            //compute positions of neurons in next layer (needed to draw synapses)
            for (var j = 0; j < weights.Rows; j++)
            {
                nextNeuronsX[j] = x + width + (width / 2);
                nextNeuronsY[j] = y + height / (weights.Rows + 1) * (j + 1);
            }

            //fill(255);
            for (var i = 0; i < numNeurons; i++)
            {
                // draw neurons
                //stroke(255);
                //strokeWeight(1);
                var curX = x + width / 2;
                var curY = y + height / (numNeurons + 1) * (i + 1);

                //draw synapses
                for (var j = 0; j < weights.Rows; j++)
                {
                    //compute color based on weight: red for negative, green for positive
                    var synWeight = weights.Data[j][i];
                    var red = synWeight < 0 ? Map((int) synWeight, min.Value, 0, 0, 255) : 0;
                    var green = synWeight >= 0 ? Map((int) synWeight, 0, max.Value, 0, 255) : 0;
                    //stroke(red, green, 0);

                    //bigger stroke weight for heavier weights
                    //strokeWeight(map(Math.abs(synWeight), 0, Math.max(max, Math.abs(min)), 0, maxStroke));

                    var strokeWeight = Map(Math.Abs(synWeight), 0, Math.Max(max.Value, Math.Abs(min.Value)), 0,
                        maxStroke);
                    var pen = new Pen(new SolidBrush(Color.FromArgb(255, (int) red, (int) green, 0)),
                        strokeWeight);
                    graphics.DrawLine(pen,
                        curX + neuronSize / 2.0f, curY + neuronSize / 2.0f, nextNeuronsX[j] + neuronSize / 2.0f,
                        nextNeuronsY[j] + neuronSize / 2.0f);
                }

                graphics.FillEllipse(new SolidBrush(Color.White), curX, curY, Math.Max(0.5f, neuronSize),
                    Math.Max(0.5f, neuronSize));
            }
        }

        private int Map(int n, int start1, int stop1, int start2, int stop2, bool withinBounds = false)
        {
            var newval = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
                return newval;

            return start2 < stop2
                ? Math.Max(Math.Min(newval, start2), stop2)
                : Math.Max(Math.Min(newval, stop2), start2);
        }

        private float Map(float n, float start1, float stop1, float start2, float stop2, bool withinBounds = false)
        {
            var newval = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
                return newval;

            return start2 < stop2
                ? Math.Max(Math.Min(newval, start2), stop2)
                : Math.Max(Math.Min(newval, stop2), start2);
        }

        public void OnResizeEnd(object sender, EventArgs e)
        {
            Size = _parentForm.Size;

            var img = new Bitmap(BackgroundImage, Size.Width, Size.Height);
            BackgroundImage = img;
            _graphics = Graphics.FromImage(img);
            Refresh(_lastNeuralNetworkState);
        }
    }
}