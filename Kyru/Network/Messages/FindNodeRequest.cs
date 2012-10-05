using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class FindNodeRequest
	{
		[ProtoMember(1)]
		internal KademliaId NodeId;
	}
}