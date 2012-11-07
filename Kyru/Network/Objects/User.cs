using System;
using System.Collections.Generic;
using Kyru.Core;
using Kyru.Utilities;
using ProtoBuf;

namespace Kyru.Network.Objects
{
	/// <summary>
	/// The User class contains public as well as encrypted data
	/// </summary>
	[ProtoContract]
	internal class User : KyruObject
	{
		[ProtoMember(1)]
		private readonly List<UserFile> files = new List<UserFile>();

		[ProtoMember(2)]
		private readonly List<Tuple<byte[], ulong>> deletedFiles = new List<Tuple<byte[], ulong>>();

		internal IList<UserFile> Files
		{
			get
			{
				return files.AsReadOnly();
			}
		}

		internal IList<Tuple<byte[], ulong>> DeletedFiles
		{
			get
			{
				return deletedFiles.AsReadOnly();
			}
		}

		/// <summary>
		/// Adds a file to the file list
		/// </summary>
		/// <param name="userFile">file to add</param>
		internal void Add(UserFile userFile)
		{
			files.Add(userFile);
		}

		/// <summary>
		/// Checks if the signature is valid and, if so, adds it to the deleted file list and deletes the UserFile object
		/// </summary>
		/// <param name="deletedFile">signature + fileId</param>
		private void AddDeletedFile(byte[] signature, ulong fileId)
		{
			if (Crypto.VerifySignature(BitConverter.GetBytes(fileId),ObjectId.Bytes, signature))
			{
				deletedFiles.Add(new Tuple<byte[], ulong>(signature, fileId));
				files.RemoveAll(kf => kf.FileId == fileId);
			}
		}
	}
}