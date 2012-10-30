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
		private List<KFile> files;

		internal User(string name, KademliaId publicKey)
		{
			Name = name;
			id = publicKey;
			deletedFiles = new List<Tuple<byte[], ulong>>();
			files = new List<KFile>();
		}

		internal ReadOnlyCollection<KFile> Files
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
		/// <param name="kFile">file to add</param>
		internal void Add(KFile kFile)
		{
			files.Add(kFile);
		}

		/// <summary>
		/// Checks if the signature is valid and, if so, adds it to the deleted file list and deletes the KFile object
		/// </summary>
		/// <param name="deletedFile">signature + fileId</param>
		private void AddDeletedFile(Tuple<byte[], ulong> deletedFile)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(id);
			if (Convert.ToUInt64(rsa.Encrypt(deletedFile.Item1, true)) == deletedFile.Item2)
			{
				deletedFiles.Add(deletedFile);
				files.RemoveAll(kF => kF.Id == deletedFile.Item2);
			}
		}

		/// <summary>
		/// Decrypts the file name of a given KFile
		/// </summary>
		/// <param name="kFile">File of which the name is desired</param>
		/// <returns>The filename</returns>
		internal string DecryptFileName(KFile kFile)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.ImportCspBlob(id);
			return rsa.Decrypt(kFile.EncryptedFileName, true).ToString();
		}

		/// <summary> // Question: why not do a normal deserialisation? this would also allow Name to be readonly again.
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

		/// <summary>
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