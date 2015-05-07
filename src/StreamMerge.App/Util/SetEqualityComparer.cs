using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamMerge.App.Util
{
    internal class SetEqualityComparer<T> : IEqualityComparer<ISet<T>>
    {
        private readonly IEqualityComparer<T> _setItemComparer;

        public SetEqualityComparer()
            : this(EqualityComparer<T>.Default)
        {
        }

        public SetEqualityComparer(IEqualityComparer<T> setItemComparer)
        {
            _setItemComparer = setItemComparer;
        }

        public bool Equals(ISet<T> x, ISet<T> y)
        {
            return x.SetEquals(y);
        }

        public int GetHashCode(ISet<T> obj)
        {
            unchecked
            {
                int result = 0;
                if (obj != null)
                {
                    foreach (var i in obj)
                    {
                        result ^= 3 * _setItemComparer.GetHashCode(i);
                    }
                }
                return result;
            }
        }
    }
}
