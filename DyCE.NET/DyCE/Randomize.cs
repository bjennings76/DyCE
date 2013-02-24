using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DyCE
{
    public static class Randomize
    {
        private static bool IsPrime(int candidate)
        {
            // Test whether the parameter is a prime number.
            if ((candidate & 1) == 0)
                return candidate == 2;

            // Note:
            // ... This version was changed to test the square.
            // ... Original version tested against the square root.
            // ... Also we exclude 1 at the very end.
            for (int i = 3; (i*i) <= candidate; i += 2)
                if ((candidate%i) == 0)
                    return false;

            return candidate != 1;
        }

        private static readonly int[] _primes = new[]
                                                {
                                                    3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131,
                                                    163, 197, 239, 293, 353, 431, 521, 631, 761, 919, 1103, 
                                                    1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 
                                                    7013, 8419, 10103, 12143, 14591, 17519, 21023, 25229, 
                                                    30293, 36353, 43627, 52361, 62851, 75431, 90523, 
                                                    108631, 130363, 156437, 187751, 225307, 270371, 324449, 
                                                    389357, 467237, 560689, 672827, 807403, 968897, 
                                                    1162687, 1395263, 1674319, 2009191, 2411033, 2893249, 
                                                    3471899, 4166287, 4999559, 5999471, 7199369
                                                };

        public static int GetRandomPrime(int count, Random r = null)
        {
            if (r == null) r = new Random();

            int[] biggerPrimes = _primes.Where(num => num > count).ToArray();
            return biggerPrimes[r.Next(biggerPrimes.Count())];
        }

        //public static int GetPrime(int count)
        //{
        //    // Get the first hashtable prime number that is equal to or greater than the parameter.
        //    int prime = _primes.FirstOrDefault(n => n > count);

        //    if (prime != 0) return prime;

        //    for (int j = count + 1 | 1; j < 2147483647; j += 2)
        //        if (IsPrime(j))
        //            return j;

        //    return count;
        //}


        public static void Shuffle<T>(this IList<T> list, Random r = null)
        {
            if (r == null) r = new Random();

            //list = list.OrderBy(i => r.Next()).ToList();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static int GetNextIndex(int lastIndex, int itemCount, int cyclePrime)
        {
            if (itemCount < 1)
                return -1;

            if (itemCount == 1)
                return 0;

            int newIndex = lastIndex + cyclePrime;

            while (newIndex >= itemCount) 
                newIndex = newIndex - itemCount;

            int testIndex = (lastIndex + cyclePrime)%itemCount;

            Debug.WriteLine("newIndex via remainder:  " + testIndex);
            Debug.WriteLine("newIndex via while loop: " + newIndex);

            return newIndex;
        }
    }
}
