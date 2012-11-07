using System;
using System.Security.Cryptography;

namespace Kyru.Utilities
{
	internal static class Random
	{
		private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

		internal static byte[] Bytes(int byteCount)
		{
			var bytes = new byte[byteCount];
			rng.GetBytes(bytes);
			return bytes;
		}

		internal static ulong UInt64()
		{
			var bytes = Bytes(sizeof(ulong));
			return BitConverter.ToUInt64(bytes, 0);
		}
	}
}