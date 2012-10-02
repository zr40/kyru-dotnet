using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class KeepObjectRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;
	}
}