using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StreamMerge.App.Quiz.DataModel
{
    public class StreamResponse
    {
        protected StreamResponse()
        { }

        public StreamResponse(int currentValue, int? lastValue)
            : this()
        {
            CurrentValue = currentValue;
            LastValue = lastValue;
        }

        [JsonProperty("current")]
        public int CurrentValue { get; private set; }

        [JsonProperty("last")]
        public int? LastValue { get; private set; }
    }
}
