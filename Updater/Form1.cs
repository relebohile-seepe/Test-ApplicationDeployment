using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class Form1 : Form
    {
        private const string OctopusServerUrl = "https://testapplication.octopus.app";
        private const string SpaceId = "Spaces-1";
        private const string PackageId = "TestApplication";

        private readonly string _appDir;
        private readonly string _newVersion;
        private readonly string _apiKey;

        public Form1(string appDir, string newVersion, string apiKey)
        {
            _appDir = appDir;
            _newVersion = newVersion;
            _apiKey = apiKey;
            InitializeComponent();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Waiting for application to close...";
                await Task.Delay(2000);

                lblStatus.Text = "Downloading update...";
                var packageBytes = await DownloadPackageAsync();

                lblStatus.Text = "Removing old version...";
                DeleteOldFiles(_appDir);

                lblStatus.Text = "Installing update...";
                ExtractPackage(packageBytes, _appDir);

                lblStatus.Text = "Launching application...";
                await Task.Delay(500);

                var exePath = FindExecutable(_appDir);
                if (exePath != null)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = exePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Update installed but could not find the application executable.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}",
                    "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Application.Exit();
            }
        }

        private async Task<byte[]> DownloadPackageAsync()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Octopus-ApiKey", _apiKey);

            var url = $"{OctopusServerUrl}/api/{SpaceId}/packages/packages-{PackageId}.{_newVersion}/raw";
            return await httpClient.GetByteArrayAsync(url);
        }

        private void DeleteOldFiles(string directory)
        {
            var dirInfo = new DirectoryInfo(directory);

            foreach (var file in dirInfo.GetFiles())
            {
                try { file.Delete(); }
                catch { /* skip locked files */ }
            }

            foreach (var dir in dirInfo.GetDirectories())
            {
                if (dir.Name.Equals("Updater", StringComparison.OrdinalIgnoreCase))
                    continue;

                try { dir.Delete(true); }
                catch { /* skip locked directories */ }
            }
        }

        private void ExtractPackage(byte[] packageBytes, string targetDir)
        {
            var tempFile = Path.Combine(Path.GetTempPath(), $"update_{_newVersion}.zip");
            try
            {
                File.WriteAllBytes(tempFile, packageBytes);
                ZipFile.ExtractToDirectory(tempFile, targetDir);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        private string FindExecutable(string directory)
        {
            var exePath = Path.Combine(directory, "Test Application.exe");
            if (File.Exists(exePath))
                return exePath;

            // Fallback: find any .exe that isn't the Updater
            var exeFiles = Directory.GetFiles(directory, "*.exe")
                .Where(f => !Path.GetFileName(f).Equals("Updater.exe", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            return exeFiles.FirstOrDefault();
        }
    }
}
