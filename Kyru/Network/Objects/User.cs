using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly byte[] publicKey;

		[Obsolete("TODO: use other constructor")]
		internal User()
		{
			// used by serialization
		}

		internal User(byte[] publicKey)
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

		internal void Merge(User user)
		{
			foreach (var file in user.files)
			{
				if (files.All(f => f.FileId != file.FileId) && deletedFiles.All(f => f.Item2 != file.FileId))
					Add(file);
			}

			foreach (var file in user.deletedFiles)
			{
				if (deletedFiles.All(f => f.Item2 != file.Item2))
					AddDeletedFile(file.Item1, file.Item2);
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
			if (Crypto.VerifySignature(userFile.HashObject(), publicKey, userFile.Signature))
			{
				files.Add(userFile);
			}
		}

		/// <summary>
		/// Checks if the signature is valid and, if so, adds it to the deleted file list and deletes the UserFile object
		/// </summary>
		/// <param name="fileId">fileId</param>
		/// <param name="signature">Cryptographic signature of the fileId</param>
		internal void AddDeletedFile(byte[] signature, ulong fileId)
		{
			if (Crypto.VerifySignature(BitConverter.GetBytes(fileId), publicKey, signature))
			{
				deletedFiles.Add(new Tuple<byte[], ulong>(signature, fileId));
				files.RemoveAll(kf => kf.FileId == fileId);
			}
		}

		/// <summary>
		/// Verifies all signatures in the (deleted) file list and checks for inconsistencies
		/// </summary>
		/// <returns>True if no problems were found, false if an invalid signature or inconsistency was found</returns>
		internal override bool VerifyData()
		{
			if (files.Any(file => !Crypto.VerifySignature(file.HashObject(), publicKey, file.Signature)))
			{
				return false;
			}
			foreach (Tuple<byte[], ulong> file in deletedFiles)
			{
				if (!Crypto.VerifySignature(BitConverter.GetBytes(file.Item2), publicKey, file.Item1))
					return false;
				if (files.Any(f => f.FileId == file.Item2)) 
					return false;
			}
			return true;
		}
	}
}