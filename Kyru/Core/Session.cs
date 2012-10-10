using System;
using System.IO;
using System.Security;

namespace Kyru.Core
{
	/// <summary>
	/// The Session class provides the ability to change a user object in a way that other nodes will accept, by signing it with the appropriate key
	/// </summary>
	internal class Session
	{
		private byte[] _privateKey;
		private User _user;

		/// <summary>
		/// Constructor of Session class
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="password">Password of the user</param>
		/// <param name="user">User object corresponding to the User</param>
		internal Session(string username, string password, User user)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a KFile from a normal file
		/// </summary>
		/// <param name="file">the file to add to the User's files</param>
		/// <returns></returns>
		internal KFile AddFile(FileStream file)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Changes a user's file's status to deleted
		/// </summary>
		/// <param name="kFile"></param>
		internal void DeleteFile(KFile kFile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Uses the private key to sign a message
		/// </summary>
		/// <param name="message">message to sign</param>
		/// <returns>signed message</returns>
		internal string SignMessage(string message)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Decrypts a kfile and outputs the result in file
		/// </summary>
		/// <param name="kFile">the file to decrypt</param>
		/// <param name="file">the destination of the decrypted file</param>
		internal void DecryptFile(KFile kFile, FileStream file)
		{
			throw new NotImplementedException();
		}
	}
}