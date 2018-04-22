using System;
using System.Collections;

namespace DoodleClassification
{
    public static class Utilities
    {
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks);

        /// <summary>
        ///     Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        public static void Shuffle<T>(ref T[] array)
        {
            var n = ((ICollection) array).Count;
            for (var i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                var r = i + Random.Next(n - i);
                var t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
    }
}