using Newtonsoft.Json;
using SplitAdmin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class WorkspacesClient : EndpointClient
    {
        public WorkspacesClient(HttpClient client, bool useCache) : base(client, useCache) { }

        public async Task<IList<Workspace>> GetAll()
        {
            var workspaces = new List<Workspace>();
            int offset = 0;

            while (true)
            {
                var rawResponse = await _client.GetAsync($"workspaces?offset={offset}");
                ValidateResponse(rawResponse);

                var rawContent = await rawResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<MultipleResultsResponse<Workspace>>(rawContent);

                if (response == null)
                {
                    throw new SplitAdminResponseException("Failed to decode workspace response");
                }

                if (response.Objects.Count == 0)
                {
                    throw new SplitAdminResponseException($"Unexpected zero length collection from workspaces: {rawContent}");
                }

                workspaces.AddRange(response.Objects);

                if (workspaces.Count < response.TotalCount)
                {
                    offset += response.Objects.Count;
                }
                else
                {
                    break;
                }
            }

            return workspaces;
        }

    }
}