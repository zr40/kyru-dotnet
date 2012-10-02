using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class UdpMessage
	{
		[ProtoMember(1)]
		internal uint ProtocolVersion;

		[ProtoMember(2)]
		internal KademliaId SenderNodeId;

		[ProtoMember(3)]
		internal ulong MessageId;

		[ProtoMember(4)]
		internal PingRequest PingRequest;

		[ProtoMember(5)]
		internal FindNodeRequest FindNodeRequest;

		[ProtoMember(6)]
		internal FindNodeResponse FindNodeResponse;

		[ProtoMember(7)]
		internal FindValueRequest FindValueRequest;

		[ProtoMember(8)]
		internal FindValueResponse FindValueResponse;

		[ProtoMember(9)]
		internal StoreRequest StoreRequest;

		[ProtoMember(10)]
		internal StoreResponse StoreResponse;

		[ProtoMember(11)]
		internal KeepObjectRequest KeepObjectRequest;

		[ProtoMember(12)]
		internal KeepObjectResponse KeepObjectResponse;
	}
}