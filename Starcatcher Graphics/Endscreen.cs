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
    public partial class Endscreen : Form
    {
        public Endscreen()
        {
            InitializeComponent();

            this.Font = CustomFont.GetFont(12, FontStyle.Regular);

            BackButton.Font = CustomFont.GetFont(12, FontStyle.Regular);

            this.CenterToScreen();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
