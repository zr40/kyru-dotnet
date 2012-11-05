using System;
using System.Collections.Generic;
using Kyru.Network;
using ProtoBuf;

namespace Kyru.Core
{
	[ProtoContract]
	internal class UserFile
	{
		[ProtoMember(1)]
		internal List<KademliaId> ChunkIds;
		[ProtoMember(2)]
		internal byte[] EncryptedFileKey;
		[ProtoMember(3)]
		internal byte[] EncryptedFileName;
		[ProtoMember(4)]
		internal byte[] Hash;
		[ProtoMember(5)]
		internal ulong Id;

		internal UserFile() {
		
		}

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