using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
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
			txtIp.BackColor = SystemColors.Window;
			txtPort.BackColor = SystemColors.Window;

			int port;
			if (!int.TryParse(txtPort.Text, out port) || port < 1 || port > ushort.MaxValue)
			{
				txtPort.BackColor = Color.LightCoral;
				return;
			}

			IPAddress[] addressList;
			try
			{
				addressList = Dns.GetHostAddresses(txtIp.Text);
			}
			catch (SocketException)
			{
				txtIp.BackColor = Color.LightCoral;
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
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}