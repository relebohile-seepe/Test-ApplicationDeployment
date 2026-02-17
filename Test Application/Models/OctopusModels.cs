using System.Collections.Generic;

namespace Test_Application.Models
{
    public class ResourceCollection<T>
    {
        public List<T> Items { get; set; }
    }

    public class SpaceResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ProjectResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class EnvironmentResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ProgressionResponse
    {
        public List<ProgressionRelease> Releases { get; set; }
    }

    public class ProgressionRelease
    {
        public ReleaseResource Release { get; set; }
        public Dictionary<string, List<DeploymentResource>> Deployments { get; set; }
    }

    public class ReleaseResource
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }

    public class DeploymentResource
    {
        public string State { get; set; }
    }
}
