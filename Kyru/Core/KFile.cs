using System;
using System.Collections.Generic;
using Kyru.Network;

namespace Kyru.Core
{
	internal class KFile
	{
		internal List<KademliaId> ChunkIds;
		internal ulong Id;
		internal byte[] Hash;
		internal byte[] EncryptedFileKey;
		internal byte[] EncryptedFileName;
		
		internal KFile(List<KademliaId> chunkIds)
		{
            this.ChunkIds = chunkIds;
		}

        /// <summary>
        /// Return all chunks where the file is made of.
        /// </summary>
        /// <returns></returns>
		internal List<Chunk> Split()
		{
            // TIP: Use KObjectSet.GetList
			throw new NotImplementedException();
		}

		internal void Rename(string newName)
		{
			throw new NotImplementedException();
		}

		internal void Load(string Name)
		{
            // TIP: Use KObjectSet.Add for each chunk
			throw new NotImplementedException();
		}

		internal void Save(string Name)
		{
            // TIP: Use KObjectSet.GetList
			throw new NotImplementedException();
		}
	}
}