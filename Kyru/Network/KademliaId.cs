using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class KademliaId
	{
		private const int ArraySize = 160 / 8 / sizeof(uint);

		[ProtoMember(1)]
		private readonly uint[] id = new uint[ArraySize];

		public static KademliaId operator -(KademliaId left, KademliaId right)
		{
			var result = new KademliaId();
			for (int i = 0; i < ArraySize; i++)
			{
				result.id[i] = left.id[i] ^ right.id[i];
			}
			return result;
		}
	}
}