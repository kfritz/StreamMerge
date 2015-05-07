using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StreamMerge.App.Quiz.DataModel
{
    public class NamedStreamResponse : StreamResponse
    {
        private NamedStreamResponse()
            : base()
        { }

        public NamedStreamResponse(string streamName, int currentValue, int lastValue)
            : base(currentValue, lastValue)
        {
            StreamName = streamName;
        }

        [JsonProperty("stream")]
        public string StreamName { get; private set; }
    }
}
