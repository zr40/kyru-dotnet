namespace Kyru
{
	internal partial class SystemStatusForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Timer timer1;
			this.label1 = new System.Windows.Forms.Label();
			this.CurrentContactsLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.EstimatedNetworkSizeLabel = new System.Windows.Forms.Label();
			timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// timer1
			// 
			timer1.Enabled = true;
			timer1.Interval = 1000;
			timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Current contacts:";
			// 
			// CurrentContactsLabel
			// 
			this.CurrentContactsLabel.AutoSize = true;
			this.CurrentContactsLabel.Location = new System.Drawing.Point(155, 9);
			this.CurrentContactsLabel.Name = "CurrentContactsLabel";
			this.CurrentContactsLabel.Size = new System.Drawing.Size(38, 13);
			this.CurrentContactsLabel.TabIndex = 1;
			this.CurrentContactsLabel.Text = "label2";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 26);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Estimated network size:";
			// 
			// EstimatedNetworkSizeLabel
			// 
			this.EstimatedNetworkSizeLabel.AutoSize = true;
			this.EstimatedNetworkSizeLabel.Location = new System.Drawing.Point(155, 26);
			this.EstimatedNetworkSizeLabel.Name = "EstimatedNetworkSizeLabel";
			this.EstimatedNetworkSizeLabel.Size = new System.Drawing.Size(38, 13);
			this.EstimatedNetworkSizeLabel.TabIndex = 3;
			this.EstimatedNetworkSizeLabel.Text = "label4";
			// 
			// SystemStatusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.EstimatedNetworkSizeLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.CurrentContactsLabel);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SystemStatusForm";
			this.Text = "SystemStatusForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label CurrentContactsLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label EstimatedNetworkSizeLabel;


	}
}