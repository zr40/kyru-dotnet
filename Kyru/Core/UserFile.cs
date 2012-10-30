using System;
using System.Collections.Generic;
using Kyru.Network;

namespace Kyru.Core
{
	internal class UserFile
	{
		private readonly Config config;
		internal List<KademliaId> ChunkIds;
		internal byte[] EncryptedFileKey;
		internal byte[] EncryptedFileName;
		internal byte[] Hash;
		internal ulong Id;

		internal UserFile(List<KademliaId> chunkIds, Config config)
		{
			ChunkIds = chunkIds;
			this.config = config;
		}

		/// <summary>
		/// Return all chunks belonging to the file
		/// </summary>
		/// <returns></returns>
		internal List<Chunk> RetrieveChunks()
		{
			return new KObjectSet(config).GetList<Chunk>(ChunkIds, true);
		}
	}
}