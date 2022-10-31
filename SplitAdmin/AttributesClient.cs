using Newtonsoft.Json;
using SplitAdmin.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class AttributesClient : EndpointClient
    {
        public AttributesClient(HttpClient client) : base(client) { }

        public async Task<IList<TrafficType>> Get(Workspace workspace, TrafficType trafficType)
        {
            return await Get(workspace.Id, trafficType.Id);
        }

        public async Task<IList<TrafficType>> Get(string workspaceId, TrafficType trafficType)
        {
            return await Get(workspaceId, trafficType.Id);
        }

        public async Task<IList<TrafficType>> Get(Workspace workspace, string trafficTypeId)
        {
            return await Get(workspace.Id, trafficTypeId);
        }

        public async Task<IList<TrafficType>> Get(string workspaceId, string trafficTypeId)
        {
            var rawResponse = await _client.GetAsync($"schema/ws/{workspaceId}/trafficTypes/{trafficTypeId}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<List<TrafficType>>(rawContent);

            if (value == null)
            {
                throw new InvalidDataException("Failed to decode response from API");
            }

            return value;
        }

    }
}