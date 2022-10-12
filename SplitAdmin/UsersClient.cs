using Newtonsoft.Json;
using SplitAdmin.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SplitAdmin
{
    public class UsersClient : EndpointClient
    {
        private readonly Cache<string, User> _cache;

        public UsersClient(HttpClient client) : base(client)
        {
            _cache = Cache<string, User>.LoadFromCache<string, User>("users");
        }

        public async Task<User?> Get(string userId)
        {
            if (_cache.ContainsKey(userId))
            {
                return _cache[userId];
            }

            var rawResponse = await _client.GetAsync($"users/{userId}");
            ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<User>(rawContent);

            if (result != null)
            {
                _cache[userId] = result;
                _cache.Save();
            }

            return result;
        }

    }
}