using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace SplitAdmin
{
    public class EndpointClient
    {
        internal readonly HttpClient _client;
        internal readonly bool _useCache;

        public EndpointClient(HttpClient client, bool useCache)
        {
            _client = client;
            _useCache = useCache;
        }

        internal static async void ValidateResponse(HttpResponseMessage response)
        {
            if ((int)response.StatusCode < 400)
            {
                return;
            }

            var body = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? data;

            try
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            }
            catch
            {
                throw new HttpRequestException($"Request failed with code: {response.StatusCode} -> {body}");
            }

            if (data == null)
            {
                throw new HttpRequestException($"Request failed with code: {response.StatusCode} -> {body}");
            }

            throw new HttpRequestException(
                $"Request failed with code: {response.StatusCode}\nMessage: {data["message"]}\nDetails: {data["details"]}"
            );
        }
    }
}