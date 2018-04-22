using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DoodleClassification
{
    public partial class MainForm : Form
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
            this.Text = "ToyNN";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Closing += MainForm_Closing;
            
            //this.Size = new System.Drawing.Size(320, 446);
            this.Size = new System.Drawing.Size(610, 446);
            this._trainAllButton = new System.Windows.Forms.Button();
            this._trainAllButton.Text = "Train All";
            this._trainAllButton.Location = new System.Drawing.Point(10, 10);
            this._trainAllButton.Size = new System.Drawing.Size(80, 50);
            this._trainAllButton.Click += TrainAllButton_Click;
            
            this._trainButton = new System.Windows.Forms.Button();
            this._trainButton.Text = "Train";
            this._trainButton.Location = new System.Drawing.Point(190, 65);
            this._trainButton.Size = new System.Drawing.Size(80, 22);
            this._trainButton.Click += TrainButton_Click;
            
            this._epochTrainNum = new System.Windows.Forms.NumericUpDown();
            this._epochTrainNum.Location = new System.Drawing.Point(10, 66);
            this._epochTrainNum.Size = new System.Drawing.Size(80, 50);
            this._epochTrainNum.DecimalPlaces = 0;
            this._epochTrainNum.Minimum = 1;
            this._epochTrainNum.Maximum = 1000;
            
            this._testAllButton = new System.Windows.Forms.Button();
            this._testAllButton.Text = "Test All";
            this._testAllButton.Location = new System.Drawing.Point(190, 10);
            this._testAllButton.Size = new System.Drawing.Size(80, 50);
            this._testAllButton.Click += TestAllButton_Click;
            
            this._testButton = new System.Windows.Forms.Button();
            this._testButton.Text = "Test";
            this._testButton.Location = new System.Drawing.Point(100, 10);
            this._testButton.Size = new System.Drawing.Size(80, 50);
            this._testButton.Click += TestButton_Click;
            
            this._doodleSelectNum = new System.Windows.Forms.NumericUpDown();
            this._doodleSelectNum.Location = new System.Drawing.Point(100, 66);
            this._doodleSelectNum.Size = new System.Drawing.Size(80, 50);
            this._doodleSelectNum.DecimalPlaces = 0;
            this._doodleSelectNum.ValueChanged += DoodleSelectNum_Changed;

            this._percentLabel = new System.Windows.Forms.Label();
            this._percentLabel.Text = "";
            this._percentLabel.Location = new System.Drawing.Point(10, 96);
            this._percentLabel.Size = new System.Drawing.Size(280, 16);
            
            this._pictureBox = new InterpolatingPictureBox();
            this._pictureBox.InterpolationMode = InterpolationMode.NearestNeighbor;
            this._pictureBox.Location = new System.Drawing.Point(10, 116);
            this._pictureBox.Size = new System.Drawing.Size(280, 280);
            this._pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this._pictureBox.BackColor = Color.DeepPink;
            
            this._pictureBoxSmall = new InterpolatingPictureBox();
            this._pictureBoxSmall.Location = new System.Drawing.Point(10, 116);
            this._pictureBoxSmall.Size = new System.Drawing.Size(30, 30);
            this._pictureBoxSmall.SizeMode = PictureBoxSizeMode.CenterImage;
            this._pictureBoxSmall.BackColor = Color.Pink;
            
            this._drawControl = new DrawControl();
            this._drawControl.Location = new System.Drawing.Point(300, 116);
            this._drawControl.Size = new System.Drawing.Size(280, 280);
            
            this._trainDoodleButton = new System.Windows.Forms.Button();
            this._trainDoodleButton.Text = "Train Doodle";
            this._trainDoodleButton.Location = new System.Drawing.Point(310, 10);
            this._trainDoodleButton.Size = new System.Drawing.Size(80, 50);
            this._trainDoodleButton.Click += TrainDoodleButton_Click;
            
            this._testDoodleButton = new System.Windows.Forms.Button();
            this._testDoodleButton.Text = "Test Doodle";
            this._testDoodleButton.Location = new System.Drawing.Point(400, 10);
            this._testDoodleButton.Size = new System.Drawing.Size(80, 50);
            this._testDoodleButton.Click += TestDoodleButton_Click;
            
            this._clearDoodleButton = new System.Windows.Forms.Button();
            this._clearDoodleButton.Text = "Clear Doodle";
            this._clearDoodleButton.Location = new System.Drawing.Point(490, 10);
            this._clearDoodleButton.Size = new System.Drawing.Size(80, 50);
            this._clearDoodleButton.Click += (sender, args) => _drawControl.Clear();
            
            this._comboBox = new System.Windows.Forms.ComboBox();
            this._comboBox.Location = new System.Drawing.Point(400, 65);
            this._comboBox.Size = new System.Drawing.Size(80, 50);
            this._comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._comboBox.Items.Add("Cat");
            this._comboBox.Items.Add("Train");
            this._comboBox.Items.Add("Rainbow");
            this._comboBox.SelectedIndex = 0;
            
            this._toolTip = new System.Windows.Forms.ToolTip();
            this._toolTip.SetToolTip(this._trainButton, "This trains the currently displayed image");
            this._toolTip.SetToolTip(this._trainAllButton, "This trains all images in the training data");
            this._toolTip.SetToolTip(this._comboBox, "This is the category your doodle is in");
            this._toolTip.SetToolTip(this._testButton, "This will test the currently displayed image");
            this._toolTip.SetToolTip(this._testAllButton, "This will train all images in the test data");
            this._toolTip.SetToolTip(this._epochTrainNum, "This specifies how many epochs should be trained");
            this._toolTip.SetToolTip(this._doodleSelectNum, "This specifies what doodle to display, train and test");
                
            this.Controls.Add(this._pictureBoxSmall);
            this.Controls.Add(this._pictureBox);
            this.Controls.Add(this._trainButton);
            this.Controls.Add(this._trainAllButton);
            this.Controls.Add(this._percentLabel);
            this.Controls.Add(this._testButton);
            this.Controls.Add(this._comboBox);
            this.Controls.Add(this._testDoodleButton);
            this.Controls.Add(this._trainDoodleButton);
            this.Controls.Add(this._testAllButton);
            this.Controls.Add(this._doodleSelectNum);
            this.Controls.Add(this._epochTrainNum);
            this.Controls.Add(this._drawControl);
            this.Controls.Add(this._clearDoodleButton);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private InterpolatingPictureBox _pictureBox;
        private InterpolatingPictureBox _pictureBoxSmall;
        private System.Windows.Forms.Button _trainButton;
        private System.Windows.Forms.Button _trainAllButton;
        private System.Windows.Forms.Label _percentLabel;
        private System.Windows.Forms.Button _testButton;
        private System.Windows.Forms.Button _testDoodleButton;
        private System.Windows.Forms.Button _trainDoodleButton;
        private System.Windows.Forms.Button _clearDoodleButton;
        private System.Windows.Forms.Button _testAllButton;
        private System.Windows.Forms.NumericUpDown _doodleSelectNum;
        private System.Windows.Forms.NumericUpDown _epochTrainNum;
        private System.Windows.Forms.ComboBox _comboBox;
        private System.Windows.Forms.ToolTip _toolTip;

        private DrawControl _drawControl;
    }
}