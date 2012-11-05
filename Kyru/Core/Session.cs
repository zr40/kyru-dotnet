﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

using Kyru.Network.Objects;

namespace Kyru.Core
{
	/// <summary>
	/// The Session class provides the ability to change a user object in a way that other nodes will accept, by signing it with the appropriate key
	/// </summary>
	internal sealed class Session
	{
		private byte[] privateKey;
		private App app;
		private User user;

		/// <summary>
		/// Constructor of Session class for a new User
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="password">Password of the user</param>
		internal Session(string username, string password, App app)
		{
			var sha1 = SHA1.Create();
			var bytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(username.ToCharArray()));
			var id = new Network.KademliaId(bytes);
			this.app = app;
			this.user = app.LocalObjectStorage.Get(id).User;
			if (this.user == null)
			{
				// A new user
				this.user = new User(username);
			}

			// TODO: Fill KademliaId correctly;
			// TODO: Create privateKey to encrypt/decrypt files
		}

		/// <summary>
		/// Decrypts the file name of a given UserFile
		/// </summary>
		/// <param name="userFile">File of which the name is desired</param>
		/// <returns>The filename</returns>
		internal string DecryptFileName(UserFile userFile)
		{
			return System.Text.Encoding.UTF8.GetString(userFile.EncryptedFileName);
			//TODO: encryption;
		}

		internal User User
		{
			get
			{
				return user;
			}
		}

		/// <summary>
		/// Creates a UserFile from a normal file
		/// </summary>
		/// <param name="file">the file to add to the User's files</param>
		/// <returns></returns>
		internal UserFile AddFile(FileStream file)
		{
			byte[] data = new byte[file.Length];
			var idList = new List<Network.KademliaId>();
			file.Read(data, 0, (int) file.Length);
			Chunk chunk = new Chunk(data);
			idList.Add(chunk.ObjectId);
			UserFile userFile = new UserFile(idList);
			userFile.EncryptedFileName = System.Text.Encoding.UTF8.GetBytes(file.Name.ToCharArray());
			User.Add(userFile);

			app.LocalObjectStorage.Store(chunk);
			app.LocalObjectStorage.Store(user);
			return userFile;
		}

		/// <summary>
		/// Changes a user's file's status to deleted
		/// </summary>
		/// <param name="userFile"></param>
		internal void DeleteFile(UserFile userFile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Uses the private key to sign a message
		/// </summary>
		/// <param name="message">message to sign</param>
		/// <returns>signed message</returns>
		private byte[] SignMessage(byte[] message)
		{
			return message;
		}

		/// <summary>
		/// decrypts the filekey belonging to a Kfile
		/// </summary>
		/// <param name="userFile">Kfile object containing an encrypted filekey</param>
		/// <returns>the decrypted filekey</returns>
		private byte[] DecryptFileKey(UserFile userFile)
		{
			return userFile.EncryptedFileDecryptionKey;
			//TODO: encryption;
		}


		/// <summary>
		/// Decrypts a kfile and outputs the result in file
		/// </summary>
		/// <param name="userFile">the file to decrypt</param>
		/// <param name="fileStream">the destination of the decrypted file</param>
		internal void DecryptFile(UserFile userFile, FileStream fileStream)
		{
			foreach (var afile in userFile.ChunkList)
			{
				var chunk = app.LocalObjectStorage.Get(afile).Chunk;
				fileStream.Write(chunk.Data, 0, chunk.Data.Length);
			}
			// TODO: Encryption
			// TODO: Work with chunks
		}
	}
}