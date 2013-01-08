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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Button button1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemStatusForm));
			this.CurrentContactsLabel = new System.Windows.Forms.Label();
			this.EstimatedNetworkSizeLabel = new System.Windows.Forms.Label();
			this.ObjectsStoredLabel = new System.Windows.Forms.Label();
			timer1 = new System.Windows.Forms.Timer(this.components);
			label1 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			button1 = new System.Windows.Forms.Button();
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
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(95, 13);
			label1.TabIndex = 0;
			label1.Text = "Current contacts:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(12, 26);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(128, 13);
			label3.TabIndex = 2;
			label3.Text = "Estimated network size:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(12, 57);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(85, 13);
			label2.TabIndex = 4;
			label2.Text = "Objects stored:";
			// 
			// button1
			// 
			button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			button1.Location = new System.Drawing.Point(173, 90);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 8;
			button1.Text = "Close";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(this.button1_Click);
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
			// EstimatedNetworkSizeLabel
			// 
			this.EstimatedNetworkSizeLabel.AutoSize = true;
			this.EstimatedNetworkSizeLabel.Location = new System.Drawing.Point(155, 26);
			this.EstimatedNetworkSizeLabel.Name = "EstimatedNetworkSizeLabel";
			this.EstimatedNetworkSizeLabel.Size = new System.Drawing.Size(38, 13);
			this.EstimatedNetworkSizeLabel.TabIndex = 3;
			this.EstimatedNetworkSizeLabel.Text = "label4";
			// 
			// ObjectsStoredLabel
			// 
			this.ObjectsStoredLabel.AutoSize = true;
			this.ObjectsStoredLabel.Location = new System.Drawing.Point(155, 57);
			this.ObjectsStoredLabel.Name = "ObjectsStoredLabel";
			this.ObjectsStoredLabel.Size = new System.Drawing.Size(38, 13);
			this.ObjectsStoredLabel.TabIndex = 6;
			this.ObjectsStoredLabel.Text = "label5";
			// 
			// SystemStatusForm
			// 
			this.AcceptButton = button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = button1;
			this.ClientSize = new System.Drawing.Size(260, 125);
			this.Controls.Add(button1);
			this.Controls.Add(this.ObjectsStoredLabel);
			this.Controls.Add(label2);
			this.Controls.Add(this.EstimatedNetworkSizeLabel);
			this.Controls.Add(label3);
			this.Controls.Add(this.CurrentContactsLabel);
			this.Controls.Add(label1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SystemStatusForm";
			this.Text = "Kyru system status";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label CurrentContactsLabel;
		private System.Windows.Forms.Label EstimatedNetworkSizeLabel;
		private System.Windows.Forms.Label ObjectsStoredLabel;


	}
}