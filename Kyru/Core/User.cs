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
		private List<Tuple<byte[], KademliaId>> _deletedFiles;
		private List<KFile> _files;

		internal User(string name, KademliaId publicKey)
		{
			throw new NotImplementedException();
		}

		internal ReadOnlyCollection<KFile> Files
		{
			get { return _files.AsReadOnly(); }
		}

		internal ReadOnlyCollection<Tuple<byte[], KademliaId>> DeletedFiles
		{
			get { return _deletedFiles.AsReadOnly(); }
		}

		/// <summary>
		/// processes a keep request for a KFile according to the Kyru Spec
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
		/// Checks if the signature is valid and, if so, adds it to the deleted file list
		/// </summary>
		/// <param name="deletedFile">signature + fileId</param>
		private void AddDeletedFile(Tuple<byte[], KademliaId> deletedFile)
		{
			throw new NotImplementedException();
		}
	}
}