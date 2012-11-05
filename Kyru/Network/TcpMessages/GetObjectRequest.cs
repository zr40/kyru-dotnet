using ProtoBuf;

namespace Kyru.Network.TcpMessages
{
	[ProtoContract]
	internal sealed class GetObjectRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;
	}
}