using System.Net.Sockets;

using Kyru.Core;

using ProtoBuf;

namespace Kyru.Network.TcpMessages.ServerState
{
	internal sealed class GetObjectState : IServerState
	{
		private readonly NetworkStream stream;
		private readonly App app;
		private readonly GetObjectRequest getObjectRequest;

		internal GetObjectState(NetworkStream stream, App app, GetObjectRequest getObjectRequest)
		{
			this.stream = stream;
			this.app = app;
			this.getObjectRequest = getObjectRequest;
		}

		public IServerState Process()
		{
			var bytes = app.LocalObjectStorage.GetBytes(getObjectRequest.ObjectId);
			var response = new GetObjectResponse();
			if (bytes == null)
			{
				response.Error = Error.NotFound;
				Serializer.SerializeWithLengthPrefix(stream, response, PrefixStyle.Base128);
				return null;
			}

			response.Error = Error.Success;
			response.Length = (uint) bytes.Length;

			Serializer.SerializeWithLengthPrefix(stream, response, PrefixStyle.Base128);

			stream.Write(bytes, 0, bytes.Length);
			return null;
		}
	}
}