using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kyru.Utilities;
using MbUnit.Framework;
using Random = System.Random;

namespace Tests
{
	internal sealed class CryptoTest
	{
		[Test]
		internal void TestAES()
		{
			var key = Crypto.GenerateAesKey();
			var data = new byte[10001];
			new Random().NextBytes(data);
			var IV = Crypto.GenerateIV();
			Assert.AreElementsEqual(data, Crypto.DecryptAes(Crypto.EncryptAes(data, key, IV), key, IV));
		}
	}
}
