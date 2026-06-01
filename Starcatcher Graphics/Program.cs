using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starcatcher
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<int> Starter_Values = new List<int>() {0, 1, 1, 0, 0};
            CustomFont.Initialize();

            Application.Run(new MainMenu(Starter_Values));
        }
    }
}
