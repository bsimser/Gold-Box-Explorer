using System;
using System.Windows.Forms;
using GoldBoxExplorer.Lib.Exceptions;

namespace GoldBoxExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UnhandledExceptionManager.AddHandler();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
