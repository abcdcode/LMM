using System.Windows.Forms;

namespace LobotomyBaseGUI
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

 

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.UpBtn = new System.Windows.Forms.Button();
            this.DownBtn = new System.Windows.Forms.Button();
            this.PathFindBtn = new System.Windows.Forms.Button();
            this.PathText = new System.Windows.Forms.TextBox();
            this.DescText = new System.Windows.Forms.TextBox();
            this.ApplyAndRunBtn = new System.Windows.Forms.Button();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.RemoveBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(9, 60);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(779, 212);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // UpBtn
            // 
            this.UpBtn.Location = new System.Drawing.Point(613, 376);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(80, 25);
            this.UpBtn.TabIndex = 1;
            this.UpBtn.Text = "위로";
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.UpBtnClick);
            // 
            // DownBtn
            // 
            this.DownBtn.Location = new System.Drawing.Point(613, 407);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(80, 25);
            this.DownBtn.TabIndex = 2;
            this.DownBtn.Text = "아래로";
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.DownBtnClick);
            // 
            // PathFindBtn
            // 
            this.PathFindBtn.Location = new System.Drawing.Point(708, 29);
            this.PathFindBtn.Name = "PathFindBtn";
            this.PathFindBtn.Size = new System.Drawing.Size(80, 25);
            this.PathFindBtn.TabIndex = 3;
            this.PathFindBtn.Text = "찾아보기";
            this.PathFindBtn.UseVisualStyleBackColor = true;
            this.PathFindBtn.Click += new System.EventHandler(this.PathFindBtnClick);
            // 
            // PathText
            // 
            this.PathText.Location = new System.Drawing.Point(9, 34);
            this.PathText.Name = "PathText";
            this.PathText.Size = new System.Drawing.Size(691, 21);
            this.PathText.TabIndex = 4;
            // 
            // DescText
            // 
            this.DescText.Location = new System.Drawing.Point(12, 278);
            this.DescText.Multiline = true;
            this.DescText.Name = "DescText";
            this.DescText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescText.Size = new System.Drawing.Size(595, 153);
            this.DescText.TabIndex = 5;
            // 
            // ApplyAndRunBtn
            // 
            this.ApplyAndRunBtn.Location = new System.Drawing.Point(350, 450);
            this.ApplyAndRunBtn.Name = "ApplyAndRunBtn";
            this.ApplyAndRunBtn.Size = new System.Drawing.Size(80, 25);
            this.ApplyAndRunBtn.TabIndex = 6;
            this.ApplyAndRunBtn.Text = "게임 실행";
            this.ApplyAndRunBtn.UseVisualStyleBackColor = true;
            this.ApplyAndRunBtn.Click += new System.EventHandler(this.ApplyAndRun);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(708, 278);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(80, 25);
            this.ApplyBtn.TabIndex = 7;
            this.ApplyBtn.Text = "모드 추가";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.AddMod);
            // 
            // button1
            // 
            this.RemoveBtn.Location = new System.Drawing.Point(708, 309);
            this.RemoveBtn.Name = "button1";
            this.RemoveBtn.Size = new System.Drawing.Size(80, 25);
            this.RemoveBtn.TabIndex = 8;
            this.RemoveBtn.Text = "모드 추가";
            this.RemoveBtn.UseVisualStyleBackColor = true;
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveMod);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.ApplyAndRunBtn);
            this.Controls.Add(this.DescText);
            this.Controls.Add(this.PathText);
            this.Controls.Add(this.PathFindBtn);
            this.Controls.Add(this.DownBtn);
            this.Controls.Add(this.UpBtn);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "Form1";
            this.Text = "로보토미 연동모드 매니저";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public System.Windows.Forms.CheckedListBox checkedListBox1;
        public System.Windows.Forms.Button UpBtn;
        public System.Windows.Forms.Button DownBtn;
        public System.Windows.Forms.Button PathFindBtn;
        public System.Windows.Forms.TextBox PathText;
        public System.Windows.Forms.TextBox DescText;
        public System.Windows.Forms.Button ApplyAndRunBtn;
        public System.Windows.Forms.Button ApplyBtn;
        public Button RemoveBtn;
    }
}

