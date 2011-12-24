namespace CodeWrapper
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCodeBox = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.btnClearBoxes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(377, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Paste your code into this box and it will be wrapped with output context writers." +
                "";
            // 
            // txtCodeBox
            // 
            this.txtCodeBox.Location = new System.Drawing.Point(16, 46);
            this.txtCodeBox.Multiline = true;
            this.txtCodeBox.Name = "txtCodeBox";
            this.txtCodeBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCodeBox.Size = new System.Drawing.Size(574, 225);
            this.txtCodeBox.TabIndex = 1;
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.LimeGreen;
            this.btnExecute.FlatAppearance.BorderColor = System.Drawing.Color.LimeGreen;
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.Location = new System.Drawing.Point(16, 278);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(95, 36);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(16, 320);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(574, 273);
            this.txtResults.TabIndex = 3;
            // 
            // btnClearBoxes
            // 
            this.btnClearBoxes.Location = new System.Drawing.Point(515, 286);
            this.btnClearBoxes.Name = "btnClearBoxes";
            this.btnClearBoxes.Size = new System.Drawing.Size(75, 23);
            this.btnClearBoxes.TabIndex = 4;
            this.btnClearBoxes.Text = "Clear Boxes";
            this.btnClearBoxes.UseVisualStyleBackColor = true;
            this.btnClearBoxes.Click += new System.EventHandler(this.btnClearBoxes_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 605);
            this.Controls.Add(this.btnClearBoxes);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtCodeBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "CondorXE Code Wrapper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCodeBox;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button btnClearBoxes;
    }
}