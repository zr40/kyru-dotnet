using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Kyru.Utilities
{
	internal static class Primes
	{
		/// <summary>
		/// Some prime numbers. Actually I have no idea whether this list is big enough.
		/// Estimate: you need all primes below 300
		/// </summary>
		static BigInteger[] ar = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101,
		                          103,107,109,113,127,131,137,139,149,151,157,163,167,173,179,181,191,193,197,
		                          199,211,223,227,229,233,239,241,251,257,263,269,271,277,281,283,293,307,311,
		                          313,317,331,337,347,349,353,359,367,373,379,383,389,397,401,409,419,421,431,
		                          433,439,443,449,457,461,463,467,479,487,491,499,503,509,521,523,541,547,557,
		                          563,569,571,577,587,593,599,601,607,613,617,619,631,641,643,647,653,659,661,
		                          673,677,683,691,701,709,719,727,733,739,743,751,757,761,769,773,787,797,809,
		                          811,821,823,827,829,839,853,857,859,863,877,881,883,887,907,911,919,929,937,
		                          941,947,953,967,971,977,983,991,997};
		
		/// <summary>
		/// Finds the next number that is a prime.
		/// Note: No guarantee is given that the returned number is in fact a prime
		/// </summary>
		/// <param name="n">n a number</param>
		/// <returns>the next prime</returns>
		internal static BigInteger next(BigInteger n)
		{
			n |= 1;
			while (true){
				if (MillerRabin(n))
					return n;
				n += 2;
			}
		}

		/// <summary>
		/// Checks whether a number will be a prime
		/// Note: No guarantee is given that the returned number is in fact a prime
		/// </summary>
		/// <param name="n">The number to be checked</param>
		/// <returns>whether n is a prime</returns>
		static bool MillerRabin(BigInteger n)
		{
			BigInteger d = n - 1;
			int s = 0;
			while ((d & 1) == 0) { d >>= 1; s++; }
			int i, j;
			for (i = 0; i < ar.Length; i++)
			{
				BigInteger a = BigInteger.Min(n - 2, ar[i]);
				BigInteger now = BigInteger.ModPow(a, d, n);
				if (now == 1) continue;
				if (now == n - 1) continue;
				for (j = 1; j < s; j++)
				{
					now = (now * now) % n;
					if (now == n - 1) break;
				}
				if (j == s) return false;
			}
			return true;
		}
	}
}
