using System.Drawing;
using System.Windows.Forms;

namespace DoodleClassification
{
    public partial class VisualizerForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "ToyNN - Visualizer";
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            
            //this.Size = new System.Drawing.Size(320, 446);
            this.Size = new System.Drawing.Size(610, 446);
            
            this._networkVisualizer = new NeuralNetworkVisualizer(this);
            this._networkVisualizer.Size = this.Size;
            this._networkVisualizer.Location = new Point(0, 0);

            this.ResizeEnd += _networkVisualizer.OnResizeEnd;
            this.Closing += (sender, args) =>
            {
                args.Cancel = true;
                this.Hide();
            };
            
            this.Controls.Add(_networkVisualizer);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private NeuralNetworkVisualizer _networkVisualizer;
    }
}