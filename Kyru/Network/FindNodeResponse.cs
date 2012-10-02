using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class FindNodeResponse
	{
		[ProtoMember(1)]
		internal NodeInformation[] Nodes;
	}
}