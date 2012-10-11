using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kyru.Network;

namespace Kyru.Core
{
	/// <summary>
	/// The User class contains public as well as encrypted data
	/// </summary>
	internal class User
	{
		internal readonly string Name;
		internal KademliaId PublicKey;
		private List<Tuple<byte[], KademliaId>> deletedFiles;
		private List<KFile> files;

		internal User(string name, KademliaId publicKey)
		{
			throw new NotImplementedException();
		}

		internal ReadOnlyCollection<KFile> Files
		{
			get { return files.AsReadOnly(); }
		}

		internal ReadOnlyCollection<Tuple<byte[], KademliaId>> DeletedFiles
		{
			get { return deletedFiles.AsReadOnly(); }
		}

		/// <summary>
		/// Processes a keep request for a KFile according to the Kyru Spec
		/// </summary>
		/// <param name="kFile">file to be kept</param>
		internal void Keep(KFile kFile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds a file to the file list
		/// </summary>
		/// <param name="kFile">file to add</param>
		internal void Add(KFile kFile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks if the signature is valid and, if so, adds it to the deleted file list and deletes the KFile object
		/// </summary>
		/// <param name="deletedFile">signature + fileId</param>
		private void AddDeletedFile(Tuple<byte[], KademliaId> deletedFile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Decrypts the file name of a given KFile
		/// </summary>
		/// <param name="kFile">File of which the name is desired</param>
		/// <returns>The filename</returns>
		internal string DecryptFileName(KFile kFile)
		{
			throw new NotImplementedException();
		}
	}
}