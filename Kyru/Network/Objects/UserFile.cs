using System;
using System.Collections.Generic;
using System.Linq;

using Kyru.Utilities;

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

		[ProtoMember(8)] internal List<byte[]> ChunkList = new List<byte[]>();

		internal byte[] HashObject()
		{
			var bytes = new List<byte>();

			bytes.AddRange(BitConverter.GetBytes(FileId));
			bytes.AddRange(EncryptedKey);
			bytes.AddRange(FileIV);
			bytes.AddRange(NameIV);
			bytes.AddRange(BitConverter.GetBytes(EncryptedFileName.Length));
			bytes.AddRange(EncryptedFileName);
			bytes.AddRange(BitConverter.GetBytes(ChunkList.Count));
			foreach (var chunkId in ChunkList)
			{
				bytes.AddRange(chunkId);
			}

			return Crypto.Hash(bytes.ToArray());
		}

		internal bool ValidateData()
		{
			return EncryptedKey.Length == 32 && FileIV.Length == 16 && NameIV.Length == 16 && Hash.Length == 20;
		}
	}
}