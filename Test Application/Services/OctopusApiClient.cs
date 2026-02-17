using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Test_Application.Models;

namespace Test_Application.Services
{
    public class OctopusApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl;
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public OctopusApiClient(string serverUrl, string apiKey)
        {
            _serverUrl = serverUrl.TrimEnd('/');
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Octopus-ApiKey", apiKey);
        }

        public async Task<string> GetLatestDeployedVersionAsync(string spaceName, string projectName, string environmentName)
        {
            // 1. Resolve space
            var spaces = await GetAsync<ResourceCollection<SpaceResource>>(
                $"/api/spaces?partialName={Uri.EscapeDataString(spaceName)}&skip=0&take=1");
            var spaceId = spaces.Items.FirstOrDefault()?.Id;
            if (spaceId == null) return null;

            // 2. Resolve project
            var projects = await GetAsync<ResourceCollection<ProjectResource>>(
                $"/api/{spaceId}/projects?partialName={Uri.EscapeDataString(projectName)}&skip=0&take=1");
            var projectId = projects.Items.FirstOrDefault()?.Id;
            if (projectId == null) return null;

            // 3. Resolve environment
            var environments = await GetAsync<ResourceCollection<EnvironmentResource>>(
                $"/api/{spaceId}/environments?partialName={Uri.EscapeDataString(environmentName)}&skip=0&take=1");
            var environmentId = environments.Items.FirstOrDefault()?.Id;
            if (environmentId == null) return null;

            // 4. Get project progression (releases + deployments)
            var progression = await GetAsync<ProgressionResponse>(
                $"/api/{spaceId}/projects/{projectId}/progression");

            if (progression?.Releases == null) return null;

            // 5. Find latest release successfully deployed to this environment
            foreach (var entry in progression.Releases)
            {
                if (entry.Deployments != null &&
                    entry.Deployments.TryGetValue(environmentId, out var deployments))
                {
                    if (deployments.Any(d => string.Equals(d.State, "Success", StringComparison.OrdinalIgnoreCase)))
                    {
                        return entry.Release.Version;
                    }
                }
            }

            return null;
        }

        private async Task<T> GetAsync<T>(string relativeUrl)
        {
            var response = await _httpClient.GetStringAsync(_serverUrl + relativeUrl);
            return JsonSerializer.Deserialize<T>(response, JsonOptions);
        }
    }
}
