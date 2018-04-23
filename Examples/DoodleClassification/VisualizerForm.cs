using System;
using System.Windows.Forms;
using ToyNN;

namespace DoodleClassification
{
    public partial class VisualizerForm
    {
        public VisualizerForm()
        {
            InitializeComponent();
        }

        public void Update(NeuralNetwork nn)
        {
            _networkVisualizer.Refresh(nn);
        }
    }
}