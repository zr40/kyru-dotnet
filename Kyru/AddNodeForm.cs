using System;
using System.Net;
using System.Windows.Forms;

using Kyru.Network;
using System.Net.Sockets;

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
			int port;
			if (int.TryParse(txtPort.Text, out port))
			{
				IPAddress[] addressList;
				try
				{
					addressList = System.Net.Dns.GetHostAddresses(txtIp.Text);
				}
				catch
				{
					txtIp.Text = "";
					txtIp.BackColor = System.Drawing.Color.Red;
					return;
				}
				foreach (var address in addressList)
				{
					if (address.AddressFamily == AddressFamily.InterNetwork)
					{
						// IPv4 only
						kademlia.AddNode(new IPEndPoint(address, port));
					}
				}
				this.Close();
			}
		}
	}
}