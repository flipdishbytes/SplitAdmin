using Newtonsoft.Json;
using SplitAdmin.Models;
using SplitAdmin.Models.Converters;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Environment = SplitAdmin.Models.Environment;

namespace SplitAdmin
{
    public class SegmentsClient : EndpointClient
    {
        public SegmentsClient(HttpClient client) : base(client) { }

        public async Task<IList<Segment>> GetAll(Workspace workspace, string? environmentName = null)
        {
            return await GetAll(workspace.Id, environmentName);
        }


        public async Task<IList<Segment>> GetAll(string workspaceId, string? environmentName = null)
        {
            var segments = new List<Segment>();
            int offset = 0;

            while (true)
            {
                // 50 is the max we can request in one go.
                var url = $"segments/ws/{workspaceId}";

                if (environmentName != null)
                {
                    url += $"/environments/{environmentName}";
                }

                url += $"?offset={offset}&limit=50";

                var rawResponse = await _client.GetAsync(url);
                await ValidateResponse(rawResponse);

                var rawContent = await rawResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<MultipleResultsResponse<Segment>>(
                    rawContent,
                    new UnixMillisecondTimestampConverter()
                );

                if (response == null)
                {
                    throw new SplitAdminResponseException("Failed to decode segment data response");
                }

                segments.AddRange(response.Objects);
                if (segments.Count < response.TotalCount)
                {
                    offset += response.Objects.Count;
                }
                else
                {
                    break;
                }
            }

            return segments;
        }

        public async Task<List<string>> GetKeys(Environment environment, Segment segment)
        {
            return await GetKeys(environment.Id, segment.Name);
        }

        public async Task<List<string>> GetKeys(Environment environment, string segmentName)
        {
            return await GetKeys(environment.Id, segmentName);
        }

        public async Task<List<string>> GetKeys(string environmentId, Segment segment)
        {
            return await GetKeys(environmentId, segment.Name);
        }

        public async Task<List<string>> GetKeys(string environmentId, string segmentName)
        {
            var rawResponse = await _client.GetAsync($"segments/{environmentId}/{segmentName}/keys");
            await ValidateResponse(rawResponse);

            var rawContent = await rawResponse.Content.ReadAsStringAsync();

            var value = JsonConvert.DeserializeObject<List<string>>(rawContent, new UnixMillisecondTimestampConverter());

            if (value == null)
            {
                throw new SplitAdminResponseException("Failed to decode response from API");
            }

            return value;
        }

    }
}