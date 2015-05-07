using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamMerge.App.Quiz.DataModel;

namespace StreamMerge.App.Quiz
{
    public interface IStreamRepository
    {
        Task<StreamResponse> GetNextAsync(params string[] streamNames);
    }

    internal class StreamRepository : IStreamRepository
    {
        private readonly IStreamClient _client;
        private readonly IStreamStore _store;

        public StreamRepository(IStreamClient client, IStreamStore store)
        {
            _client = client;
            _store = store;
        }

        public async Task<StreamResponse> GetNextAsync(params string[] streamNames)
        {
            // TODO-KPF: Assert collection not empty

            // Get all the current values of each stream
            var state = _store.GetStreams(streamNames);
            
            if(state == null)
            {
                // If the given collection of streams has never been requested, pull all of their first values
                state = await InitializeStreamReaderAsync(streamNames);
                _store.AddStreams(state);
            }

            // Determine which stream has the minimum value
            string minStream = null;
            int? minValue = null;
            foreach(var s in streamNames)
            {
                var currentStreamValue = state[s];

                if(minValue == null || minValue > currentStreamValue)
                {
                    minValue = currentStreamValue;
                    minStream = s;
                }
            }
            
            // Prepare the response
            var result = new StreamResponse(minValue.Value, state.LastValue);

            // Fetch the next value in the minimum stream for next time
            var nextValue = await _client.GetNextValueAsync(minStream);
            state.SetStreamValue(minStream, nextValue.CurrentValue);

            return result;
        }

        private async Task<StreamState> InitializeStreamReaderAsync(IEnumerable<string> streamNames)
        {
            var values = new Dictionary<string, int>();
            foreach(var s in streamNames)
            {
                var nextStreamValue = await _client.GetNextValueAsync(s);
                values.Add(s, nextStreamValue.CurrentValue);
            }
            return new StreamState(values);
        }
    }
}
