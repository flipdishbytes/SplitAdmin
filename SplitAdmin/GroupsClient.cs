using Newtonsoft.Json;
using SplitAdmin.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class GroupsClient : EndpointClient
    {
        private readonly Cache<string, Group>? _cache;

        public GroupsClient(HttpClient client, bool useCache) : base(client)
        {
            if (useCache)
            {
                _cache = Cache<string, Group>.LoadFromCache<string, Group>("groups");
            }
        }

        public async Task<Group?> Get(string groupId)
        {
            if (_cache?.ContainsKey(groupId) ?? false)
            {
                return _cache[groupId];
            }

            var rawResponse = await _client.GetAsync($"groups/{groupId}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Group>(rawContent);

            if (_cache != null && result != null)
            {
                _cache[groupId] = result;
                _cache.Save();
            }

            return result;
        }

    }
}