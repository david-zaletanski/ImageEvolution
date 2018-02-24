namespace net.dzale.ImageEvolution
{
    partial class frmMain
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
            this.srcImgBox = new System.Windows.Forms.PictureBox();
            this.genImgBox = new System.Windows.Forms.PictureBox();
            this.loadSrcImgBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.generationNumTxtBox = new System.Windows.Forms.TextBox();
            this.evolutionControlBtn = new System.Windows.Forms.Button();
            this.mutationRateTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numberOfChildrenTxtBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.generationProgBar = new System.Windows.Forms.ProgressBar();
            this.statusLbl = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.saveLocationTxtBox = new System.Windows.Forms.TextBox();
            this.selectSaveLocationBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.saveAmtTxtBox = new System.Windows.Forms.TextBox();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.saveImagesChkBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.srcImgBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.genImgBox)).BeginInit();
            this.SuspendLayout();
            // 
            // srcImgBox
            // 
            this.srcImgBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srcImgBox.Location = new System.Drawing.Point(28, 36);
            this.srcImgBox.Name = "srcImgBox";
            this.srcImgBox.Size = new System.Drawing.Size(350, 400);
            this.srcImgBox.TabIndex = 0;
            this.srcImgBox.TabStop = false;
            // 
            // genImgBox
            // 
            this.genImgBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genImgBox.Location = new System.Drawing.Point(399, 36);
            this.genImgBox.Name = "genImgBox";
            this.genImgBox.Size = new System.Drawing.Size(350, 400);
            this.genImgBox.TabIndex = 1;
            this.genImgBox.TabStop = false;
            // 
            // loadSrcImgBtn
            // 
            this.loadSrcImgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadSrcImgBtn.Location = new System.Drawing.Point(790, 36);
            this.loadSrcImgBtn.Name = "loadSrcImgBtn";
            this.loadSrcImgBtn.Size = new System.Drawing.Size(148, 33);
            this.loadSrcImgBtn.TabIndex = 2;
            this.loadSrcImgBtn.Text = "Load Source Image";
            this.loadSrcImgBtn.UseVisualStyleBackColor = true;
            this.loadSrcImgBtn.Click += new System.EventHandler(this.loadSrcImgBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(787, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Generation:";
            // 
            // generationNumTxtBox
            // 
            this.generationNumTxtBox.Location = new System.Drawing.Point(855, 213);
            this.generationNumTxtBox.Name = "generationNumTxtBox";
            this.generationNumTxtBox.ReadOnly = true;
            this.generationNumTxtBox.Size = new System.Drawing.Size(83, 20);
            this.generationNumTxtBox.TabIndex = 4;
            this.generationNumTxtBox.Text = "0";
            this.generationNumTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // evolutionControlBtn
            // 
            this.evolutionControlBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.evolutionControlBtn.Location = new System.Drawing.Point(790, 136);
            this.evolutionControlBtn.Name = "evolutionControlBtn";
            this.evolutionControlBtn.Size = new System.Drawing.Size(148, 33);
            this.evolutionControlBtn.TabIndex = 5;
            this.evolutionControlBtn.Text = "Begin";
            this.evolutionControlBtn.UseVisualStyleBackColor = true;
            this.evolutionControlBtn.Click += new System.EventHandler(this.evolutionControlBtn_Click);
            // 
            // mutationRateTxtBox
            // 
            this.mutationRateTxtBox.Location = new System.Drawing.Point(870, 248);
            this.mutationRateTxtBox.Name = "mutationRateTxtBox";
            this.mutationRateTxtBox.Size = new System.Drawing.Size(68, 20);
            this.mutationRateTxtBox.TabIndex = 7;
            this.mutationRateTxtBox.Text = "0.05";
            this.mutationRateTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(787, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mutation Rate:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Source Image";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(396, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Fittest Generated Image";
            // 
            // numberOfChildrenTxtBox
            // 
            this.numberOfChildrenTxtBox.Location = new System.Drawing.Point(870, 274);
            this.numberOfChildrenTxtBox.Name = "numberOfChildrenTxtBox";
            this.numberOfChildrenTxtBox.Size = new System.Drawing.Size(68, 20);
            this.numberOfChildrenTxtBox.TabIndex = 11;
            this.numberOfChildrenTxtBox.Text = "150";
            this.numberOfChildrenTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(787, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "# of Children:";
            // 
            // generationProgBar
            // 
            this.generationProgBar.Location = new System.Drawing.Point(790, 193);
            this.generationProgBar.Name = "generationProgBar";
            this.generationProgBar.Size = new System.Drawing.Size(148, 14);
            this.generationProgBar.TabIndex = 12;
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Location = new System.Drawing.Point(787, 177);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(54, 13);
            this.statusLbl.TabIndex = 13;
            this.statusLbl.Text = "No Status";
            this.statusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(787, 380);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Save Location:";
            // 
            // saveLocationTxtBox
            // 
            this.saveLocationTxtBox.Location = new System.Drawing.Point(790, 396);
            this.saveLocationTxtBox.Name = "saveLocationTxtBox";
            this.saveLocationTxtBox.ReadOnly = true;
            this.saveLocationTxtBox.Size = new System.Drawing.Size(148, 20);
            this.saveLocationTxtBox.TabIndex = 15;
            // 
            // selectSaveLocationBtn
            // 
            this.selectSaveLocationBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectSaveLocationBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectSaveLocationBtn.Location = new System.Drawing.Point(894, 371);
            this.selectSaveLocationBtn.Name = "selectSaveLocationBtn";
            this.selectSaveLocationBtn.Size = new System.Drawing.Size(44, 22);
            this.selectSaveLocationBtn.TabIndex = 16;
            this.selectSaveLocationBtn.Text = "Select";
            this.selectSaveLocationBtn.UseVisualStyleBackColor = true;
            this.selectSaveLocationBtn.Click += new System.EventHandler(this.selectSaveLocation_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(787, 419);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Every # of Generations:";
            // 
            // saveAmtTxtBox
            // 
            this.saveAmtTxtBox.Location = new System.Drawing.Point(905, 416);
            this.saveAmtTxtBox.Name = "saveAmtTxtBox";
            this.saveAmtTxtBox.Size = new System.Drawing.Size(33, 20);
            this.saveAmtTxtBox.TabIndex = 18;
            this.saveAmtTxtBox.Text = "0";
            this.saveAmtTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(28, 464);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(910, 87);
            this.outputBox.TabIndex = 19;
            this.outputBox.Text = "";
            // 
            // saveImagesChkBox
            // 
            this.saveImagesChkBox.AutoSize = true;
            this.saveImagesChkBox.Location = new System.Drawing.Point(790, 351);
            this.saveImagesChkBox.Name = "saveImagesChkBox";
            this.saveImagesChkBox.Size = new System.Drawing.Size(88, 17);
            this.saveImagesChkBox.TabIndex = 20;
            this.saveImagesChkBox.Text = "Save Images";
            this.saveImagesChkBox.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 563);
            this.Controls.Add(this.saveImagesChkBox);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.saveAmtTxtBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.selectSaveLocationBtn);
            this.Controls.Add(this.saveLocationTxtBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.generationProgBar);
            this.Controls.Add(this.numberOfChildrenTxtBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mutationRateTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.evolutionControlBtn);
            this.Controls.Add(this.generationNumTxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loadSrcImgBtn);
            this.Controls.Add(this.genImgBox);
            this.Controls.Add(this.srcImgBox);
            this.Name = "frmMain";
            this.Text = "Image Evolution";
            ((System.ComponentModel.ISupportInitialize)(this.srcImgBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.genImgBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox srcImgBox;
        private System.Windows.Forms.PictureBox genImgBox;
        private System.Windows.Forms.Button loadSrcImgBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox generationNumTxtBox;
        private System.Windows.Forms.Button evolutionControlBtn;
        private System.Windows.Forms.TextBox mutationRateTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox numberOfChildrenTxtBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar generationProgBar;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox saveLocationTxtBox;
        private System.Windows.Forms.Button selectSaveLocationBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox saveAmtTxtBox;
        private System.Windows.Forms.RichTextBox outputBox;
        private System.Windows.Forms.CheckBox saveImagesChkBox;
    }
}

