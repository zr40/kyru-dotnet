using System;
using System.Collections.Generic;
using System.Linq;

using ProtoBuf;

namespace Kyru.Network.Objects
{
	[ProtoContract]
	internal class UserFile
	{
		[ProtoMember(1)] internal byte[] Signature;

		[ProtoMember(2)] internal ulong FileId;

		[ProtoMember(3)] internal byte[] EncryptedKey;

		[ProtoMember(4)] internal byte[] FileIV;

		[ProtoMember(5)] internal byte[] NameIV;

		[ProtoMember(6)] internal byte[] EncryptedFileName;

		[ProtoMember(7)] internal byte[] Hash; // Hash of encrypted file contents.

		[ProtoMember(8)] internal List<KademliaId> ChunkList = new List<KademliaId>();

		internal byte[] HashObject()
		{
			throw new NotImplementedException();
		}
	}
}