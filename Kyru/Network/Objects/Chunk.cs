using System.Linq;
using Kyru.Utilities;

using ProtoBuf;

namespace Kyru.Network.Objects
{
	[ProtoContract(SkipConstructor = true)]
	internal class Chunk : KyruObject
	{
		[ProtoMember(1)]
		internal readonly byte[] Data;

		internal Chunk(byte[] data, KademliaId objectId)
		{
			Data = data;
			ObjectId = objectId;
		}

		internal override bool VerifyData()
		{
			return ObjectId.Bytes.SequenceEqual(Crypto.Hash(Data));
		}
	}
}