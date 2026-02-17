using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Test_Application.Services
{
    public class UpdateChecker
    {
        private const string OctopusServerUrl = "https://testapplication.octopus.app";
        private const string SpaceName = "Default";
        private const string ProjectName = "Test Application";
        private const string EnvironmentName = "dev";

        public async Task<(bool UpdateAvailable, string LatestVersion)> CheckForUpdateAsync()
        {
            var apiKey = Environment.GetEnvironmentVariable("OCTOPUS_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
                return (false, null);

            var client = new OctopusApiClient(OctopusServerUrl, apiKey);
            var latestVersion = await client.GetLatestDeployedVersionAsync(SpaceName, ProjectName, EnvironmentName);

            if (string.IsNullOrEmpty(latestVersion))
                return (false, null);

            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var remoteVersion = ParseVersion(latestVersion);

            if (remoteVersion > currentVersion)
                return (true, latestVersion);

            return (false, null);
        }

        private static Version ParseVersion(string versionString)
        {
            // Handle versions like "1.0.0" or "1.0.0.0"
            if (Version.TryParse(versionString, out var version))
                return version;
            return new Version(0, 0, 0);
        }
    }
}
