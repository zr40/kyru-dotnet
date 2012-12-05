using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kyru.Network;
using Kyru.Network.Objects;
using Kyru.Network.TcpMessages;
using Kyru.Utilities;
using Random = Kyru.Utilities.Random;

namespace Kyru.Core
{
	/// <summary>
	/// The Session class provides the ability to change a user object in a way that other nodes will accept, by signing it with the appropriate key
	/// </summary>
	internal sealed class Session
	{
		internal readonly User User;
		private readonly LocalObjectStorage localObjectStorage;
		private readonly RsaKeyPair rsaKeyPair;

		/// <summary>
		/// Constructor of Session class for a new User
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="password">Password of the user</param>
		/// <param name="localObjectStorage">LocalObjectStorage</param>
		internal Session(string username, string password, LocalObjectStorage localObjectStorage)
		{
			this.localObjectStorage = localObjectStorage;

			rsaKeyPair = Crypto.DeriveRsaKey(Encoding.UTF8.GetBytes(username), Encoding.UTF8.GetBytes(password));

			var id = new KademliaId(Crypto.Hash(rsaKeyPair.Public));

			Username = username;

			User = localObjectStorage.GetObject(id) as User ?? new User(rsaKeyPair.Public) {ObjectId = id};
		}

		internal string Username { get; private set; }

		/// <summary>
		/// Decrypts the file name of a given UserFile
		/// </summary>
		/// <param name="userFile">File of which the name is desired</param>
		/// <returns>The filename</returns>
		internal string DecryptFileName(UserFile userFile)
		{
			return Encoding.UTF8.GetString(Crypto.DecryptAes(userFile.EncryptedFileName, DecryptKey(userFile), userFile.NameIV));
		}

		private void AddChunk(List<KademliaId> chunkIDs, byte[] chunkData)
		{
			var chunkId = new KademliaId(Crypto.Hash(chunkData));
			chunkIDs.Add(chunkId);
			localObjectStorage.StoreObject(new Chunk(chunkData, chunkId), true);
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
			byte[] nameIV = Crypto.GenerateIV();

			input.Read(data, 0, (int) input.Length);
			data = Crypto.EncryptAes(data, fileKey, fileIV);

			int lastChunkSize = data.Length%LocalObjectStorage.MaxObjectSize;
			int chunks = data.Length/LocalObjectStorage.MaxObjectSize;
			var chunkData = new byte[LocalObjectStorage.MaxObjectSize];

			for (int i = 0; i < chunks; i++)
			{
				Array.Copy(data, i*LocalObjectStorage.MaxObjectSize, chunkData, 0, LocalObjectStorage.MaxObjectSize);
				AddChunk(chunkList, chunkData);
			}

			if (lastChunkSize != 0)
			{
				Array.Copy(data, chunks*LocalObjectStorage.MaxObjectSize, chunkData, 0, lastChunkSize);
				AddChunk(chunkList, chunkData.Take(lastChunkSize).ToArray());
			}

			var userFile = new UserFile
			               	{
			               		FileId = Random.UInt64(),
			               		ChunkList = chunkList,
			               		EncryptedFileName = Crypto.EncryptAes(Encoding.UTF8.GetBytes(fileName), fileKey, nameIV),
			               		EncryptedKey = Crypto.EncryptRsa(fileKey, rsaKeyPair.Public),
			               		FileIV = fileIV,
			               		NameIV = nameIV,
			               		Hash = Crypto.Hash(data),
			               	};
			userFile.Signature = Crypto.Sign(userFile.HashObject(), rsaKeyPair);

			User.Add(userFile);

			localObjectStorage.StoreObject(User, true);
			return userFile;
		}

		/// <summary>
		/// Changes a user's file's status to deleted
		/// </summary>
		/// <param name="userFile"></param>
		internal void DeleteFile(UserFile userFile)
		{
			User.AddDeletedFile(Crypto.Sign(BitConverter.GetBytes(userFile.FileId), rsaKeyPair),
			                    userFile.FileId);

			localObjectStorage.StoreObject(User, true);
		}

		/// <summary>
		/// decrypts the filekey belonging to a Kfile
		/// </summary>
		/// <param name="userFile">Kfile object containing an encrypted filekey</param>
		/// <returns>the decrypted filekey</returns>
		private byte[] DecryptKey(UserFile userFile)
		{
			return Crypto.DecryptRsa(userFile.EncryptedKey, rsaKeyPair);
		}

		/// <summary>
		/// Decrypts a file and outputs the result in the stream. The file's chunks must be present before calling this method.
		/// </summary>
		/// <param name="userFile">the file to decrypt</param>
		/// <param name="output">the destination of the decrypted file</param>
		/// <exception cref="NullReferenceException">One or more of the chunks could not be found</exception>
		internal void DecryptFile(UserFile userFile, Stream output)
		{
			byte[] bytes;
			using (var ms = new MemoryStream())
			{
				foreach (KademliaId chunkId in userFile.ChunkList)
				{
					var chunk = localObjectStorage.GetObject(chunkId) as Chunk;
					ms.Write(chunk.Data, 0, chunk.Data.Length);
				}
				
				bytes = ms.ToArray();
			}
			bytes = Crypto.DecryptAes(bytes, DecryptKey(userFile), userFile.FileIV);
			output.Write(bytes, 0, bytes.Length);
		}

		internal void DownloadObjects(List<KademliaId> ids, Action<Error> done)
		{
			localObjectStorage.DownloadObjects(ids, done);
		}
	}
}