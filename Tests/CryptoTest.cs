using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kyru.Utilities;
using MbUnit.Framework;
using System;
using Random = System.Random;

namespace Tests
{
	internal sealed class CryptoTest
	{
		[Test]
		internal void TestAES()
		{
			var key = Crypto.GenerateAesKey();
			var data = new byte[10000];
			new Random().NextBytes(data);
			Assert.AreElementsEqual(data, Crypto.DecryptAes(Crypto.EncryptAes(data, key), key));
		}
	}
}
