using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class FindNodeRequest
	{
		[ProtoMember(1)]
		internal byte[] NodeId;
	}
}