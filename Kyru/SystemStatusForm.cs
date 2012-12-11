using System.Windows.Forms;

using Kyru.Core;

namespace Kyru
{
	internal sealed partial class SystemStatusForm : Form
	{
		private readonly KyruApplication app;

		internal SystemStatusForm(KyruApplication app)
		{
			this.app = app;
			InitializeComponent();
			timer1_Tick(null, null);
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			CurrentContactsLabel.Text = app.Node.Kademlia.CurrentContactCount.ToString();
			EstimatedNetworkSizeLabel.Text = app.Node.Kademlia.EstimatedNetworkSize.ToString();
		}
	}
}