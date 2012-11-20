using System.Collections.Generic;

using ProtoBuf;

namespace Kyru.Network.Objects
{
	[ProtoContract]
	internal class UserFile
	{
		[ProtoMember(1)]
		internal byte[] Signature;

		[ProtoMember(2)]
		internal ulong FileId;

		[ProtoMember(3)]
		internal byte[] EncryptedKey;

		[ProtoMember(4)]
		internal byte[] IV;

		[ProtoMember(5)]
		internal byte[] EncryptedFileName;

		[ProtoMember(6)]
		internal byte[] Hash; // Hash of encrypted file contents.

		[ProtoMember(7)]
		internal List<KademliaId> ChunkList = new List<KademliaId>();
	}
}