using Newtonsoft.Json;
using SplitAdmin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class WorkspacesClient : EndpointClient
    {        
        public WorkspacesClient(HttpClient client) : base(client) { }

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