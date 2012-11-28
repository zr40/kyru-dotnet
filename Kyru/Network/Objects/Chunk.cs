using ProtoBuf;
using Kyru.Utilities;

namespace Kyru.Network.Objects
{
	[ProtoContract(SkipConstructor = true)]
	internal class Chunk : KyruObject
	{
		[ProtoMember(1)]
		internal readonly byte[] Data;

		internal Chunk(byte[] data)
		{
			Data = data;
		}

		internal override bool VerifyData()
		{
			return (ObjectId == generateID());
		}

		internal KademliaId generateID() {
			byte[] hash = Crypto.Hash(Data);
			return new KademliaId(hash);
		}
	}
}