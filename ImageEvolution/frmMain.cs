using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace net.dzale.ImageEvolution
{
    public partial class frmMain : Form
    {
        #region Variables

        private ImageEvolutionChamber chamber;      // Threaded program that actually executes the Image Evolution logic.

        #endregion

        #region Constructor

        public frmMain()
        {
            InitializeComponent();
            chamber = new ImageEvolutionChamber();

            // Register events for UI updates
            chamber.FittestGeneratedImageUpdate += new ImageEvolutionChamber.ImageUpdateDelegate(chamber_FittestGeneratedImageUpdate);
            chamber.GenerationUpdate += new ImageEvolutionChamber.GenerationUpdateDelegate(chamber_GenerationUpdate);
            chamber.GenerationProgress += new ImageEvolutionChamber.GenerationProgressDelegate(chamber_GenerationProgress);
            chamber.StatusUpdateEvent += new ImageEvolutionChamber.StatusUpdateDelegate(chamber_StatusUpdateEvent);
        }

        #endregion

        #region Application And Logging Functions

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

        #region UI - Status Updates and Event Handlers

        /*
                The following are thread-safe interface update calls.
        */

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

        #region UI - Controller

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
            // Starts or stops the Image Evolution thread.
            if (evolutionControlBtn.Text == "Begin")
            {
                // Valid user input parameters.
                double validMutationRate = 0.0;
                int validNumberOfChildren = 0;
                if (!Double.TryParse(mutationRateTxtBox.Text, out validMutationRate)
                    || validMutationRate <= 0.00 || validMutationRate >= 1.00)
                {
                    MessageBox.Show("Mutation rate is not a valid decimal, value must be between 0.00 and 1.00 (exclusive).");
                    return;
                }
                else if (!Int32.TryParse(numberOfChildrenTxtBox.Text, out validNumberOfChildren)
                    || validNumberOfChildren <= 0)
                {
                    MessageBox.Show("Number of children is not a valid number, value must be greater than 0.");
                    return;
                }

                // Check image saving parameters.
                bool saveImages = saveImagesChkBox.Checked;
                int validSaveInterval = 0;
                string validSaveLocation = saveLocationTxtBox.Text;
                if (saveImages)
                {
                    if (!isValidImageSaveLocation(validSaveLocation))
                    {
                        MessageBox.Show("Image save location is not a valid directory.");
                        return;
                    }
                    if (!Int32.TryParse(saveAmtTxtBox.Text, out validSaveInterval)
                        || validSaveInterval <= 0)
                    {
                        MessageBox.Show("Image save interval is not a valid number, value must be greater than 0.");
                        return;
                    }
                }

                setInputParametersReadOnly(true);
                try
                {
                    chamber.Begin(validMutationRate, validNumberOfChildren, validSaveLocation, validSaveInterval, saveImages);
                    evolutionControlBtn.Text = "Stop";
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error starting Image Evolution. Check log for more information.");
                    AddOutput("An error occurred starting Image Evolution:");
                    AddOutput(ex.Message);
                    AddOutput(ex.StackTrace);
                }
            }
            else if (evolutionControlBtn.Text == "Stop")
            {
                try {
                    chamber.Stop();
                    evolutionControlBtn.Text = "Begin";
                    setInputParametersReadOnly(false);
                } catch (Exception ex)
                {
                    AddOutput("An error occurred stopping Image Evolution:");
                    AddOutput(ex.Message);
                    AddOutput(ex.StackTrace);
                }
            }
        }

        private void setInputParametersReadOnly(bool readOnly)
        {
            mutationRateTxtBox.ReadOnly = readOnly;
            numberOfChildrenTxtBox.ReadOnly = readOnly;
            saveAmtTxtBox.ReadOnly = readOnly;
        }

        private bool isValidImageSaveLocation(string saveLocation)
        {
            // Check That Directory To Save Images Exists / Is Valid
            if (saveLocation == null || saveLocation.Length == 0 ||
                !System.IO.Directory.Exists(saveLocation))
            {
                MessageBox.Show("Please select valid image save location or disable image capture.");
                return false;
            }
            return true;
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
