using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class FindValueResponse
	{
		[ProtoMember(1)]
		internal NodeInformation[] Nodes;

		[ProtoMember(2)]
		internal KyruObjectMetadata[] Value;
	}
}