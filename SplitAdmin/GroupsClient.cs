using SplitAdmin.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class GroupsClient : EndpointClient
    {
        private readonly Cache<string, Group> _cache;

        public GroupsClient(HttpClient client, bool useCache) : base(client, useCache)
        {
            _cache = Cache<string, Group>.LoadFromCache<string, Group>("groups");
        }

        public async Task<Group?> Get(string groupId)
        {
            if (_useCache && _cache.ContainsKey(groupId))
            {
                return _cache[groupId];
            }

            var rawResponse = await _client.GetAsync($"groups/{groupId}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Group>(rawContent);

            if (_useCache && result != null)
            {
                _cache[groupId] = result;
                _cache.Save();
            }

            return result;
        }

    }
}