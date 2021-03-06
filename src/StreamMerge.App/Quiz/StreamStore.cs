﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StreamMerge.App.Quiz.DataModel;
using StreamMerge.App.Util;

namespace StreamMerge.App.Quiz
{
    internal interface IStreamStore
    {
        StreamState GetStreams(IEnumerable<string> streamNames);
        void AddStreams(StreamState state);
    }

    internal class StreamStore : IStreamStore
    {
        // TODO-KPF: The quiz prompt didn't say whether concurrency should be considered.  If necessary,
        // these collections should be made safer.
        private readonly IDictionary<ISet<string>, StreamState> _store;

        public StreamStore()
        {
            var setComparer = new SetEqualityComparer<string>(StreamNameComparer.Comparer);
            _store = new Dictionary<ISet<string>, StreamState>(setComparer);
        }

        public StreamState GetStreams(IEnumerable<string> streamNames)
        {
            var nameSet = new HashSet<string>(streamNames, StreamNameComparer.Comparer);
            if(_store.ContainsKey(nameSet))
            {
                return _store[nameSet];
            }
            else
            {
                return null;
            }
        }

        public void AddStreams(StreamState state)
        {
            var nameSet = new HashSet<string>(state.StreamNames, StreamNameComparer.Comparer);
            if (_store.ContainsKey(nameSet))
            {
                throw new InvalidOperationException("Cannot add the state for the same stream collection more than once.");
            }
            else
            {
                _store.Add(nameSet, state);
            }
        }
    }
}
