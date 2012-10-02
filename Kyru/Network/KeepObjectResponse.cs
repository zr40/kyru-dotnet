using ProtoBuf;

namespace Kyru.Network
{
	[ProtoContract]
	internal sealed class KeepObjectResponse
	{
		[ProtoMember(1)]
		internal bool HasObject;
	}
}