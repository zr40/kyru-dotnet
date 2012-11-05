using ProtoBuf;

namespace Kyru.Network.TcpMessages
{
	[ProtoContract]
	internal sealed class StoreObjectRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;

		[ProtoMember(2)]
		internal byte[] Hash;

		[ProtoMember(3)]
		internal uint Length;
	}
}