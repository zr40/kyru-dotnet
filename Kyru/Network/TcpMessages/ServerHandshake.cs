using ProtoBuf;

namespace Kyru.Network.TcpMessages
{
	[ProtoContract]
	internal sealed class ServerHandshake
	{
		[ProtoMember(1)]
		internal byte[] NodeId;
	}
}