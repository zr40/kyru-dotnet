using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class KeepObjectRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;
	}
}