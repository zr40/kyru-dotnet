using Kyru.Utilities;

using ProtoBuf;

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
			return ObjectId == CalculateHash();
		}

		internal KademliaId CalculateHash()
		{
			byte[] hash = Crypto.Hash(Data);
			return new KademliaId(hash);
		}
	}
}