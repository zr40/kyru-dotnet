using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class FindValueResponse
	{
		[ProtoMember(1)]
		internal NodeInformation[] Nodes = new NodeInformation[0];

		[ProtoMember(2)]
		internal KyruObjectMetadata[] Data = new KyruObjectMetadata[0];
	}
}