using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamMerge.App.Quiz.DataModel
{
    internal class StreamState
    {
        private readonly IDictionary<string, int> _currentValues;

        public StreamState(IDictionary<string, int> initialValues, int lastValue)
        {
            _currentValues = initialValues;
            LastValue = lastValue;
        }

        public int LastValue { get; private set; }

        public int this[string streamName]
        {
            get
            {
                if(_currentValues.ContainsKey(streamName))
                {
                    return _currentValues[streamName];
                }
                else
                {
                    throw new ArgumentException(
                        string.Format("This state object is not configured for the stream named {0}.", streamName),
                        "streamName");
                }
            }
        }

        public IEnumerable<string> StreamNames
        {
            get
            {
                return _currentValues.Keys;
            }
        }

        public void SetStreamValue(string streamName, int newValue)
        {
            LastValue = this[streamName];
            _currentValues[streamName] = newValue;
        }
    }
}
