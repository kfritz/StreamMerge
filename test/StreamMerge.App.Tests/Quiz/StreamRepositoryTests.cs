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
            const int stream1StepSize = 3;
            const int stream2StepSize = 7;
            const string stream1Name = "stream1";
            const string stream2Name = "stream2";
            var stream1 = GetInfiniteSequence(stream1StepSize);
            var stream2 = GetInfiniteSequence(stream2StepSize);
            var streams = new Dictionary<string, IEnumerator<int>>(StreamNameComparer.Comparer)
            {
                { stream1Name, stream1.GetEnumerator() },
                { stream2Name, stream2.GetEnumerator() }
            };

            var systemUnderTest = new StreamRepository(new FakeStreamClient(streams), new StreamStore());

            const int numToCheck = 50;
            int last = int.MinValue;
            for (int index = 0; index < numToCheck; index++)
            {
                var next = await systemUnderTest.GetNextAsync(stream1Name, stream2Name);
                Assert.True(next.CurrentValue >= last, "The next value from the stream should be greater than or equal to the previous one.");
                Assert.True(
                    next.CurrentValue % stream1StepSize == 0 || next.CurrentValue % stream2StepSize == 0, 
                    "The values from the merged stream should come from either of the source streams.");
                if (last != int.MinValue)
                {
                    Assert.True(last == next.LastValue, "The returned last value should match the previous invocation's current value.");
                }
                last = next.CurrentValue;
            }
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
