using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starcatcher
{
    public partial class Messagebox : Form
    {
        public Messagebox(int points)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            label1.Font = CustomFont.GetFont(25, FontStyle.Regular);
            label1.Text = $"You need {points} more stars!";

            button1.Font = CustomFont.GetFont(25, FontStyle.Regular);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Messagebox_Load(object sender, EventArgs e)
        {
            ApplyCustomFont(this);
        }

        private void ApplyCustomFont(Control container)
        {
            container.Font = CustomFont.GetFont(25, FontStyle.Regular);
        }
    }
}
