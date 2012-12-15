using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class StoreRequest
	{
		[ProtoMember(1)]
		internal byte[] ObjectId;

		[ProtoMember(2)]
		internal KyruObjectMetadata[] Data = new KyruObjectMetadata[0];
	}
}