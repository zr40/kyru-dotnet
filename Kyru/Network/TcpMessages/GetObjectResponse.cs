using ProtoBuf;

namespace Kyru.Network.TcpMessages
{
	internal sealed class GetObjectResponse
	{
		[ProtoMember(1)]
		internal Error Error;

		[ProtoMember(2)]
		internal uint Length;

		// object contents are sent as-is, following this serialized data.
	}
}