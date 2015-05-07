using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamMerge.App.Quiz;
using StreamMerge.App.Quiz.DataModel;
using Xunit;

namespace StreamMerge.App.Tests.Quiz
{
    public class StreamRepositoryTests
    {
        [Fact]
        public async Task GetNextAsync_MergesTwoStreams()
        {

        }

        #region Fake Data Helpers

        private IEnumerable<int> GetInfiniteSequence(int stepSize, int start = 0)
        {
            int current = start;
            while(true)
            {
                yield return current;
                current += stepSize;
            }
        }

        private class FakeStreamClient : IStreamClient
        {
            private readonly IDictionary<string, IEnumerator<int>> _streams;

            public FakeStreamClient(IDictionary<string, IEnumerator<int>> streams)
            {
                _streams = streams;
            }

            public Task<NamedStreamResponse> GetNextValueAsync(string streamName)
            {
                if(_streams.ContainsKey(streamName))
                {
                    var enumerator = _streams[streamName];
                    var previous = enumerator.Current;
                    enumerator.MoveNext();
                    var next = enumerator.Current;

                    return Task.FromResult(new NamedStreamResponse(streamName, next, previous));
                }
                else
                {
                    throw new ArgumentException("The given stream name was not setup for this test.", "streamName");
                }
            }
        }

        #endregion
    }
}
