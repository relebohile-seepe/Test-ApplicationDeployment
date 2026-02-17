using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Test_Application.Services;

namespace Test_Application
{
    public partial class Form1 : Form
    {
        private string _latestVersion;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var checker = new UpdateChecker();
                var (updateAvailable, latestVersion) = await checker.CheckForUpdateAsync();

                if (updateAvailable)
                {
                    _latestVersion = latestVersion;
                    button1.Text = $"Update to {latestVersion}";
                    button1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update check failed: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var appDir = AppDomain.CurrentDomain.BaseDirectory;
                var updaterPath = Path.Combine(appDir, "..", "Updater", "Updater.exe");
                updaterPath = Path.GetFullPath(updaterPath);

                if (!File.Exists(updaterPath))
                {
                    MessageBox.Show("Updater not found. Please reinstall the application.",
                        "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var apiKey = Environment.GetEnvironmentVariable("OCTOPUS_API_KEY");

                Process.Start(new ProcessStartInfo
                {
                    FileName = updaterPath,
                    Arguments = $"\"{appDir.TrimEnd('\\')}\" \"{_latestVersion}\" \"{apiKey}\"",
                    UseShellExecute = true
                });

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch updater: {ex.Message}",
                    "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
