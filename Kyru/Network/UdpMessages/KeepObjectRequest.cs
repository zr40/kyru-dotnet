using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class KeepObjectRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;
	}
}