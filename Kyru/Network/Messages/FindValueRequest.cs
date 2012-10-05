using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class FindValueRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;
	}
}