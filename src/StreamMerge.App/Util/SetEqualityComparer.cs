using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamMerge.App.Util
{
    /// <summary>
    /// An implementation of <see cref="IEqualityComparer{ISet{T}}"/> that treats instances of 
    /// <see cref="ISet{T}"/> as equal if subtracting the intersection of two sets from the union 
    /// of the two sets produces the empty set.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sets.</typeparam>
    internal class SetEqualityComparer<T> : IEqualityComparer<ISet<T>>
    {
        private readonly IEqualityComparer<T> _setItemComparer;

        /// <summary>
        /// Initializes the set comparer with the default equality comparer for the set element 
        /// type.
        /// </summary>
        /// <remarks>
        /// Do not use this constructor if the sets to compare are using custom equality comparers.
        /// </remarks>
        public SetEqualityComparer()
            : this(EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes the set comparer with the given equality comparer of the set element type.
        /// </summary>
        /// <remarks>
        /// The given comparer should be the same comparer used by the sets themselves.
        /// </remarks>
        /// <param name="setItemComparer">The equality comparer to use.</param>
        public SetEqualityComparer(IEqualityComparer<T> setItemComparer)
        {
            _setItemComparer = setItemComparer;
        }

        /// <summary>
        /// Determines whether the specified sets are equal.
        /// </summary>
        /// <param name="x">The first set to compare.</param>
        /// <param name="y">The second set to compare.</param>
        /// <returns>True if the specified sets are equal; false otherwise.</returns>
        public bool Equals(ISet<T> x, ISet<T> y)
        {
            return x.SetEquals(y);
        }

        /// <summary>
        /// Returns a hash code for the specified set.
        /// </summary>
        /// <param name="obj">The set for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified set.</returns>
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
