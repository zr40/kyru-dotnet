using ProtoBuf;

namespace Kyru.Network.Messages
{
	[ProtoContract]
	internal sealed class KeepObjectResponse
	{
		[ProtoMember(1)]
		internal bool HasObject;
	}
}