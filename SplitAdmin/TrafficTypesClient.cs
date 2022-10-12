using Newtonsoft.Json;
using SplitAdmin.Models;

namespace SplitAdmin
{
    public class TrafficTypesClient : EndpointClient
    {        
        public TrafficTypesClient(HttpClient client) : base(client) { }

        public async Task<IList<TrafficType>> Get(Workspace workspace)
        {
            return await Get(workspace.Id);
        }

        public async Task<IList<TrafficType>> Get(string workspaceId)
        {
            var rawResponse = await _client.GetAsync($"trafficTypes/ws/{workspaceId}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<List<TrafficType>>(rawContent);

            if (value == null)
            {
                throw new SplitAdminResponseException("Failed to decode response from API");
            }

            return value;
        }

    }
}