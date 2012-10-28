using Kyru.Network.Messages;

namespace Kyru.Network
{
	internal sealed class Kademlia
	{
		private readonly Node node;

		internal Kademlia(Node node)
		{
			this.node = node;
		}

		/// <summary>
		/// Notifies Kademlia about incoming request messages. If necessary, a ping request is added to the response message.
		/// </summary>
		/// <param name="request">The incoming request message.</param>
		/// <param name="response">The response object that will be sent.</param>
		internal void HandleRequest(UdpMessage request, UdpMessage response)
		{
			throw new System.NotImplementedException();
		}
	}
}