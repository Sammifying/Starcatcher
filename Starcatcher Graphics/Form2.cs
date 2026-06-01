using Starcatcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    public partial class Form2 : Form
    {

        int points2 = 0;
        int StarsUpgradeLevel;
        int RarityUpgradeLevel;
        int UniverseUnlocked = 0;

        public List<int> DataToReturn;
        public Form2(List<int>SentData)
        {

            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;

            points2 = SentData[0];
            StarsUpgradeLevel = SentData[1];
            RarityUpgradeLevel = SentData[2];
            UniverseUnlocked = SentData[3];

            if (UniverseUnlocked == 1)
            {
                UniverseUnlockButton.Text = "UNLOCKED";
            }

            // Set Font to all Components

            button1.Font = CustomFont.GetFont(13, FontStyle.Regular);
            StarsUpgradeButton.Font = CustomFont.GetFont(25, FontStyle.Regular);
            RarityUpgradeButton.Font = CustomFont.GetFont(25, FontStyle.Regular);
            UniverseUnlockButton.Font = CustomFont.GetFont(25, FontStyle.Regular);

        }

        Font VPCH2_Font40;
        Font VPCH2_Font20;
        Font VPCH2_Font14;

        SolidBrush OrangeBrush = new SolidBrush(Color.DarkOrange);
        SolidBrush FontBrush = new SolidBrush(Color.MistyRose);
        SolidBrush SlateBlue = new SolidBrush(Color.FromArgb(0, 213, 255));

        Rectangle StarsLabel = new Rectangle(700, 21, 89, 34);
        Rectangle PointsLabel2 = new Rectangle(770, 9, 200, 67);
        Rectangle StarsStateLabel = new Rectangle(149, 340, 39, 24);
        Rectangle RarityStateLabel = new Rectangle(479, 340, 39, 24);
        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            VPCH2_Font40 = CustomFont.GetFont(40, FontStyle.Regular);
            VPCH2_Font20 = CustomFont.GetFont(20, FontStyle.Regular);
            VPCH2_Font14 = CustomFont.GetFont(14, FontStyle.Regular);

            Graphics g = e.Graphics;
            g.DrawString("Stars:", VPCH2_Font20, FontBrush, StarsLabel);
            g.DrawString($"{points2}", VPCH2_Font40, FontBrush, PointsLabel2);

            g.DrawString($"{StarsUpgradeLevel}/5", VPCH2_Font14, OrangeBrush, StarsStateLabel);
            g.DrawString($"{RarityUpgradeLevel}/5", VPCH2_Font14, SlateBlue, RarityStateLabel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataToReturn = new List<int>() { points2, StarsUpgradeLevel, RarityUpgradeLevel, UniverseUnlocked };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        Messagebox messagebox;
        private void StarsUpgradeButton_Click(object sender, EventArgs e)
        {
            if (points2 >= 0)
            {
                switch (StarsUpgradeLevel)
                {
                    case 1:
                        if (points2 >= 50)
                        {
                            points2 -= 50;
                            StarsUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(50 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                    case 2:
                        if (points2 >= 150)
                        {
                            points2 -= 150;
                            StarsUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(150 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                    case 3:
                        if (points2 >= 500)
                        {
                            points2 -= 500;
                            StarsUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(500 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                    case 4:
                        if (points2 >= 1000)
                        {
                            points2 -= 1000;
                            StarsUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(1000 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                }
            }
        }

        private void RarityUpgradeButton_Click(object sender, EventArgs e)
        {
            if (points2 >= 0)
            {
                switch (RarityUpgradeLevel)
                {
                    case 1:
                        if (points2 >= 100)
                        {
                            points2 -= 100;
                            RarityUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(100 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                    case 2:
                        if (points2 >= 250)
                        {
                            points2 -= 250;
                            RarityUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(250 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                    case 3:
                        if (points2 >= 600)
                        {
                            points2 -= 600;
                            RarityUpgradeLevel++;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(600 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                    case 4:
                        if (points2 >= 1250)
                        {
                            points2 -= 1250;
                            RarityUpgradeLevel += 1;

                            this.Invalidate();
                        }
                        else
                        {
                            messagebox = new Messagebox(1250 - points2);
                            messagebox.ShowDialog();
                        }
                        break;

                }
            }
        }

        private void UniverseUnlockButton_Click(object sender, EventArgs e)
        {
            if (points2 < 10000 && UniverseUnlockButton.Text == "UNLOCK")
            {
                messagebox = new Messagebox(10000 - points2);
                messagebox.ShowDialog();
            }
            else
            {
                UniverseUnlocked = 1;

                Endscreen endscreen = new Endscreen();
                if (UniverseUnlockButton.Text == "UNLOCK")
                    points2 -= 10000;

                UniverseUnlockButton.Text = "UNLOCKED";
                this.Invalidate();

                endscreen.Show();
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DataToReturn = new List<int>() { points2, StarsUpgradeLevel, RarityUpgradeLevel, UniverseUnlocked };
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ApplyCustomFont(this);
        }

        private void ApplyCustomFont(Control container)
        {
            container.Font = CustomFont.GetFont(12, FontStyle.Regular);
        }
    }

}
