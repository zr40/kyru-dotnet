using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class NodeInformation
	{
		[ProtoMember(1)]
		internal KademliaId NodeId;

		[ProtoMember(2)]
		internal uint IpAddress;

		[ProtoMember(3)]
		internal ushort Port;
	}
}