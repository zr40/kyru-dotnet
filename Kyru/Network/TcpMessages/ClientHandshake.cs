using ProtoBuf;

namespace Kyru.Network.TcpMessages
{
	[ProtoContract]
	internal sealed class ClientHandshake
	{
		[ProtoMember(1)]
		internal KademliaId NodeId;

		[ProtoMember(2)]
		internal GetObjectRequest GetObjectRequest;

		[ProtoMember(3)]
		internal StoreObjectRequest StoreObjectRequest;

		[ProtoMember(4)]
		internal ushort Port;
	}
}