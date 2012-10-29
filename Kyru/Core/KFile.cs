using System;
using System.Collections.Generic;
using Kyru.Network;

namespace Kyru.Core
{
	[Serializable]
	internal class KFile
	{
		private readonly Config config;
		internal List<KademliaId> ChunkIds;
		internal byte[] EncryptedFileKey;
		internal byte[] EncryptedFileName;
		internal byte[] Hash;
		internal ulong Id;

		internal KFile(List<KademliaId> chunkIds, Config config)
		{
			ChunkIds = chunkIds;
			config = config;
		}

		/// <summary>
		/// Return all chunks belonging to the file
		/// </summary>
		/// <returns></returns>
		internal List<Chunk> Split() // Note: the name of this method doesn't correspond well to its docstring description.
		{
			return new KObjectSet(config).GetList<Chunk>(ChunkIds, true);
		}

		internal void Load(string Name)
		{
			// TIP: Use KObjectSet.Add for each chunk
			throw new NotImplementedException();
			// no docstring: it is not clear to me what is the intention of this method.
		}

		internal void Save(string Name)
		{
			// TIP: Use KObjectSet.GetList
			throw new NotImplementedException();
			// no docstring: it is not clear to me what is the intention of this method.
		}
	}
}