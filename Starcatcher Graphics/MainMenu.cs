using Starcatcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starcatcher
{
    public partial class MainMenu : Form
    {

        int points2 = 0;
        int StarsUpgradeLevel = 1;
        int RarityUpgradeLevel = 1;
        int UniverseUnlocked = 0;
        int DeathCounter = 0;

        public List<int> DataToSend;
        public List<int> DataToReturnMenu;
        Form1 form1 = new Form1();

        public MainMenu(List<int>SentData)
        {
            InitializeComponent();
            this.CenterToScreen();

            QuitButton.Font = CustomFont.GetFont(12, FontStyle.Regular);
            points2 = SentData[0];
            StarsUpgradeLevel = SentData[1];
            RarityUpgradeLevel = SentData[2];
            UniverseUnlocked = SentData[3];
            DeathCounter = SentData[4];
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            DataToReturnMenu = new List<int>() {points2, StarsUpgradeLevel, RarityUpgradeLevel, DeathCounter };

            form1.ShowDialog();

            if (!form1.Visible)
            {
                points2 = form1.DataToSend[0];
                StarsUpgradeLevel = form1.DataToSend[1];
                RarityUpgradeLevel = form1.DataToSend[2];
                UniverseUnlocked = form1.DataToSend[3];
                DeathCounter = form1.DataToSend[4];
            }
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ShowMenu()
        {
            this.Show();
        }


        private void MainMenu_Load(object sender, EventArgs e)
        {
            ApplyCustomFont(this);
        }
        private void ApplyCustomFont(Control container)
        {
            container.Font = CustomFont.GetFont(8, FontStyle.Regular);
        }
    }

    public static class CustomFont
    {
        public static PrivateFontCollection FontCollection = new PrivateFontCollection();
        public static string FontName { get; private set; }

        public static void Initialize()
        {
            byte[] fontData = Starcatcher.Properties.Resources.VPCH2;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);

            try
            {
                Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                FontCollection.AddMemoryFont(fontPtr, fontData.Length);
            }
            finally
            {
                Marshal.FreeCoTaskMem(fontPtr);
            }

            FontName = FontCollection.Families[0].Name;
        }
        public static Font GetFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font(FontName, size, style, GraphicsUnit.Point);
        }
    }
}
