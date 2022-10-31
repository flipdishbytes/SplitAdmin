using Newtonsoft.Json;
using SplitAdmin.Models;
using SplitAdmin.Models.Converters;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class SplitsClient : EndpointClient
    {
        private readonly Cache<string, Split>? _cache;

        public SplitsClient(HttpClient client, bool useCache) : base(client)
        {
            if (useCache)
            {
                _cache = Cache<string, Split>.LoadFromCache<string, Split>("splits");
            }
        }

        public async Task<IList<Split>> GetAll(Workspace workspace, string? environmentName = null)
        {
            return await GetAll(workspace.Id, environmentName);
        }


        public async Task<IList<Split>> GetAll(string workspaceId, string? environmentName = null)
        {
            var splits = new List<Split>();
            int offset = 0;


            while (true)
            {
                // 50 is the max we can request in one go.
                var url = $"splits/ws/{workspaceId}";

                if (environmentName != null)
                {
                    url += $"/environments/{environmentName}";
                }

                url += $"?offset={offset}&limit=50";

                var rawResponse = await _client.GetAsync(url);
                await ValidateResponse(rawResponse);

                var rawContent = await rawResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<MultipleResultsResponse<Split>>(
                    rawContent,
                    new UnixMillisecondTimestampConverter()
                );

                if (response == null)
                {
                    throw new SplitAdminResponseException("Failed to deserialize split response");
                }

                splits.AddRange(response.Objects);
                if (splits.Count < response.TotalCount)
                {
                    offset += response.Objects.Count;
                }
                else
                {
                    break;
                }
            }

            return splits;
        }

        public async Task<Split?> Get(Workspace workspace, string splitName)
        {
            return await Get(workspace.Id, splitName);
        }

        public async Task<Split?> Get(string workspaceId, string splitName)
        {
            var key = $"{workspaceId}/{splitName}";

            if (_cache?.ContainsKey(key) ?? false)
            {
                return _cache[key];
            }

            var rawResponse = await _client.GetAsync($"splits/ws/{workspaceId}/{splitName}");
            await ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<Split>(rawContent, new UnixMillisecondTimestampConverter());

            if (_cache != null && value != null)
            {
                _cache[key] = value;
                _cache.Save();
            }

            return value;
        }

    }
}