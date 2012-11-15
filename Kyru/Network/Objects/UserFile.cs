using System.Collections.Generic;
using ProtoBuf;

namespace Kyru.Network.Objects
{
	[ProtoContract]
	internal class UserFile
	{
		[ProtoMember(1)]
		internal byte[] CryptographicSignature;

		[ProtoMember(2)]
		internal ulong FileId;

		[ProtoMember(3)]
		internal byte[] EncryptedEncryptionKey;

		[ProtoMember(4)] 
		internal byte[] EncryptedIV;

		[ProtoMember(5)]
		internal byte[] EncryptedFileName;

		[ProtoMember(6)]
		internal byte[] HashOfEncryptedFileContents;

		[ProtoMember(7)]
		internal List<KademliaId> ChunkList = new List<KademliaId>();
	}
}