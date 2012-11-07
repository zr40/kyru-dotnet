using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Kyru.Network;

namespace Kyru
{
	internal partial class AddNodeForm : Form
	{
		Kademlia kademlia;
		internal AddNodeForm(Kademlia kademlia)
		{
			InitializeComponent();
			this.kademlia = kademlia;
		}

		private void btOk_Click(object sender, EventArgs e)
		{
			IPAddress address;
			int port;
			if (IPAddress.TryParse(txtIp.Text,out address) && int.TryParse(txtPort.Text,out port)){
				kademlia.AddNode(new IPEndPoint(address,port));
				//this.Close();
			}
		}
	}
}
