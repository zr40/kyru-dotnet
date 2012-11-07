using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class KyruObjectMetadata
	{
		[ProtoMember(1)]
		internal KademliaId NodeId;

		[ProtoMember(2)]
		internal uint IpAddress;

		[ProtoMember(3)]
		internal ushort Port;

		[ProtoMember(4)]
		internal uint Timestamp;
	}
}