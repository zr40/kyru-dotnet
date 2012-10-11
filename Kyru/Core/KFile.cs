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
			throw new NotImplementedException();
		}

		internal List<Chunk> Split()
		{
			throw new NotImplementedException();
		}

		internal void Rename(string newName)
		{
			throw new NotImplementedException();
		}

		internal void Load(string Name)
		{
			throw new NotImplementedException();
		}

		internal void Save(string Name)
		{
			throw new NotImplementedException();
		}
	}
}