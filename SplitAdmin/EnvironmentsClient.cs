using Newtonsoft.Json;
using SplitAdmin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Environment = SplitAdmin.Models.Environment;

namespace SplitAdmin
{
    public class EnvironmentsClient : EndpointClient
    {
        public EnvironmentsClient(HttpClient client) : base(client) { }

        public async Task<IList<Environment>> Get(Workspace workspace)
        {
            return await Get(workspace.Id);
        }

        public async Task<IList<Environment>> Get(string workspaceId)
        {
            var rawResponse = await _client.GetAsync($"environments/ws/{workspaceId}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<List<Environment>>(rawContent);

            if (value == null)
            {
                throw new SplitAdminResponseException("Failed to decode response from API");
            }

            return value;
        }

    }
}