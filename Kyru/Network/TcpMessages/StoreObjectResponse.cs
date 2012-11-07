using ProtoBuf;

namespace Kyru.Network.TcpMessages
{
	[ProtoContract]
	internal sealed class StoreObjectResponse
	{
		[ProtoMember(1)]
		internal Error Error;

		// if Error is Success, client will send the object contents as-is.
	}
}