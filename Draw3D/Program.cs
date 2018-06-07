using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Draw3D
{
    static class Program
    {
        public static FormGraph formMain;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formMain = new FormGraph();
            Application.Run(formMain);
        }
    }
}
