using System;
using System.IO;
using System.Security.Cryptography;

namespace Kyru.Core
{
	/// <summary>
	/// The Session class provides the ability to change a user object in a way that other nodes will accept, by signing it with the appropriate key
	/// </summary>
	internal sealed class Session
	{
		private byte[] privateKey;

		/// <summary>
		/// Constructor of Session class for an existing User
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="password">Password of the user</param>
		/// <param name="config">The configuration settings</param>
		/// <param name="user">User object corresponding to the User</param>
		internal Session(string username, string password, Config config, User user)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Constructor of Session class for a new User
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="password">Password of the user</param>
		internal Session(string username, string password, Config config)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Decrypts the file name of a given UserFile
		/// </summary>
		/// <param name="userFile">File of which the name is desired</param>
		/// <returns>The filename</returns>
		internal string DecryptFileName(UserFile userFile)
		{
			throw new NotImplementedException();
		}

		internal User User { get; private set; }

		/// <summary>
		/// Creates a UserFile from a normal file
		/// </summary>
		/// <param name="file">the file to add to the User's files</param>
		/// <returns></returns>
		internal UserFile AddFile(FileStream file)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// decrypts the filekey belonging to a Kfile
		/// </summary>
		/// <param name="userFile">Kfile object containing an encrypted filekey</param>
		/// <returns>the decrypted filekey</returns>
		private byte[] DecryptFileKey(UserFile userFile)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Decrypts a kfile and outputs the result in file
		/// </summary>
		/// <param name="userFile">the file to decrypt</param>
		/// <param name="file">the destination of the decrypted file</param>
		internal void DecryptFile(UserFile userFile, FileStream file)
		{
			throw new NotImplementedException();
		}
	}
}