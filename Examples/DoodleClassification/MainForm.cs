using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToyNN;

namespace DoodleClassification
{
    public partial class MainForm
    {
        private readonly NeuralNetwork _neuralNetwork;
        private float _lastPrediction;
        public Doodle[][] TestingLabelSet;
        public Doodle[] TestingSet;

        public Doodle[][] TrainingLabelSet;

        public Doodle[] TrainingSet;

        private VisualizerForm _visualizerForm;

        public MainForm()
        {
            InitializeComponent();

            if (File.Exists("nn.bin"))
            {
                _neuralNetwork = NeuralNetwork.Load("nn.bin");
                Text = $"ToyNN - Epoch: {_neuralNetwork.Epoch}";
            }
            else
            {
                _neuralNetwork = new NeuralNetwork(784, 64, 3);
            }

            TrainingLabelSet = new Doodle[3][];
            TestingLabelSet = new Doodle[3][];
            LoadData(Doodle.DoodleLabel.Cat, File.ReadAllBytes("Data/cats1000.bin"));
            LoadData(Doodle.DoodleLabel.Rainbow, File.ReadAllBytes("Data/rainbows1000.bin"));
            LoadData(Doodle.DoodleLabel.Train, File.ReadAllBytes("Data/trains1000.bin"));

            // Each category holds 800 items. There are 3 categories
            TrainingSet = new Doodle[800 * 3];
            Array.Copy(TrainingLabelSet[(int) Doodle.DoodleLabel.Cat], 0, TrainingSet, 0, 800);
            Array.Copy(TrainingLabelSet[(int) Doodle.DoodleLabel.Rainbow], 0, TrainingSet, 800, 800);
            Array.Copy(TrainingLabelSet[(int) Doodle.DoodleLabel.Train], 0, TrainingSet, 1600, 800);
            Utilities.Shuffle(ref TrainingSet);

            TestingSet = new Doodle[200 * 3];
            Array.Copy(TestingLabelSet[(int) Doodle.DoodleLabel.Cat], 0, TestingSet, 0, 200);
            Array.Copy(TestingLabelSet[(int) Doodle.DoodleLabel.Rainbow], 0, TestingSet, 200, 200);
            Array.Copy(TestingLabelSet[(int) Doodle.DoodleLabel.Train], 0, TestingSet, 400, 200);
            Utilities.Shuffle(ref TestingSet);

            _doodleSelectNum.Maximum = TestingSet.Length;
            DisplayDoodle(TestingSet[0].Data);
            
            _visualizerForm = new VisualizerForm();
            _visualizerForm.Update(_neuralNetwork);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            _neuralNetwork.Save("nn.bin");
        }

        private void TrainButton_Click(object sender, EventArgs e)
        {
            var data = TestingSet[(int) _doodleSelectNum.Value];
            
            var inputs = new float[data.Data.Length];
            for (var i1 = 0; i1 < data.Data.Length; i1++)
                inputs[i1] = data.Data[i1] / 255.0f;

            var targets = new[] {0.0f, 0.0f, 0.0f};
            targets[(int) data.Label] = 1;

            _neuralNetwork.Train(inputs, targets);
            
            _visualizerForm.Update(_neuralNetwork);
        }
        
        private void TrainAllButton_Click(object sender, EventArgs e)
        {
            ToggleControls();
            
            Task.Run(() =>
            {
                for (var i = 0; i < _epochTrainNum.Value; i++)
                {
                    TrainEpoch(TrainingSet);
                    Invoke((MethodInvoker) delegate
                    {
                        Text = $"ToyNN - Epoch: {_neuralNetwork.Epoch}";
                        _visualizerForm.Update(_neuralNetwork);
                    });
                }

                ToggleControls(true);

                Invoke((MethodInvoker) delegate
                {
                    Text = $"ToyNN - Epoch: {_neuralNetwork.Epoch}";
                });
            });
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            var data = TestingSet[(int) _doodleSelectNum.Value];
            var inputs = new float[data.Data.Length];
            for (var i1 = 0; i1 < data.Data.Length; i1++)
                inputs[i1] = data.Data[i1] / 255.0f;

            var label = data.Label;
            var guess = _neuralNetwork.Predict(inputs);
            var m = guess.Max();
            var classification = Array.IndexOf(guess, m);

            Invoke((MethodInvoker) delegate
            {
                _percentLabel.Text = $"Showing: {label}, Predicted: {(Doodle.DoodleLabel) classification}";
            });
        }

        private void TestAllButton_Click(object sender, EventArgs e)
        {
            ToggleControls();
            
            Task.Run(() =>
            {
                var oldSelected = TestingSet[(int) _doodleSelectNum.Value];
                Utilities.Shuffle(ref TestingSet);
                var idx = Array.IndexOf(TestingSet, oldSelected);

                var percentage = TestAll(TestingSet);
                _lastPrediction = percentage;

                ToggleControls(true);
                
                Invoke((MethodInvoker) delegate
                {
                    _doodleSelectNum.Value = idx;
                    
                    Text = $"ToyNN - Epoch: {_neuralNetwork.Epoch} ({_lastPrediction:F2}%)";
                });
            });
        }

        private void TestDoodleButton_Click(object sender, EventArgs e)
        {
            var data = new byte[784];
            
            var resizedData = new Bitmap(_drawControl.Image, new Size(28, 28));
            for (var x = 0; x < resizedData.Width; x++)
            {
                for (var y = 0; y < resizedData.Height; y++)
                {
                    var color = resizedData.GetPixel(x, y);
                    data[y * resizedData.Width + x] = (byte)(color.R > 0 || color.G > 0 || color.B > 0 ? 255 : 0);
                }
            }
            var inputs = new float[data.Length];
            for (var i1 = 0; i1 < data.Length; i1++)
                inputs[i1] = data[i1] / 255.0f;

            var label = ((Doodle.DoodleLabel)Enum.Parse(typeof(Doodle.DoodleLabel), (string)_comboBox.SelectedItem));
            
            DisplayDoodle(data);
            var guess = _neuralNetwork.Predict(inputs);
            var m = guess.Max();
            var classification = Array.IndexOf(guess, m);

            Invoke((MethodInvoker) delegate
            {
                _percentLabel.Text = $"Showing: {label}, Predicted: {(Doodle.DoodleLabel) classification}";
            });
        }

        private void TrainDoodleButton_Click(object sender, EventArgs e)
        {
            var data = new byte[784];
            
            var resizedData = new Bitmap(_drawControl.Image, new Size(28, 28));
            for (var x = 0; x < resizedData.Width; x++)
            {
                for (var y = 0; y < resizedData.Height; y++)
                {
                    var color = resizedData.GetPixel(x, y);
                    data[y * resizedData.Width + x] = (byte)(color.R > 0 || color.G > 0 || color.B > 0 ? 255 : 0);
                }
            }
            
            var inputs = new float[data.Length];
            for (var i1 = 0; i1 < data.Length; i1++)
                inputs[i1] = data[i1] / 255.0f;

            var label = ((Doodle.DoodleLabel)Enum.Parse(typeof(Doodle.DoodleLabel), (string)_comboBox.SelectedItem));

            var targets = new[] {0.0f, 0.0f, 0.0f};
            targets[(int) label] = 1;

            _neuralNetwork.Train(inputs, targets);
            
            _visualizerForm.Update(_neuralNetwork);
        }

        private void DoodleSelectNum_Changed(object sender, EventArgs e)
        {
            if (_doodleSelectNum.Value >= TestingSet.Length)
            {
                _doodleSelectNum.Value = TestingSet.Length - 1;
                return;
            }

            var doodle = TestingSet[(int) _doodleSelectNum.Value];
            DisplayDoodle(doodle.Data);
            Invoke((MethodInvoker) delegate { _percentLabel.Text = $"Showing: {doodle.Label}, Predicted: ???"; });
        }

        public void LoadData(Doodle.DoodleLabel label, byte[] dataSet)
        {
            const int len = 784;
            const int totalData = 1000;

            var threshold = (int) Math.Floor(0.8f * totalData);

            TrainingLabelSet[(int) label] = new Doodle[threshold];
            TestingLabelSet[(int) label] = new Doodle[totalData - threshold];
            for (var i = 0; i < totalData; i++)
            {
                var offset = i * len;
                if (i < threshold)
                {
                    TrainingLabelSet[(int) label][i].Data = new byte[len];
                    Array.Copy(dataSet, offset, TrainingLabelSet[(int) label][i].Data, 0, len);
                    TrainingLabelSet[(int) label][i].Label = label;
                }
                else
                {
                    TestingLabelSet[(int) label][i - threshold].Data = new byte[len];
                    Array.Copy(dataSet, offset, TestingLabelSet[(int) label][i - threshold].Data, 0, len);
                    TestingLabelSet[(int) label][i - threshold].Label = label;
                }
            }
        }

        public void TrainEpoch(Doodle[] doodles)
        {
            Utilities.Shuffle(ref doodles);
            foreach (var data in doodles)
            {
                var inputs = new float[data.Data.Length];
                for (var i1 = 0; i1 < data.Data.Length; i1++)
                    inputs[i1] = data.Data[i1] / 255.0f;

                var targets = new[] {0.0f, 0.0f, 0.0f};
                targets[(int) data.Label] = 1;

                _neuralNetwork.Train(inputs, targets);
            }

            _neuralNetwork.Epoch++;
        }

        public float TestAll(Doodle[] doodles)
        {
            var correct = 0;
            foreach (var data in doodles)
            {
                var inputs = new float[data.Data.Length];
                for (var i1 = 0; i1 < data.Data.Length; i1++)
                    inputs[i1] = data.Data[i1] / 255.0f;

                var label = data.Label;
                var guess = _neuralNetwork.Predict(inputs);
                var m = guess.Max();
                var classification = Array.IndexOf(guess, m);

#if DEBUG
                Console.WriteLine($"Real: {label}, Predicted: {(Doodle.DoodleLabel) classification}");
#endif

                if (classification == (int) label)
                    correct++;
            }

            var percent = 100 * correct / doodles.Length;
            return percent;
        }

        public void DisplayDoodle(byte[] doodle)
        {
            var bmp = new Bitmap(28, 28);

            for (var i = 0; i < doodle.Length; i++)
            {
                var x = i % bmp.Width;
                var y = i / bmp.Width;
                var r = doodle[i];
                var brightness = (255 - r) / 255;
                var color = Color.FromArgb(255, r, r, r);
                bmp.SetPixel(x, y, color);
            }

            _pictureBox.Image = bmp;
            _pictureBoxSmall.Image = bmp;
        }

        private void ToggleControls(bool enabled = false)
        {
            Invoke((MethodInvoker) delegate
            {
                _testDoodleButton.Enabled = enabled;
                _trainDoodleButton.Enabled = enabled;
                _epochTrainNum.Enabled = enabled;
                _trainButton.Enabled = enabled;
                _trainAllButton.Enabled = enabled;
                _testButton.Enabled = enabled;
                _testAllButton.Enabled = enabled;
            });
        }

        private void OnVisualizeButton_Click(object sender, EventArgs e)
        {
            if (!_visualizerForm.Visible)
            {
                _visualizerForm.Update(_neuralNetwork);
                _visualizerForm.Show();
            }
            else
            {
                _visualizerForm.Hide();
            }
        }
    }
}