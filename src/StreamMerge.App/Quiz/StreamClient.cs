using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StreamMerge.App.Quiz.DataModel;

namespace StreamMerge.App.Quiz
{
    internal interface IStreamClient
    {
        Task<NamedStreamResponse> GetNextValueAsync(string streamName);
    }

    internal class StreamClient : IStreamClient
    {
        private readonly JsonSerializer _serializer;
        private readonly bool _isHttps;
        private readonly string _host;

        public StreamClient()
        {
            _serializer = new JsonSerializer();
            _isHttps = true;
            _host = "api.pelotoncycle.com";
        }

        public async Task<NamedStreamResponse> GetNextValueAsync(string streamName)
        {
            streamName = Uri.EscapeUriString(streamName);

            var scheme = _isHttps ? "https" : "http";
            var url = string.Format("{0}://{1}/quiz/next/{2}", scheme, _host, streamName);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var responseStream = await client.GetStreamAsync(url))
                using (var streamReader = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return _serializer.Deserialize<NamedStreamResponse>(jsonTextReader);
                }
            }
        }
    }
}
