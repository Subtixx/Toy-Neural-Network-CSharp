using System;
using System.IO;

namespace ToyNN
{
    public class NeuralNetwork
    {
        private static readonly ActivationFunction sigmoid = new ActivationFunction(
            (x, row, col) => 1 / (1 + Math.Exp(-x)),
            (y, row, col) => y * (1 - y)
        );

        private static ActivationFunction tanh = new ActivationFunction(
            (x, row, col) => Math.Tanh(x),
            (y, row, col) => 1 - y * y
        );

        private ActivationFunction _activationFunction;

        private float _learningRate;

        public NeuralNetwork(NeuralNetwork nn)
        {
            InputNodes = nn.InputNodes;
            HiddenNodes = nn.HiddenNodes;
            OutputNodes = nn.InputNodes;

            WeightsInputHidden = nn.WeightsInputHidden;
            WeightsHiddenOutput = nn.WeightsHiddenOutput;

            BiasHidden = nn.BiasHidden;
            BiasOutput = nn.BiasOutput;

            _learningRate = nn._learningRate;
            _activationFunction = nn._activationFunction;
        }

        public NeuralNetwork(int inputNodes, int hiddenNodes, int outputNodes)
        {
            InputNodes = inputNodes;
            HiddenNodes = hiddenNodes;
            OutputNodes = outputNodes;

            WeightsInputHidden = new Matrix(hiddenNodes, inputNodes);
            WeightsHiddenOutput = new Matrix(outputNodes, hiddenNodes);

            WeightsInputHidden.Randomize();
            WeightsHiddenOutput.Randomize();

            BiasHidden = new Matrix(hiddenNodes, 1);
            BiasOutput = new Matrix(outputNodes, 1);
            BiasHidden.Randomize();
            BiasOutput.Randomize();

            SetLearningRate();
            SetActivationFunction();
        }

        public int Epoch { get; set; }

        public int InputNodes { get; }

        public int HiddenNodes { get; }

        public int OutputNodes { get; }

        public Matrix WeightsInputHidden { get; }

        public Matrix WeightsHiddenOutput { get; }

        public Matrix BiasHidden { get; }
        public Matrix BiasOutput { get; }

        public float[] Predict(float[] inputArray)
        {
            // Generating the Hidden Outputs
            var inputs = Matrix.FromArray(inputArray);
            var hidden = Matrix.Multiply(WeightsInputHidden, inputs);
            hidden.Add(BiasHidden);

            // activation function!
            hidden.Map(_activationFunction.Func);

            // Generating the output's output!
            var output = Matrix.Multiply(WeightsHiddenOutput, hidden);
            output.Add(BiasOutput);
            output.Map(_activationFunction.Func);

            return output.ToArray();
        }

        public void SetLearningRate(float learningRate = 0.1f)
        {
            _learningRate = learningRate;
        }

        public void SetActivationFunction(ActivationFunction func = null)
        {
            _activationFunction = func ?? sigmoid;
        }

        public void Train(float[] inputArray, float[] targetArray)
        {
            var inputs = Matrix.FromArray(inputArray);
            var hidden = Matrix.Multiply(WeightsInputHidden, inputs);
            hidden.Add(BiasHidden);
            hidden.Map(_activationFunction.Func);

            var outputs = Matrix.Multiply(WeightsHiddenOutput, hidden);
            outputs.Add(BiasOutput);
            outputs.Map(_activationFunction.Func);

            var targets = Matrix.FromArray(targetArray);
            var outputErrors = Matrix.Subtract(targets, outputs);

            var gradients = Matrix.Map(outputs, _activationFunction.Dfunc);
            gradients.Multiply(outputErrors);
            gradients.Multiply(_learningRate);

            var hiddenTranspose = Matrix.Transpose(hidden);
            var weightsHiddenOutputDeltas = Matrix.Multiply(gradients, hiddenTranspose);

            // Adjust the weights by deltas
            WeightsHiddenOutput.Add(weightsHiddenOutputDeltas);
            // Adjust the bias by its deltas (which is just the gradients)
            BiasOutput.Add(gradients);

            // Calculate the hidden layer errors
            var weightsHiddenOutputTranspose = Matrix.Transpose(WeightsHiddenOutput);
            var hiddenErrors = Matrix.Multiply(weightsHiddenOutputTranspose, outputErrors);

            // Calculate hidden gradient
            var hiddenGradient = Matrix.Map(hidden, _activationFunction.Dfunc);
            hiddenGradient.Multiply(hiddenErrors);
            hiddenGradient.Multiply(_learningRate);

            // Calcuate input->hidden deltas
            var inputsTranspose = Matrix.Transpose(inputs);
            var weightInputHiddenDeltas = Matrix.Multiply(hiddenGradient, inputsTranspose);

            WeightsInputHidden.Add(weightInputHiddenDeltas);
            // Adjust the bias by its deltas (which is just the gradients)
            BiasHidden.Add(hiddenGradient);
        }

        public static NeuralNetwork Load(string file)
        {
            using (var fs = File.OpenRead(file))
            {
                using (var br = new BinaryReader(fs))
                {
                    var epoch = br.ReadInt32();
                    var inputNodes = br.ReadInt32();
                    var hiddenNodes = br.ReadInt32();
                    var outputNodes = br.ReadInt32();
                    var nn = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes)
                    {
                        Epoch = epoch
                    };
                    nn.WeightsInputHidden.Load(br);
                    nn.WeightsHiddenOutput.Load(br);
                    nn.BiasHidden.Load(br);
                    nn.BiasOutput.Load(br);
                    nn._learningRate = br.ReadSingle();

                    return nn;
                }
            }
        }

        public void Save(string file)
        {
            using (var fs = File.OpenWrite(file))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(Epoch);

                    bw.Write(InputNodes);
                    bw.Write(HiddenNodes);
                    bw.Write(OutputNodes);

                    WeightsInputHidden.Save(bw);
                    WeightsHiddenOutput.Save(bw);
                    BiasHidden.Save(bw);
                    BiasOutput.Save(bw);
                    bw.Write(_learningRate);
                }
            }
        }

        public NeuralNetwork Copy()
        {
            return new NeuralNetwork(this);
        }

        public void Mutate(Func<float, int, int, float> func)
        {
            WeightsInputHidden.Map(func);
            WeightsHiddenOutput.Map(func);
            BiasHidden.Map(func);
            BiasOutput.Map(func);
        }
    }
}