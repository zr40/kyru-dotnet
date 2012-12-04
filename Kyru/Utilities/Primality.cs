using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Kyru.Utilities
{
	internal static class Primality
	{
		private static bool IsProbablyPrime(BigInteger num)
		{
			const int iterations = 40;

			// Miller-Rabin probabilistic test
			// code adapted from http://rosettacode.org/wiki/Miller-Rabin_primality_test#C.23

			if (num == 2 || num == 3)
				return true;
			if (num < 2 || num % 2 == 0)
				return false;

			BigInteger d = num - 1;
			int s = 0;

			while (d % 2 == 0)
			{
				d /= 2;
				s += 1;
			}

			RandomNumberGenerator rng = RandomNumberGenerator.Create();
			var bytes = new byte[num.ToByteArray().LongLength];
			BigInteger a;

			for (int i = 0; i < iterations; i++)
			{
				do
				{
					rng.GetBytes(bytes);
					a = new BigInteger(bytes);
				} while (a < 2 || a >= num - 2);

				BigInteger x = BigInteger.ModPow(a, d, num);
				if (x == 1 || x == num - 1)
					continue;

				for (int r = 1; r < s; r++)
				{
					x = BigInteger.ModPow(x, 2, num);
					if (x == 1)
						return false;
					if (x == num - 1)
						break;
				}

				if (x != num - 1)
					return false;
			}

			return true;
		}

		internal static bool IsPrime(this BigInteger num)
		{
			return IsProbablyPrime(num);

			// TODO: test ECPP speed (for deterministic primality test)
		}

		internal static BigInteger FindPrime(this BigInteger num)
		{
			while (!num.IsPrime())
			{
				//Console.WriteLine("Primality: {0} is not prime", num);
				num++;
			}
			//Console.WriteLine("Primality: {0} is probably prime", num);
			return num;
		}
	}
}