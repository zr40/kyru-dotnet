using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class FindValueRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;
	}
}