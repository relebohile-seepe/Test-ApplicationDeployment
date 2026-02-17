using System;
using System.Windows.Forms;

namespace Updater
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                MessageBox.Show("Updater must be launched by the main application.",
                    "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args[0], args[1], args[2]));
        }
    }
}
