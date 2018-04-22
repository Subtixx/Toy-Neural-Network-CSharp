using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ToyNN;

namespace DoodleClassification
{
    public class Main
    {
        private static readonly Random _random = new Random();

        private readonly NeuralNetwork _neuralNetwork;
        public Doodle[][] TestingLabelSet;
        public Doodle[] TestingSet;

        public Doodle[][] TrainingLabelSet;

        public Doodle[] TrainingSet;

        public Main()
        {
            _neuralNetwork = new NeuralNetwork(784, 64, 3);

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
            Shuffle(TrainingSet);

            TestingSet = new Doodle[200 * 3];
            Array.Copy(TestingLabelSet[(int) Doodle.DoodleLabel.Cat], 0, TestingSet, 0, 200);
            Array.Copy(TestingLabelSet[(int) Doodle.DoodleLabel.Rainbow], 0, TestingSet, 200, 200);
            Array.Copy(TestingLabelSet[(int) Doodle.DoodleLabel.Train], 0, TestingSet, 400, 200);
            Shuffle(TestingSet);
        }

        /// <summary>
        ///     Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        private static void Shuffle<T>(IList<T> array)
        {
            var n = array.Count;
            for (var i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                var r = i + _random.Next(n - i);
                var t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
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
                    TrainingLabelSet[(int) label][i].Label = Doodle.DoodleLabel.Cat;
                }
                else
                {
                    TestingLabelSet[(int) label][i - threshold].Data = new byte[len];
                    Array.Copy(dataSet, offset, TestingLabelSet[(int) label][i - threshold].Data, 0, len);
                    TestingLabelSet[(int) label][i - threshold].Label = Doodle.DoodleLabel.Cat;
                }
            }
        }

        public void TrainEpoch(Doodle[] doodles)
        {
            Shuffle(doodles);
            foreach (var data in doodles)
            {
                var inputs = new float[data.Data.Length];
                for (var i1 = 0; i1 < data.Data.Length; i1++)
                    inputs[i1] = data.Data[i1] / 255.0f;

                var targets = new[] {0.0f, 0.0f, 0.0f};
                targets[(int) data.Label] = 1;

                _neuralNetwork.Train(inputs, targets);
            }
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

                if (classification == (int) label)
                    correct++;
            }

            var percent = 100 * correct / doodles.Length;
            return percent;
        }
    }

    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());

            //new Main();
        }
    }
}