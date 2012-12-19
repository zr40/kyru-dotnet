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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KyruForm));
			this.virtualLocalFileTree = new System.Windows.Forms.TreeView();
			this.iconList = new System.Windows.Forms.ImageList(this.components);
			this.topMenu = new System.Windows.Forms.MenuStrip();
			this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addAFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.virtualLocalFileTree.AllowDrop = true;
			this.virtualLocalFileTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.virtualLocalFileTree.ImageIndex = 0;
			this.virtualLocalFileTree.ImageList = this.iconList;
			this.virtualLocalFileTree.Location = new System.Drawing.Point(0, 24);
			this.virtualLocalFileTree.Name = "virtualLocalFileTree";
			this.virtualLocalFileTree.SelectedImageIndex = 0;
			this.virtualLocalFileTree.Size = new System.Drawing.Size(292, 242);
			this.virtualLocalFileTree.TabIndex = 0;
			this.virtualLocalFileTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.virtualLocalFileTree_DragDrop);
			this.virtualLocalFileTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.virtualLocalFileTree_DragEnter);
			this.virtualLocalFileTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.virtualLocalFileTree_MouseUp);
			// 
			// iconList
			// 
			this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
			this.iconList.TransparentColor = System.Drawing.Color.Transparent;
			this.iconList.Images.SetKeyName(0, "root.gif");
			this.iconList.Images.SetKeyName(1, "folder.gif");
			this.iconList.Images.SetKeyName(2, "uploading.gif");
			this.iconList.Images.SetKeyName(3, "file.gif");
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
            this.refreshToolStripMenuItem});
			this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
			this.systemToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.systemToolStripMenuItem.Text = "System";
			// 
			// addAFileToolStripMenuItem
			// 
			this.addAFileToolStripMenuItem.Name = "addAFileToolStripMenuItem";
			this.addAFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.addAFileToolStripMenuItem.Text = "Add a file";
			this.addAFileToolStripMenuItem.Click += new System.EventHandler(this.addAFileToolStripMenuItem_Click);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
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
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// infoToolStripMenuItem
			// 
			this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
			this.infoToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
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
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.topMenu;
			this.Name = "KyruForm";
			this.Text = "Kyru";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KyruForm_FormClosing);
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
		  private System.Windows.Forms.ToolStripMenuItem addAFileToolStripMenuItem;
		  private System.Windows.Forms.ContextMenuStrip rightClickMenu;
		  private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
		  private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}

