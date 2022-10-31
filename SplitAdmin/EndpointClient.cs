using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace SplitAdmin
{
    public class EndpointClient
    {
        internal readonly HttpClient _client;

        public EndpointClient(HttpClient client)
        {
            _client = client;
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
            catch (Exception ex)
            {
                throw new HttpRequestException($"Request failed with code: {response.StatusCode} -> {body}", ex);
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