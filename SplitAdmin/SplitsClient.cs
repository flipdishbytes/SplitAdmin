using Newtonsoft.Json;
using SplitAdmin.Models;
using SplitAdmin.Models.Converters;

namespace SplitAdmin
{
    public class SplitsClient : EndpointClient
    {
        private readonly Cache<string, Split> _cache;

        public SplitsClient(HttpClient client) : base(client) 
        {
            _cache = Cache<string, Split>.LoadFromCache<string, Split>("splits");
        }

        public async Task<IList<Split>> GetAll(Workspace workspace, string? environmentName = null)
        {
            return await GetAll(workspace.Id, environmentName);
        }


        public async Task<IList<Split>> GetAll(string workspaceId, string? environmentName = null)
        {
            List<Split> splits = new();
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
                ValidateResponse(rawResponse);

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

            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            var rawResponse = await _client.GetAsync($"splits/ws/{workspaceId}/{splitName}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<Split>(rawContent, new UnixMillisecondTimestampConverter());

            if (value != null)
            {
                _cache[key] = value;
                _cache.Save();
            }

            return value;
        }

    }
}