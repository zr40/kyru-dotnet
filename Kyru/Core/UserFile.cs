using System;
using System.Collections.Generic;
using Kyru.Network;

namespace Kyru.Core
{
	[Serializable]
	internal class UserFile
	{
		internal List<KademliaId> ChunkIds;
		internal byte[] EncryptedFileKey;
		internal byte[] EncryptedFileName;
		internal byte[] Hash;
		internal ulong Id;

		internal UserFile(List<KademliaId> chunkIds)
		{
			ChunkIds = chunkIds;
		}

		/// <summary>
		/// Return all chunks belonging to the file
		/// </summary>
		/// <returns></returns>
		internal List<Chunk> RetrieveChunks(Config config)
		{
			return new KObjectSet(config).GetList<Chunk>(ChunkIds, true);
		}
	}
}