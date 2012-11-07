using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class FindValueResponse
	{
		[ProtoMember(1)]
		internal NodeInformation[] Nodes;

		[ProtoMember(2)]
		internal KyruObjectMetadata[] Data;
	}
}