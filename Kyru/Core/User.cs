using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Kyru.Network;
using ProtoBuf;


namespace Kyru.Core
{
	/// <summary>
	/// The User class contains public as well as encrypted data
	/// </summary>
	[ProtoContract, Serializable]
	internal class User : KObject
	{
		[ProtoMember(1)]
		internal string Name;
		[ProtoMember(2)]
		private List<Tuple<byte[], ulong>> deletedFiles;
		[ProtoMember(3)]
		private List<UserFile> files;

		public User() {
		
		}

		internal User(string name, KademliaId publicKey)
		{
			Name = name;
			Id = publicKey;
			deletedFiles = new List<Tuple<byte[], ulong>>();
			files = new List<UserFile>();
		}

		internal ReadOnlyCollection<UserFile> Files
		{
			get { return files.AsReadOnly(); }
		}

		internal ReadOnlyCollection<Tuple<byte[], ulong>> DeletedFiles
		{
			get { return deletedFiles.AsReadOnly(); }
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
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(Id.Bytes);
			if (Convert.ToUInt64(rsa.Encrypt(deletedFile.Item1, true)) == deletedFile.Item2)
			{
				deletedFiles.Add(deletedFile);
				files.Remove(files.Find(kF => kF.Id == deletedFile.Item2));
			}
		}
		// todo: protobuf (de)serialization, see UDPMessage for declaration and Node for usage
	}
}