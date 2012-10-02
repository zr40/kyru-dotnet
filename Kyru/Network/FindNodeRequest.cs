using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class FindNodeRequest
	{
		[ProtoMember(1)]
		internal KademliaId NodeId;
	}
}