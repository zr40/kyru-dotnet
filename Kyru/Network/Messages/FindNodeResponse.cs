using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class FindNodeResponse
	{
		[ProtoMember(1)]
		internal NodeInformation[] Nodes;
	}
}