namespace LobotomyBaseGUI
{
    partial class Form2
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
            this.KrBtn = new System.Windows.Forms.Button();
            this.EnBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // KrBtn
            // 
            this.KrBtn.Location = new System.Drawing.Point(110, 150);
            this.KrBtn.Name = "KrBtn";
            this.KrBtn.Size = new System.Drawing.Size(80, 25);
            this.KrBtn.TabIndex = 0;
            this.KrBtn.Text = "한국어";
            this.KrBtn.UseVisualStyleBackColor = true;
            this.KrBtn.Click += new System.EventHandler(this.KrBtn_Click);
            // 
            // EnBtn
            // 
            this.EnBtn.Location = new System.Drawing.Point(110, 250);
            this.EnBtn.Name = "EnBtn";
            this.EnBtn.Size = new System.Drawing.Size(80, 25);
            this.EnBtn.TabIndex = 1;
            this.EnBtn.Text = "English";
            this.EnBtn.UseVisualStyleBackColor = true;
            this.EnBtn.Click += new System.EventHandler(this.EnBtn_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Controls.Add(this.EnBtn);
            this.Controls.Add(this.KrBtn);
            this.Name = "Form2";
            this.Text = "";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button KrBtn;
        private System.Windows.Forms.Button EnBtn;
    }
}