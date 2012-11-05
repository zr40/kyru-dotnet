using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class FindNodeResponse
	{
		[ProtoMember(1)]
		internal NodeInformation[] Nodes;
	}
}