using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public int CurrentValue { get; private set; }
        public int? LastValue { get; private set; }
    }
}
