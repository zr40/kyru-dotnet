using System.Collections.Generic;

using ProtoBuf;

namespace Kyru.Network.UdpMessages
{
	[ProtoContract]
	internal sealed class StoreRequest
	{
		[ProtoMember(1)]
		internal KademliaId ObjectId;

		[ProtoMember(2)]
		internal KyruObjectMetadata[] Data = new KyruObjectMetadata[0];
	}
}