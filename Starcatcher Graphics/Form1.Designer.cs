namespace Starcatcher
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.points_label = new System.Windows.Forms.Label();
            this.Deaths_label = new System.Windows.Forms.Label();
            this.recover_display_label = new System.Windows.Forms.Label();
            this.MenuButton = new System.Windows.Forms.Button();
            this.ShopButton = new System.Windows.Forms.Button();
            this.PausedLabel = new System.Windows.Forms.Label();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // points_label
            // 
            this.points_label.AutoSize = true;
            this.points_label.Font = new System.Drawing.Font("VPCH2", 35F);
            this.points_label.ForeColor = System.Drawing.Color.MistyRose;
            this.points_label.Location = new System.Drawing.Point(359, 699);
            this.points_label.Name = "points_label";
            this.points_label.Size = new System.Drawing.Size(50, 59);
            this.points_label.TabIndex = 0;
            this.points_label.Text = "0";
            this.points_label.Visible = false;
            // 
            // Deaths_label
            // 
            this.Deaths_label.AutoSize = true;
            this.Deaths_label.Font = new System.Drawing.Font("VPCH2", 15F);
            this.Deaths_label.ForeColor = System.Drawing.Color.Red;
            this.Deaths_label.Location = new System.Drawing.Point(718, 9);
            this.Deaths_label.Name = "Deaths_label";
            this.Deaths_label.Size = new System.Drawing.Size(23, 25);
            this.Deaths_label.TabIndex = 1;
            this.Deaths_label.Text = "0";
            this.Deaths_label.Visible = false;
            // 
            // recover_display_label
            // 
            this.recover_display_label.AutoSize = true;
            this.recover_display_label.Font = new System.Drawing.Font("VPCH2", 45F);
            this.recover_display_label.ForeColor = System.Drawing.Color.Gold;
            this.recover_display_label.Location = new System.Drawing.Point(356, 318);
            this.recover_display_label.Name = "recover_display_label";
            this.recover_display_label.Size = new System.Drawing.Size(64, 75);
            this.recover_display_label.TabIndex = 2;
            this.recover_display_label.Text = "5";
            this.recover_display_label.Visible = false;
            // 
            // MenuButton
            // 
            this.MenuButton.BackColor = System.Drawing.Color.Black;
            this.MenuButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MenuButton.Font = new System.Drawing.Font("VPCH2", 20F);
            this.MenuButton.ForeColor = System.Drawing.Color.Red;
            this.MenuButton.Location = new System.Drawing.Point(300, 490);
            this.MenuButton.Name = "MenuButton";
            this.MenuButton.Size = new System.Drawing.Size(171, 48);
            this.MenuButton.TabIndex = 3;
            this.MenuButton.Text = "Menu";
            this.MenuButton.UseVisualStyleBackColor = false;
            this.MenuButton.Visible = false;
            this.MenuButton.Click += new System.EventHandler(this.MenuButton_Click);
            // 
            // ShopButton
            // 
            this.ShopButton.BackColor = System.Drawing.Color.Black;
            this.ShopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShopButton.Font = new System.Drawing.Font("VPCH2", 20F);
            this.ShopButton.ForeColor = System.Drawing.Color.LawnGreen;
            this.ShopButton.Location = new System.Drawing.Point(300, 396);
            this.ShopButton.Name = "ShopButton";
            this.ShopButton.Size = new System.Drawing.Size(171, 48);
            this.ShopButton.TabIndex = 4;
            this.ShopButton.Text = "Shop";
            this.ShopButton.UseVisualStyleBackColor = false;
            this.ShopButton.Visible = false;
            this.ShopButton.Click += new System.EventHandler(this.ShopButton_Click);
            // 
            // PausedLabel
            // 
            this.PausedLabel.AutoSize = true;
            this.PausedLabel.Font = new System.Drawing.Font("VPCH2", 30F);
            this.PausedLabel.ForeColor = System.Drawing.Color.Tomato;
            this.PausedLabel.Location = new System.Drawing.Point(323, 208);
            this.PausedLabel.Name = "PausedLabel";
            this.PausedLabel.Size = new System.Drawing.Size(148, 50);
            this.PausedLabel.TabIndex = 6;
            this.PausedLabel.Text = "Paused";
            this.PausedLabel.Visible = false;
            // 
            // ContinueButton
            // 
            this.ContinueButton.BackColor = System.Drawing.Color.Black;
            this.ContinueButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContinueButton.Font = new System.Drawing.Font("VPCH2", 20F);
            this.ContinueButton.ForeColor = System.Drawing.Color.Orange;
            this.ContinueButton.Location = new System.Drawing.Point(296, 302);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(175, 50);
            this.ContinueButton.TabIndex = 7;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = false;
            this.ContinueButton.Visible = false;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(752, 713);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.PausedLabel);
            this.Controls.Add(this.ShopButton);
            this.Controls.Add(this.MenuButton);
            this.Controls.Add(this.recover_display_label);
            this.Controls.Add(this.Deaths_label);
            this.Controls.Add(this.points_label);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("VPCH2", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = " Starcatcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label points_label;
        private System.Windows.Forms.Label Deaths_label;
        private System.Windows.Forms.Label recover_display_label;
        private System.Windows.Forms.Button MenuButton;
        private System.Windows.Forms.Button ShopButton;
        private System.Windows.Forms.Label PausedLabel;
        private System.Windows.Forms.Button ContinueButton;
    }
}

