using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class KeepObjectResponse
	{
		[ProtoMember(1)]
		internal bool HasObject;
	}
}