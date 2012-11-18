using System;
using System.Net;
using System.Windows.Forms;

using Kyru.Network;

namespace Kyru
{
	internal partial class AddNodeForm : Form
	{
		private Kademlia kademlia;

		internal AddNodeForm(Kademlia kademlia)
		{
			InitializeComponent();
			this.kademlia = kademlia;
		}

		private void btOk_Click(object sender, EventArgs e)
		{
			IPAddress address;
			int port;
			if (IPAddress.TryParse(txtIp.Text, out address) && int.TryParse(txtPort.Text, out port))
			{
				kademlia.AddNode(new IPEndPoint(address, port));
				this.Close();
			}
		}
	}
}