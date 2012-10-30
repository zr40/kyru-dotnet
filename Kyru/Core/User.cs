using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Kyru.Network;

namespace Kyru.Core
{
	/// <summary>
	/// The User class contains public as well as encrypted data
	/// </summary>
	[Serializable]
	internal class User : KObject
	{
		internal string Name;
		private List<Tuple<byte[], ulong>> deletedFiles;
		private List<UserFile> files;

		internal User(string name, KademliaId publicKey)
		{
			Name = name;
			id = publicKey;
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
			rsa.ImportCspBlob(id.Bytes);
			if (Convert.ToUInt64(rsa.Encrypt(deletedFile.Item1, true)) == deletedFile.Item2)
			{
				deletedFiles.Add(deletedFile);
				files.Remove(files.Find(kF => kF.Id == deletedFile.Item2));
			}
		}

		/// <summary> // TODO: Change to binary serialization
		/// Reads the file from the harddisk
		/// </summary>
		/// <param name="f">A stream of the file where the object is in</param>
		public override void Read(FileStream f)
		{
			var x = new XmlSerializer(GetType()); // Question: Why XML?
			var loaded = (User) x.Deserialize(f);

			files = loaded.files;
			deletedFiles = loaded.deletedFiles;
			id = loaded.id;
			Name = loaded.Name;
		}

		/// <summary>  // TODO: Change to binary serialization
		/// Writes the file to the harddisk
		/// </summary>
		/// <param name="f">A stream of the file</param>
		public override void Write(FileStream f)
		{
			var x = new XmlSerializer(GetType()); // Question: Why XML?
			x.Serialize(Console.Out, this);
		}
	}
}