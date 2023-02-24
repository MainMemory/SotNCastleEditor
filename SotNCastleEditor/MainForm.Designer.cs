
namespace SotNCastleEditor
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.syncIdenticalRoomsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomLevelSelector = new System.Windows.Forms.ToolStripComboBox();
			this.showGraphicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clipGraphicsToMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showBorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showOtherAreasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showTeleportDestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.layoutContainerPanel = new System.Windows.Forms.Panel();
			this.layoutPanel = new SotNCastleEditor.BufferedPanel();
			this.tileBottomSelector = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tileRightSelector = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tileTopSelector = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tileLeftSelector = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tileTypeSelector = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.roomTilePanel = new System.Windows.Forms.Panel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.layoutAreaSelect = new System.Windows.Forms.ToolStripComboBox();
			this.moveWholeAreaButton = new System.Windows.Forms.ToolStripButton();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.showReverseCastleCheckBox = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.teleportPreview = new SotNCastleEditor.BufferedPanel();
			this.teleportYCoord = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.teleportXCoord = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.teleportRoomSelect = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.teleportAreaSelect = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.teleportListBox = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editTeleportDestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveTeleportDestHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.teleportPrevTSSelect = new System.Windows.Forms.ComboBox();
			this.menuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.layoutContainerPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.teleportYCoord)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.teleportXCoord)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.teleportRoomSelect)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip1.Size = new System.Drawing.Size(800, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncIdenticalRoomsToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// syncIdenticalRoomsToolStripMenuItem
			// 
			this.syncIdenticalRoomsToolStripMenuItem.Checked = true;
			this.syncIdenticalRoomsToolStripMenuItem.CheckOnClick = true;
			this.syncIdenticalRoomsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.syncIdenticalRoomsToolStripMenuItem.Name = "syncIdenticalRoomsToolStripMenuItem";
			this.syncIdenticalRoomsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.syncIdenticalRoomsToolStripMenuItem.Text = "&Sync Identical Rooms";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomLevelSelector,
            this.showGraphicsToolStripMenuItem,
            this.clipGraphicsToMapToolStripMenuItem,
            this.showBorderToolStripMenuItem,
            this.showGridToolStripMenuItem,
            this.showOtherAreasToolStripMenuItem,
            this.showTeleportDestsToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// zoomLevelSelector
			// 
			this.zoomLevelSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.zoomLevelSelector.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "75%",
            "100%",
            "125%",
            "150%",
            "200%",
            "300%",
            "400%",
            "500%"});
			this.zoomLevelSelector.Name = "zoomLevelSelector";
			this.zoomLevelSelector.Size = new System.Drawing.Size(121, 23);
			this.zoomLevelSelector.SelectedIndexChanged += new System.EventHandler(this.zoomLevelSelector_SelectedIndexChanged);
			// 
			// showGraphicsToolStripMenuItem
			// 
			this.showGraphicsToolStripMenuItem.Checked = true;
			this.showGraphicsToolStripMenuItem.CheckOnClick = true;
			this.showGraphicsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showGraphicsToolStripMenuItem.Name = "showGraphicsToolStripMenuItem";
			this.showGraphicsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.showGraphicsToolStripMenuItem.Text = "Show &Graphics";
			this.showGraphicsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewOption_CheckedChanged);
			// 
			// clipGraphicsToMapToolStripMenuItem
			// 
			this.clipGraphicsToMapToolStripMenuItem.Checked = true;
			this.clipGraphicsToMapToolStripMenuItem.CheckOnClick = true;
			this.clipGraphicsToMapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.clipGraphicsToMapToolStripMenuItem.Name = "clipGraphicsToMapToolStripMenuItem";
			this.clipGraphicsToMapToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.clipGraphicsToMapToolStripMenuItem.Text = "&Clip Graphics To Map";
			this.clipGraphicsToMapToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewOption_CheckedChanged);
			// 
			// showBorderToolStripMenuItem
			// 
			this.showBorderToolStripMenuItem.Checked = true;
			this.showBorderToolStripMenuItem.CheckOnClick = true;
			this.showBorderToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showBorderToolStripMenuItem.Name = "showBorderToolStripMenuItem";
			this.showBorderToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.showBorderToolStripMenuItem.Text = "Show Map &Borders";
			this.showBorderToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewOption_CheckedChanged);
			// 
			// showGridToolStripMenuItem
			// 
			this.showGridToolStripMenuItem.CheckOnClick = true;
			this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
			this.showGridToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.showGridToolStripMenuItem.Text = "Show Gri&d";
			this.showGridToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewOption_CheckedChanged);
			// 
			// showOtherAreasToolStripMenuItem
			// 
			this.showOtherAreasToolStripMenuItem.Checked = true;
			this.showOtherAreasToolStripMenuItem.CheckOnClick = true;
			this.showOtherAreasToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showOtherAreasToolStripMenuItem.Name = "showOtherAreasToolStripMenuItem";
			this.showOtherAreasToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.showOtherAreasToolStripMenuItem.Text = "Show &Other Areas";
			this.showOtherAreasToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewOption_CheckedChanged);
			// 
			// showTeleportDestsToolStripMenuItem
			// 
			this.showTeleportDestsToolStripMenuItem.CheckOnClick = true;
			this.showTeleportDestsToolStripMenuItem.Name = "showTeleportDestsToolStripMenuItem";
			this.showTeleportDestsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.showTeleportDestsToolStripMenuItem.Text = "Show &Teleport Dests";
			this.showTeleportDestsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewOption_CheckedChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 24);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(800, 426);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.splitContainer1);
			this.tabPage1.Controls.Add(this.toolStrip1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(792, 400);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Layout";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(3, 28);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.layoutContainerPanel);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tileBottomSelector);
			this.splitContainer1.Panel2.Controls.Add(this.label5);
			this.splitContainer1.Panel2.Controls.Add(this.tileRightSelector);
			this.splitContainer1.Panel2.Controls.Add(this.label4);
			this.splitContainer1.Panel2.Controls.Add(this.tileTopSelector);
			this.splitContainer1.Panel2.Controls.Add(this.label3);
			this.splitContainer1.Panel2.Controls.Add(this.tileLeftSelector);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Panel2.Controls.Add(this.tileTypeSelector);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel2.Enabled = false;
			this.splitContainer1.Size = new System.Drawing.Size(786, 369);
			this.splitContainer1.SplitterDistance = 554;
			this.splitContainer1.TabIndex = 2;
			// 
			// layoutContainerPanel
			// 
			this.layoutContainerPanel.AutoScroll = true;
			this.layoutContainerPanel.Controls.Add(this.layoutPanel);
			this.layoutContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutContainerPanel.Location = new System.Drawing.Point(0, 0);
			this.layoutContainerPanel.Name = "layoutContainerPanel";
			this.layoutContainerPanel.Size = new System.Drawing.Size(554, 369);
			this.layoutContainerPanel.TabIndex = 1;
			// 
			// layoutPanel
			// 
			this.layoutPanel.Location = new System.Drawing.Point(0, 0);
			this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.layoutPanel.Name = "layoutPanel";
			this.layoutPanel.Size = new System.Drawing.Size(16384, 16384);
			this.layoutPanel.TabIndex = 0;
			this.layoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.layoutPanel_Paint);
			this.layoutPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.layoutPanel_MouseDown);
			this.layoutPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.layoutPanel_MouseMove);
			this.layoutPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.layoutPanel_MouseUp);
			// 
			// tileBottomSelector
			// 
			this.tileBottomSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tileBottomSelector.FormattingEnabled = true;
			this.tileBottomSelector.Items.AddRange(new object[] {
            "Empty",
            "Normal",
            "Hidden",
            "Secret",
            "Save",
            "Teleport",
            "Room"});
			this.tileBottomSelector.Location = new System.Drawing.Point(52, 261);
			this.tileBottomSelector.Name = "tileBottomSelector";
			this.tileBottomSelector.Size = new System.Drawing.Size(118, 21);
			this.tileBottomSelector.TabIndex = 8;
			this.tileBottomSelector.SelectedIndexChanged += new System.EventHandler(this.tileBottomSelector_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 264);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "Bottom:";
			// 
			// tileRightSelector
			// 
			this.tileRightSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tileRightSelector.FormattingEnabled = true;
			this.tileRightSelector.Items.AddRange(new object[] {
            "Empty",
            "Normal",
            "Hidden",
            "Secret",
            "Save",
            "Teleport",
            "Room"});
			this.tileRightSelector.Location = new System.Drawing.Point(52, 234);
			this.tileRightSelector.Name = "tileRightSelector";
			this.tileRightSelector.Size = new System.Drawing.Size(118, 21);
			this.tileRightSelector.TabIndex = 6;
			this.tileRightSelector.SelectedIndexChanged += new System.EventHandler(this.tileRightSelector_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 237);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Right:";
			// 
			// tileTopSelector
			// 
			this.tileTopSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tileTopSelector.FormattingEnabled = true;
			this.tileTopSelector.Items.AddRange(new object[] {
            "Empty",
            "Normal",
            "Hidden",
            "Secret",
            "Save",
            "Teleport",
            "Room"});
			this.tileTopSelector.Location = new System.Drawing.Point(52, 207);
			this.tileTopSelector.Name = "tileTopSelector";
			this.tileTopSelector.Size = new System.Drawing.Size(118, 21);
			this.tileTopSelector.TabIndex = 4;
			this.tileTopSelector.SelectedIndexChanged += new System.EventHandler(this.tileTopSelector_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 210);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Top:";
			// 
			// tileLeftSelector
			// 
			this.tileLeftSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tileLeftSelector.FormattingEnabled = true;
			this.tileLeftSelector.Items.AddRange(new object[] {
            "Empty",
            "Normal",
            "Hidden",
            "Secret",
            "Save",
            "Teleport",
            "Room"});
			this.tileLeftSelector.Location = new System.Drawing.Point(52, 180);
			this.tileLeftSelector.Name = "tileLeftSelector";
			this.tileLeftSelector.Size = new System.Drawing.Size(118, 21);
			this.tileLeftSelector.TabIndex = 2;
			this.tileLeftSelector.SelectedIndexChanged += new System.EventHandler(this.tileLeftSelector_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 183);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(28, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Left:";
			// 
			// tileTypeSelector
			// 
			this.tileTypeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tileTypeSelector.FormattingEnabled = true;
			this.tileTypeSelector.Items.AddRange(new object[] {
            "Empty",
            "Normal",
            "Hidden",
            "Secret",
            "Save",
            "Teleport",
            "Room"});
			this.tileTypeSelector.Location = new System.Drawing.Point(52, 153);
			this.tileTypeSelector.Name = "tileTypeSelector";
			this.tileTypeSelector.Size = new System.Drawing.Size(118, 21);
			this.tileTypeSelector.TabIndex = 0;
			this.tileTypeSelector.SelectedIndexChanged += new System.EventHandler(this.tileTypeSelector_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 156);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Type:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(222, 144);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Tiles";
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.roomTilePanel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(216, 125);
			this.panel1.TabIndex = 0;
			// 
			// roomTilePanel
			// 
			this.roomTilePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.roomTilePanel.Location = new System.Drawing.Point(0, 0);
			this.roomTilePanel.Name = "roomTilePanel";
			this.roomTilePanel.Size = new System.Drawing.Size(0, 0);
			this.roomTilePanel.TabIndex = 0;
			this.roomTilePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.roomTilePanel_Paint);
			this.roomTilePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.roomTilePanel_MouseClick);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layoutAreaSelect,
            this.moveWholeAreaButton});
			this.toolStrip1.Location = new System.Drawing.Point(3, 3);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(786, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// layoutAreaSelect
			// 
			this.layoutAreaSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.layoutAreaSelect.Name = "layoutAreaSelect";
			this.layoutAreaSelect.Size = new System.Drawing.Size(121, 25);
			this.layoutAreaSelect.SelectedIndexChanged += new System.EventHandler(this.layoutAreaSelect_SelectedIndexChanged);
			// 
			// moveWholeAreaButton
			// 
			this.moveWholeAreaButton.CheckOnClick = true;
			this.moveWholeAreaButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.moveWholeAreaButton.Name = "moveWholeAreaButton";
			this.moveWholeAreaButton.Size = new System.Drawing.Size(105, 22);
			this.moveWholeAreaButton.Text = "Move Whole Area";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.teleportPrevTSSelect);
			this.tabPage3.Controls.Add(this.showReverseCastleCheckBox);
			this.tabPage3.Controls.Add(this.panel2);
			this.tabPage3.Controls.Add(this.teleportYCoord);
			this.tabPage3.Controls.Add(this.label10);
			this.tabPage3.Controls.Add(this.teleportXCoord);
			this.tabPage3.Controls.Add(this.label9);
			this.tabPage3.Controls.Add(this.label8);
			this.tabPage3.Controls.Add(this.teleportRoomSelect);
			this.tabPage3.Controls.Add(this.label7);
			this.tabPage3.Controls.Add(this.teleportAreaSelect);
			this.tabPage3.Controls.Add(this.label6);
			this.tabPage3.Controls.Add(this.teleportListBox);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(792, 400);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Teleports";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// showReverseCastleCheckBox
			// 
			this.showReverseCastleCheckBox.AutoSize = true;
			this.showReverseCastleCheckBox.Location = new System.Drawing.Point(165, 142);
			this.showReverseCastleCheckBox.Name = "showReverseCastleCheckBox";
			this.showReverseCastleCheckBox.Size = new System.Drawing.Size(128, 17);
			this.showReverseCastleCheckBox.TabIndex = 12;
			this.showReverseCastleCheckBox.Text = "Show Reverse Castle";
			this.showReverseCastleCheckBox.UseVisualStyleBackColor = true;
			this.showReverseCastleCheckBox.CheckedChanged += new System.EventHandler(this.showReverseCastleCheckBox_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.AutoScroll = true;
			this.panel2.Controls.Add(this.teleportPreview);
			this.panel2.Location = new System.Drawing.Point(390, 6);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(396, 388);
			this.panel2.TabIndex = 11;
			// 
			// teleportPreview
			// 
			this.teleportPreview.Location = new System.Drawing.Point(0, 0);
			this.teleportPreview.Name = "teleportPreview";
			this.teleportPreview.Size = new System.Drawing.Size(200, 100);
			this.teleportPreview.TabIndex = 12;
			this.teleportPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.teleportPreview_Paint);
			this.teleportPreview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.teleportPreview_MouseClick);
			this.teleportPreview.MouseLeave += new System.EventHandler(this.teleportPreview_MouseLeave);
			this.teleportPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.teleportPreview_MouseMove);
			// 
			// teleportYCoord
			// 
			this.teleportYCoord.Location = new System.Drawing.Point(306, 111);
			this.teleportYCoord.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.teleportYCoord.Name = "teleportYCoord";
			this.teleportYCoord.Size = new System.Drawing.Size(78, 20);
			this.teleportYCoord.TabIndex = 10;
			this.teleportYCoord.ValueChanged += new System.EventHandler(this.teleportYCoord_ValueChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(165, 113);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(17, 13);
			this.label10.TabIndex = 9;
			this.label10.Text = "Y:";
			// 
			// teleportXCoord
			// 
			this.teleportXCoord.Location = new System.Drawing.Point(306, 85);
			this.teleportXCoord.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.teleportXCoord.Name = "teleportXCoord";
			this.teleportXCoord.Size = new System.Drawing.Size(78, 20);
			this.teleportXCoord.TabIndex = 8;
			this.teleportXCoord.ValueChanged += new System.EventHandler(this.teleportXCoord_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(165, 87);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(17, 13);
			this.label9.TabIndex = 7;
			this.label9.Text = "X:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(165, 61);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(69, 13);
			this.label8.TabIndex = 5;
			this.label8.Text = "Prev. Tileset:";
			// 
			// teleportRoomSelect
			// 
			this.teleportRoomSelect.Location = new System.Drawing.Point(306, 33);
			this.teleportRoomSelect.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.teleportRoomSelect.Name = "teleportRoomSelect";
			this.teleportRoomSelect.Size = new System.Drawing.Size(78, 20);
			this.teleportRoomSelect.TabIndex = 4;
			this.teleportRoomSelect.ValueChanged += new System.EventHandler(this.teleportRoomSelect_ValueChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(165, 35);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(38, 13);
			this.label7.TabIndex = 3;
			this.label7.Text = "Room:";
			// 
			// teleportAreaSelect
			// 
			this.teleportAreaSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.teleportAreaSelect.FormattingEnabled = true;
			this.teleportAreaSelect.Location = new System.Drawing.Point(203, 6);
			this.teleportAreaSelect.Name = "teleportAreaSelect";
			this.teleportAreaSelect.Size = new System.Drawing.Size(181, 21);
			this.teleportAreaSelect.TabIndex = 2;
			this.teleportAreaSelect.SelectedIndexChanged += new System.EventHandler(this.teleportAreaSelect_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(165, 9);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 13);
			this.label6.TabIndex = 1;
			this.label6.Text = "Area:";
			// 
			// teleportListBox
			// 
			this.teleportListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.teleportListBox.FormattingEnabled = true;
			this.teleportListBox.IntegralHeight = false;
			this.teleportListBox.Location = new System.Drawing.Point(6, 6);
			this.teleportListBox.Name = "teleportListBox";
			this.teleportListBox.Size = new System.Drawing.Size(153, 388);
			this.teleportListBox.TabIndex = 0;
			this.teleportListBox.SelectedIndexChanged += new System.EventHandler(this.teleportListBox_SelectedIndexChanged);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTeleportDestToolStripMenuItem,
            this.moveTeleportDestHereToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.contextMenuStrip1.Size = new System.Drawing.Size(204, 48);
			// 
			// editTeleportDestToolStripMenuItem
			// 
			this.editTeleportDestToolStripMenuItem.Name = "editTeleportDestToolStripMenuItem";
			this.editTeleportDestToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.editTeleportDestToolStripMenuItem.Text = "Edit Teleport Dest...";
			this.editTeleportDestToolStripMenuItem.Click += new System.EventHandler(this.editTeleportDestToolStripMenuItem_Click);
			// 
			// moveTeleportDestHereToolStripMenuItem
			// 
			this.moveTeleportDestHereToolStripMenuItem.Name = "moveTeleportDestHereToolStripMenuItem";
			this.moveTeleportDestHereToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			this.moveTeleportDestHereToolStripMenuItem.Text = "Move Teleport Dest Here";
			this.moveTeleportDestHereToolStripMenuItem.Click += new System.EventHandler(this.moveTeleportDestHereToolStripMenuItem_Click);
			// 
			// teleportPrevTSSelect
			// 
			this.teleportPrevTSSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.teleportPrevTSSelect.FormattingEnabled = true;
			this.teleportPrevTSSelect.Location = new System.Drawing.Point(240, 59);
			this.teleportPrevTSSelect.Name = "teleportPrevTSSelect";
			this.teleportPrevTSSelect.Size = new System.Drawing.Size(144, 21);
			this.teleportPrevTSSelect.TabIndex = 6;
			this.teleportPrevTSSelect.SelectedIndexChanged += new System.EventHandler(this.teleportPrevTSSelect_SelectedIndexChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "SotN Castle Editor";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.layoutContainerPanel.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.teleportYCoord)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.teleportXCoord)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.teleportRoomSelect)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem syncIdenticalRoomsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showGraphicsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clipGraphicsToMapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showBorderToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showOtherAreasToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showTeleportDestsToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.ToolStripComboBox zoomLevelSelector;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripComboBox layoutAreaSelect;
		private System.Windows.Forms.ToolStripButton moveWholeAreaButton;
		private System.Windows.Forms.Panel layoutContainerPanel;
		private BufferedPanel layoutPanel;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel roomTilePanel;
		private System.Windows.Forms.ComboBox tileTypeSelector;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox tileLeftSelector;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox tileBottomSelector;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox tileRightSelector;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox tileTopSelector;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox teleportListBox;
		private System.Windows.Forms.ComboBox teleportAreaSelect;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown teleportRoomSelect;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown teleportXCoord;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown teleportYCoord;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Panel panel2;
		private BufferedPanel teleportPreview;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem editTeleportDestToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveTeleportDestHereToolStripMenuItem;
		private System.Windows.Forms.CheckBox showReverseCastleCheckBox;
		private System.Windows.Forms.ComboBox teleportPrevTSSelect;
	}
}

