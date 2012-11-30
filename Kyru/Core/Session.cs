using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Kyru.Network;
using Kyru.Network.Objects;
using Kyru.Utilities;

namespace Kyru.Core
{
	/// <summary>
	/// The Session class provides the ability to change a user object in a way that other nodes will accept, by signing it with the appropriate key
	/// </summary>
	internal sealed class Session
	{
		private byte[] privateKey;
		private LocalObjectStorage localObjectStorage;
		internal readonly User User;
		internal string Username { get; private set; }

		/// <summary>
		/// Constructor of Session class for a new User
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="password">Password of the user</param>
		internal Session(string username, string password, LocalObjectStorage localObjectStorage)
		{
			this.localObjectStorage = localObjectStorage;

			// TODO: Derive privateKey from username and password (in order to encrypt/decrypt files)

			// TODO: id must be hash of public key
			var bytes = Crypto.Hash(Encoding.UTF8.GetBytes(username));
			var id = new KademliaId(bytes);

			Username = username;

			// TODO: this is the wrong place; create session only when the user object exists or has been created
			User = localObjectStorage.GetObject(id) as User;
			if (User == null)
			{
				// A new user
				User = new User(null); // TODO public key
				User.ObjectId = id;
			}
		}

		/// <summary>
		/// Decrypts the file name of a given UserFile
		/// </summary>
		/// <param name="userFile">File of which the name is desired</param>
		/// <returns>The filename</returns>
		internal string DecryptFileName(UserFile userFile)
		{
			return Encoding.UTF8.GetString(Crypto.DecryptAes(userFile.EncryptedFileName, DecryptFileKey(userFile), userFile.IV));
			//TODO: decryption
		}

		/// <summary>
		/// Creates a UserFile from a normal file
		/// </summary>
		/// <param name="input">the file to add to the User's files</param>
		/// <param name="fileName">the file's name</param>
		/// <returns></returns>
		internal UserFile AddFile(Stream input, string fileName)
		{
			var data = new byte[input.Length];
			var chunkList = new List<KademliaId>();
			byte[] fileKey = Crypto.GenerateAesKey();
			byte[] fileIV = Crypto.GenerateIV();

			// TODO: split into 1 MiB chunks
			input.Read(data, 0, (int) input.Length);
			data = Crypto.EncryptAes(data,fileKey,fileIV);
			var chunk = new Chunk(data);

			chunk.ObjectId = chunk.CalculateHash();
			chunkList.Add(chunk.ObjectId);
			
			var userFile = new UserFile {
				ChunkList = chunkList,
				EncryptedFileName = Crypto.EncryptAes(Encoding.UTF8.GetBytes(fileName),fileKey, fileIV),
				EncryptedKey = fileKey, // TODO: RSA encrypt the key
				IV = fileIV
				// TODO: Missing fields
			};

			User.Add(userFile);

			localObjectStorage.StoreObject(chunk, true);
			localObjectStorage.StoreObject(User, true);
			return userFile;
		}

		/// <summary>
		/// Changes a user's file's status to deleted
		/// </summary>
		/// <param name="userFile"></param>
		internal void DeleteFile(UserFile userFile)
		{
			// TODO: remove the file from the file list if it is present
			// Then add it to deleted files.
			// Finally, store the User.
			throw new NotImplementedException();
		}

		/// <summary>
		/// Uses the private key to sign a message
		/// </summary>
		/// <param name="message">message to sign</param>
		/// <returns>signed message</returns>
		private byte[] SignMessage(byte[] message)
		{
			// TODO sign
			return message;
		}

		/// <summary>
		/// decrypts the filekey belonging to a Kfile
		/// </summary>
		/// <param name="userFile">Kfile object containing an encrypted filekey</param>
		/// <returns>the decrypted filekey</returns>
		private byte[] DecryptFileKey(UserFile userFile)
		{
			return userFile.EncryptedKey;
			//TODO: decryption;
		}

		/// <summary>
		/// Decrypts a file and outputs the result in the stream. The file's chunks must be present before calling this method.
		/// </summary>
		/// <param name="userFile">the file to decrypt</param>
		/// <param name="output">the destination of the decrypted file</param>
		/// <exception cref="NullReferenceException">One or more of the chunks could not be found</exception>
		internal void DecryptFile(UserFile userFile, Stream output)
		{
			var ms = new MemoryStream();
			foreach (KademliaId chunkId in userFile.ChunkList)
			{
				var chunk = localObjectStorage.GetObject(chunkId) as Chunk;
				ms.Write(chunk.Data, 0, chunk.Data.Length);
			}

			var bytes = ms.ToArray();
			bytes = Crypto.DecryptAes(bytes, DecryptFileKey(userFile), userFile.IV);
			output.Write(bytes, 0, bytes.Length);
		}
	}
}