using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

		[ProtoMember(3)]
		private readonly RSAParameters publicKey;

		internal event Action<UserFile> OnFileAdded;
		internal event Action<ulong> OnFileDeleted;

		[Obsolete("TODO: use other constructor")]
		internal User()
		{
			// used by serialization
		}

		internal User(RSAParameters publicKey)
		{
			this.publicKey = publicKey;
		}

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
			// TODO: check signature
			files.Add(userFile);
			if (OnFileAdded != null)
				OnFileAdded(userFile);
		}

		/// <summary>
		/// Checks if the signature is valid and, if so, adds it to the deleted file list and deletes the UserFile object
		/// </summary>
		/// <param name="deletedFile">signature + fileId</param>
		internal void AddDeletedFile(byte[] signature, ulong fileId)
		{
			if (Crypto.VerifySignature(BitConverter.GetBytes(fileId), publicKey, signature))
			{
				deletedFiles.Add(new Tuple<byte[], ulong>(signature, fileId));
				files.RemoveAll(kf => kf.FileId == fileId);

				if (OnFileDeleted != null)
					OnFileDeleted(fileId);
			}
		}

		/// <summary>
		/// Verifies all signatures in the (deleted) file list and checks for inconsistencies
		/// </summary>
		/// <returns>True if no problems were found, false if an invalid signature or inconsistency was found</returns>
		internal override bool VerifyData()
		{
			// TODO
			return true;
		}
	}
}