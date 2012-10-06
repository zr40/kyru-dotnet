using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class UdpMessage
	{
		[ProtoMember(1)]
		internal uint ProtocolVersion;

		[ProtoMember(2)]
		internal KademliaId SenderNodeId;

		[ProtoMember(3)]
		internal ulong RequestId;

		[ProtoMember(4)]
		internal ulong ResponseId;

		[ProtoMember(5)]
		internal PingRequest PingRequest;

		[ProtoMember(6)]
		internal FindNodeRequest FindNodeRequest;

		[ProtoMember(7)]
		internal FindNodeResponse FindNodeResponse;

		[ProtoMember(8)]
		internal FindValueRequest FindValueRequest;

		[ProtoMember(9)]
		internal FindValueResponse FindValueResponse;

		[ProtoMember(10)]
		internal StoreRequest StoreRequest;

		[ProtoMember(11)]
		internal StoreResponse StoreResponse;

		[ProtoMember(12)]
		internal KeepObjectRequest KeepObjectRequest;

		[ProtoMember(13)]
		internal KeepObjectResponse KeepObjectResponse;
	}
}