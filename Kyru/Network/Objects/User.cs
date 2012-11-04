using System;
using System.Collections.Generic;

using ProtoBuf;

namespace Kyru.Network.Objects
{
	/// <summary>
	/// The User class contains public as well as encrypted data
	/// </summary>
	[ProtoContract(SkipConstructor = true), Serializable]
	internal class User : KyruObject
	{
		internal string Name;

		[ProtoMember(1)]
		private List<Tuple<byte[], ulong>> deletedFiles;

		[ProtoMember(2)]
		private List<UserFile> files;

		internal User(string name)
		{
			Name = name;
			deletedFiles = new List<Tuple<byte[], ulong>>();
			files = new List<UserFile>();
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
			files.Add(userFile);
		}

		/// <summary>
		/// Checks if the signature is valid and, if so, adds it to the deleted file list and deletes the UserFile object
		/// </summary>
		/// <param name="deletedFile">signature + fileId</param>
		private void AddDeletedFile(Tuple<byte[], ulong> deletedFile)
		{
			throw new NotImplementedException();
			/*var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(Id.Bytes);
			if (Convert.ToUInt64(rsa.Encrypt(deletedFile.Item1, true)) == deletedFile.Item2)
			{
				deletedFiles.Add(deletedFile);
				files.Remove(files.Find(kF => kF.FileId == deletedFile.Item2));
			}*/
		}

		// todo: protobuf (de)serialization, see UDPMessage for declaration and Node for usage
	}
}