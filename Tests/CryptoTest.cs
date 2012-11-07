using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kyru.Utilities;
using MbUnit.Framework;
using Random = Kyru.Utilities.Random;

namespace Tests
{
	internal sealed class CryptoTest
	{
		[Test]
		internal void TestAES()
		{
			var key = Crypto.GenerateAesKey();
			var data = Random.Bytes(10000);
			Assert.AreElementsEqual(data, Crypto.DecryptAes(Crypto.EncryptAes(data, key), key));
		}
	}
}
