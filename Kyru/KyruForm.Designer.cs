namespace Kyru
{
    partial class KyruForm
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
			  this.virtualLocalFileTree = new System.Windows.Forms.TreeView();
			  this.iconList = new System.Windows.Forms.ImageList(this.components);
			  this.topMenu = new System.Windows.Forms.MenuStrip();
			  this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.addAFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.addANodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.configureDiskLimitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			  this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			  this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.topMenu.SuspendLayout();
			  this.rightClickMenu.SuspendLayout();
			  this.SuspendLayout();
			  // 
			  // virtualLocalFileTree
			  // 
			  this.virtualLocalFileTree.Dock = System.Windows.Forms.DockStyle.Fill;
			  this.virtualLocalFileTree.Location = new System.Drawing.Point(0, 24);
			  this.virtualLocalFileTree.Name = "virtualLocalFileTree";
			  this.virtualLocalFileTree.Size = new System.Drawing.Size(292, 242);
			  this.virtualLocalFileTree.TabIndex = 0;
			  this.virtualLocalFileTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.virtualLocalFileTree_MouseUp);
			  // 
			  // iconList
			  // 
			  this.iconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			  this.iconList.ImageSize = new System.Drawing.Size(16, 16);
			  this.iconList.TransparentColor = System.Drawing.Color.Transparent;
			  // 
			  // topMenu
			  // 
			  this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem});
			  this.topMenu.Location = new System.Drawing.Point(0, 0);
			  this.topMenu.Name = "topMenu";
			  this.topMenu.Size = new System.Drawing.Size(292, 24);
			  this.topMenu.TabIndex = 1;
			  this.topMenu.Text = "menuStrip1";
			  // 
			  // systemToolStripMenuItem
			  // 
			  this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAFileToolStripMenuItem,
            this.addANodeToolStripMenuItem,
            this.configureDiskLimitsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
			  this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
			  this.systemToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			  this.systemToolStripMenuItem.Text = "System";
			  // 
			  // addAFileToolStripMenuItem
			  // 
			  this.addAFileToolStripMenuItem.Name = "addAFileToolStripMenuItem";
			  this.addAFileToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			  this.addAFileToolStripMenuItem.Text = "Add a file";
			  this.addAFileToolStripMenuItem.Click += new System.EventHandler(this.addAFileToolStripMenuItem_Click);
			  // 
			  // addANodeToolStripMenuItem
			  // 
			  this.addANodeToolStripMenuItem.Name = "addANodeToolStripMenuItem";
			  this.addANodeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			  this.addANodeToolStripMenuItem.Text = "Add a node";
			  this.addANodeToolStripMenuItem.Click += new System.EventHandler(this.addANodeToolStripMenuItem_Click);
			  // 
			  // configureDiskLimitsToolStripMenuItem
			  // 
			  this.configureDiskLimitsToolStripMenuItem.Name = "configureDiskLimitsToolStripMenuItem";
			  this.configureDiskLimitsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			  this.configureDiskLimitsToolStripMenuItem.Text = "Configure Disk Limits";
			  this.configureDiskLimitsToolStripMenuItem.Visible = false;
			  // 
			  // toolStripSeparator1
			  // 
			  this.toolStripSeparator1.Name = "toolStripSeparator1";
			  this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
			  this.toolStripSeparator1.Visible = false;
			  // 
			  // quitToolStripMenuItem
			  // 
			  this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			  this.quitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			  this.quitToolStripMenuItem.Text = "Quit";
			  this.quitToolStripMenuItem.Visible = false;
			  // 
			  // rightClickMenu
			  // 
			  this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.infoToolStripMenuItem});
			  this.rightClickMenu.Name = "rightClickMenu";
			  this.rightClickMenu.Size = new System.Drawing.Size(108, 70);
			  // 
			  // saveToolStripMenuItem
			  // 
			  this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			  this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			  this.saveToolStripMenuItem.Text = "Save";
			  this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			  // 
			  // deleteToolStripMenuItem
			  // 
			  this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			  this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			  this.deleteToolStripMenuItem.Text = "Delete";
			  this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			  // 
			  // infoToolStripMenuItem
			  // 
			  this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
			  this.infoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			  this.infoToolStripMenuItem.Text = "Info";
			  this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
			  // 
			  // KyruForm
			  // 
			  this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			  this.ClientSize = new System.Drawing.Size(292, 266);
			  this.Controls.Add(this.virtualLocalFileTree);
			  this.Controls.Add(this.topMenu);
			  this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			  this.MainMenuStrip = this.topMenu;
			  this.Name = "KyruForm";
			  this.Text = "Kyru";
			  this.topMenu.ResumeLayout(false);
			  this.topMenu.PerformLayout();
			  this.rightClickMenu.ResumeLayout(false);
			  this.ResumeLayout(false);
			  this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView virtualLocalFileTree;
        private System.Windows.Forms.ImageList iconList;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addANodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureDiskLimitsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem addAFileToolStripMenuItem;
		  private System.Windows.Forms.ContextMenuStrip rightClickMenu;
		  private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
    }
}

