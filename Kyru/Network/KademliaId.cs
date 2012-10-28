using System;
using System.Linq;

using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class KademliaId
	{
		private const int ArraySize = 160 / 8 / sizeof(uint);

		[ProtoMember(1)]
		private readonly uint[] id = new uint[ArraySize];

		private KademliaId()
		{
		}

		public KademliaId(byte[] bytes)
		{
			if (bytes.Length != ArraySize * sizeof(uint))
				throw new Exception("The array must of size " + ArraySize * sizeof(uint));

			for (int i = 0; i < ArraySize; i++)
			{
				id[i] = BitConverter.ToUInt32(bytes, i * sizeof(uint));
			}
		}

		public static KademliaId operator -(KademliaId left, KademliaId right)
		{
			var result = new KademliaId();
			for (int i = 0; i < ArraySize; i++)
			{
				result.id[i] = left.id[i] ^ right.id[i];
			}
			return result;
		}

		public static KademliaId RandomId
		{
			get
			{
				var bytes = Random.Bytes(ArraySize * sizeof(uint));

				return new KademliaId(bytes);
			}
		}

		public override string ToString()
		{
			return id.Aggregate("", (s, u) => s + u.ToString("0,8:X"));
		}
	}
}