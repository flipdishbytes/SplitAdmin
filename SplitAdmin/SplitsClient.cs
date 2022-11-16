using Newtonsoft.Json;
using SplitAdmin.Models;
using SplitAdmin.Models.Converters;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public async Task<Split?> Create(Workspace workspace, string splitName, TrafficType trafficType)
        {
            return await Create(workspace.Id, splitName, trafficType.Id);
        }

        public async Task<Split?> Create(string workspaceId, string splitName, TrafficType trafficType)
        {
            return await Create(workspaceId, splitName, trafficType.Id);
        }

        public async Task<Split?> Create(Workspace workspace, string splitName, string trafficTypeIdOrName)
        {
            return await Create(workspace.Id, splitName, trafficTypeIdOrName);
        }

        public async Task<Split?> Create(string workspaceId, string splitName, string trafficTypeIdOrName)
        {
            var data = new Dictionary<string, object>
            {
                {"name", splitName}
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var rawResponse = await _client.PostAsync($"splits/ws/{workspaceId}/trafficTypes/{trafficTypeIdOrName}", content);
            await ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<Split>(rawContent, new UnixMillisecondTimestampConverter());

            if (_cache != null && value != null)
            {
                _cache[$"{workspaceId}/{splitName}"] = value;
                _cache.Save();
            }

            return value;
        }

        public async Task Delete(Workspace workspace, Split split)
        {
            await Delete(workspace.Id, split.Name);
        }

        public async Task Delete(Workspace workspace, string splitName)
        {
            await Delete(workspace.Id, splitName);
        }

        public async Task Delete(string workspaceId, Split split)
        {
            await Delete(workspaceId, split.Name);
        }

        public async Task Delete(string workspaceId, string splitName)
        {
            var rawResponse = await _client.DeleteAsync($"splits/ws/{workspaceId}/{splitName}");
            await ValidateResponse(rawResponse);

            _cache?.Remove($"{workspaceId}/{splitName}");
        }

    }
}