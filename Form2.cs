using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LobotomyBaseGUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void KrBtn_Click(object sender, EventArgs e)
        {
            SetLangAndRun("kr");
        }
        public void SetLangAndRun(string lang)
        {
            Program.CurLang = lang;

            LocalizeDataList data = LocalizeDataList.LoadData(Application.StartupPath + "/Localize/"+ lang + "/Localize.xml");
            LocalizeManager.Instance.Init(data);

            Program.theForm1.Init();
            Program.theForm1.Show();
            Program.theForm1.FormClosed += new FormClosedEventHandler(this.Exit);
            this.Hide();
        }
        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnBtn_Click(object sender, EventArgs e)
        {
            SetLangAndRun("en");
        }
    }
}
