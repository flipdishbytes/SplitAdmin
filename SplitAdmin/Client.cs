using System;
using System.Net.Http;

namespace SplitAdmin
{
    public class SplitClient
    {
        private readonly HttpClient _client;
        private readonly WorkspacesClient _workspacesClient;
        private readonly EnvironmentsClient _environmentsClient;
        private readonly SplitsClient _splitsClient;
        private readonly TrafficTypesClient _trafficTypesClient;
        private readonly UsersClient _usersClient;
        private readonly GroupsClient _groupsClient;
        private readonly AttributesClient _attributesClient;
        private readonly SegmentsClient _segmentsClient;

        public WorkspacesClient Workspaces { get { return _workspacesClient; } }
        public EnvironmentsClient Environments { get { return _environmentsClient; } }
        public SplitsClient Splits { get { return _splitsClient; } }
        public TrafficTypesClient TrafficTypes { get { return _trafficTypesClient; } }
        public UsersClient Users { get { return _usersClient; } }
        public GroupsClient Groups { get { return _groupsClient; } }
        public AttributesClient Attributes { get { return _attributesClient; } }
        public SegmentsClient Segments { get { return _segmentsClient; } }

        public SplitClient(string key)
        {
            _client = new HttpClient(new HttpRetryHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri("https://api.split.io/internal/api/v2/")
            };
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            _workspacesClient = new WorkspacesClient(_client);
            _environmentsClient = new EnvironmentsClient(_client);
            _splitsClient = new SplitsClient(_client);
            _trafficTypesClient = new TrafficTypesClient(_client);
            _usersClient = new UsersClient(_client);
            _groupsClient = new GroupsClient(_client);
            _attributesClient = new AttributesClient(_client);
            _segmentsClient = new SegmentsClient(_client);
        }

    }
}