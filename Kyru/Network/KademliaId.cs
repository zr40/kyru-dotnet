using System;

using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class KademliaId
	{
		private const int ArraySize = 160 / 8 / sizeof(uint);

		[ProtoMember(1)]
		private readonly uint[] id = new uint[ArraySize];

        public KademliaId()
        {
        }

        public KademliaId(byte[] data) {
            if (data.Length != ArraySize * sizeof(uint))
                throw new Exception("160 bits of data is required for creating a kademlia Id");

            for (int i = 0; i < ArraySize; i++)
            {
                id[i] = BitConverter.ToUInt32(data, i * sizeof(uint));
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
	}
}