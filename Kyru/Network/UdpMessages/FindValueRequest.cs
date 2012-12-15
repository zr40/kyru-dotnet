using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class FindValueRequest
	{
		[ProtoMember(1)]
		internal byte[] ObjectId;
	}
}