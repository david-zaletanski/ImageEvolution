using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class frmMain : Form
    {
        #region Variables

        ImageEvolutionChamber chamber;

        #endregion

        #region Constructor

        public frmMain()
        {
            InitializeComponent();
            chamber = new ImageEvolutionChamber();

            chamber.FittestGeneratedImageUpdate += new ImageEvolutionChamber.ImageUpdateDelegate(chamber_FittestGeneratedImageUpdate);
            chamber.GenerationUpdate += new ImageEvolutionChamber.GenerationUpdateDelegate(chamber_GenerationUpdate);
            chamber.GenerationProgress += new ImageEvolutionChamber.GenerationProgressDelegate(chamber_GenerationProgress);
            chamber.StatusUpdateEvent += new ImageEvolutionChamber.StatusUpdateDelegate(chamber_StatusUpdateEvent);
        }

        #endregion

        #region Public Functions

        public void OnExit()
        {
            chamber.Stop();
        }

        private delegate void addOutputDelegate(string output);
        public void AddOutput(string output)
        {
            if (outputBox.InvokeRequired)
            {
                this.Invoke(new addOutputDelegate(AddOutput), new object[] { output });
            }
            else
            {
                outputBox.AppendText(output + "\n");
                outputBox.Select(outputBox.Text.Length, 0);
                outputBox.ScrollToCaret();
            }
        }

        #endregion

        #region Event Handlers

        private delegate void statusUpdateDelegate(string status);
        void chamber_StatusUpdateEvent(string status)
        {
            if (statusLbl.InvokeRequired && !this.IsDisposed)
            {
                this.Invoke(new statusUpdateDelegate(chamber_StatusUpdateEvent), new object[] { status });
            }
            else
            {
                statusLbl.Text = status;
            }
        }

        private delegate void generationProgressDelegate(int current, int total);
        void chamber_GenerationProgress(int current, int total)
        {
            if (generationProgBar.InvokeRequired && !this.IsDisposed)
            {
                this.Invoke(new generationProgressDelegate(chamber_GenerationProgress), new object[] { current, total });
            }
            else
            {
                generationProgBar.Maximum = total;
                generationProgBar.Value = current;
            }
        }

        private delegate void generationUpdateDelegate(int generation);
        private void chamber_GenerationUpdate(int generation)
        {
            if (generationNumTxtBox.InvokeRequired && !this.IsDisposed)
            {
                this.Invoke(new generationUpdateDelegate(chamber_GenerationUpdate), new object[] { generation });
            }
            else
            {
                this.generationNumTxtBox.Text = generation.ToString();
            }
        }

        private delegate void fittestUpdateDelegate(Image img);
        private void chamber_FittestGeneratedImageUpdate(Image img)
        {
            if (genImgBox.InvokeRequired && !this.IsDisposed)
            {
                this.Invoke(new fittestUpdateDelegate(chamber_FittestGeneratedImageUpdate), new object[] { img });
            }
            else
            {
                if (this.genImgBox.Image != null)
                {
                    this.genImgBox.Image.Dispose();
                    this.genImgBox.Image = null;
                }
                Image resizedImg = ResizeImage(img, genImgBox.Width, genImgBox.Height);
                this.genImgBox.Image = resizedImg;
            }
        }

        #endregion

        #region Control Events

        private void loadSrcImgBtn_Click(object sender, EventArgs e)
        {
            // Load source image.
            OpenFileDialog loadImgDialog = new OpenFileDialog();
            loadImgDialog.Filter = "Pictures (*.jpg)|*.jpg";
            loadImgDialog.FilterIndex = 1;
            loadImgDialog.Multiselect = false;
            DialogResult result = loadImgDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                chamber.LoadSourceImage(loadImgDialog.FileName);
                Image resizedImg = ResizeImage(chamber.SourceImage, srcImgBox.Width, srcImgBox.Height);
                this.srcImgBox.Image = resizedImg;
            }
        }

        private void evolutionControlBtn_Click(object sender, EventArgs e)
        {
            // Check That Directory To Save Images Exists / Is Valid
            if (saveImagesChkBox.Checked)
            {
                string saveLocation = saveLocationTxtBox.Text;
                if (saveLocation == null || saveLocation.Length == 0 ||
                    !System.IO.Directory.Exists(saveLocation))
                {
                    MessageBox.Show("Please select valid image save location or disable image capture.");
                    return;
                }
            }
            // Evolution Control button.
            if (evolutionControlBtn.Text == "Begin")
            {
                mutationRateTxtBox.ReadOnly = true;
                numberOfChildrenTxtBox.ReadOnly = true;
                saveAmtTxtBox.ReadOnly = true;
                try
                {
                    chamber.Begin(Double.Parse(mutationRateTxtBox.Text), Int32.Parse(numberOfChildrenTxtBox.Text),saveLocationTxtBox.Text,Int32.Parse(saveAmtTxtBox.Text), saveImagesChkBox.Checked);
                    evolutionControlBtn.Text = "Stop";
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error starting evolution. This is likely due to invalid input.\n"+ex.Message);
                }
            }
            else if (evolutionControlBtn.Text == "Stop")
            {
                chamber.Stop();
                evolutionControlBtn.Text = "Begin";
            }
        }

        private void selectSaveLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                saveLocationTxtBox.Text = fbd.SelectedPath;
            }
        }

        private Image ResizeImage(Image Source, int width, int height)
        {
            if (Source == null)
                return null;

            var newImg = new Bitmap(width, height);
            Graphics.FromImage(newImg).DrawImage(Source, 0, 0, width, height);
            return newImg;
        }

        #endregion
    }
}
