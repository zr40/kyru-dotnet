using System.Collections.Generic;

using Kyru.Core;

using ProtoBuf;

namespace Kyru.Network.Objects
{
	[ProtoContract(SkipConstructor = true)]
	internal class UserFile
	{
		private readonly List<KademliaId> chunkList;

		[ProtoMember(1)]
		internal byte[] CryptographicSignature;

		[ProtoMember(2)]
		internal ulong FileId;

		[ProtoMember(3)]
		internal byte[] EncryptedFileDecryptionKey;

		[ProtoMember(4)]
		internal byte[] EncryptedFileName;

		[ProtoMember(5)]
		internal byte[] HashOfEncryptedFileContents;

		[ProtoMember(6)]
		internal List<KademliaId> ChunkList;

		internal UserFile(List<KademliaId> chunkList)
		{
			this.chunkList = chunkList;
		}

		/// <summary>
		/// Return all chunks belonging to the file
		/// </summary>
		/// <returns></returns>
		internal List<Chunk> RetrieveChunks(Config config)
		{
			return new KObjectSet(config).GetList(ChunkList, true).ConvertAll(o => o.Chunk);
		}
	}
}