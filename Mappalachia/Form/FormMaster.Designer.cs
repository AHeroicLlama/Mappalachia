namespace Mappalachia
{
	partial class FormMaster
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMaster));
			this.menuStripMain = new System.Windows.Forms.MenuStrip();
			this.mapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.advancedModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.switchModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.layerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.layerMilitaryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.layerNWFlatwoodsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.layerNWMorgantownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.brightnessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.grayscaleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.interiorSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showFormIDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.plotSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.plotModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modeIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modeHeatmapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.plotIconSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.heatmapSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colorModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.monoColorModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.duoColorModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resolutionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resolution128MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resolution256MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resolution512MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resolution1024MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawVolumesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.userGuidesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.donateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gridViewSearchResults = new System.Windows.Forms.DataGridView();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.listViewFilterSignatures = new System.Windows.Forms.ListView();
			this.listViewFilterLockTypes = new System.Windows.Forms.ListView();
			this.buttonSelectAllSignature = new System.Windows.Forms.Button();
			this.buttonDeselectAllSignature = new System.Windows.Forms.Button();
			this.buttonDeselectAllLock = new System.Windows.Forms.Button();
			this.buttonSelectAllLock = new System.Windows.Forms.Button();
			this.gridViewLegend = new System.Windows.Forms.DataGridView();
			this.columnLegendGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnLegendEditorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnLegendDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnLegendLockType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.labelLegend = new System.Windows.Forms.Label();
			this.labelMinSpawnChance = new System.Windows.Forms.Label();
			this.numericUpDownNPCSpawnThreshold = new System.Windows.Forms.NumericUpDown();
			this.buttonSearchScrap = new System.Windows.Forms.Button();
			this.buttonSearchNPC = new System.Windows.Forms.Button();
			this.listBoxScrap = new System.Windows.Forms.ListBox();
			this.listBoxNPC = new System.Windows.Forms.ListBox();
			this.buttonDrawMap = new System.Windows.Forms.Button();
			this.labelSearchResults = new System.Windows.Forms.Label();
			this.tabControlSimpleNPCJunk = new System.Windows.Forms.TabControl();
			this.tabPageSimple = new System.Windows.Forms.TabPage();
			this.comboBoxCell = new System.Windows.Forms.ComboBox();
			this.groupBoxFilterByLockLevel = new System.Windows.Forms.GroupBox();
			this.groupBoxFilterByCategory = new System.Windows.Forms.GroupBox();
			this.buttonSelectRecommended = new System.Windows.Forms.Button();
			this.tabPageNpcSearch = new System.Windows.Forms.TabPage();
			this.tabPageScrapSearch = new System.Windows.Forms.TabPage();
			this.pictureBoxMapPreview = new System.Windows.Forms.PictureBox();
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.buttonRemoveFromLegend = new System.Windows.Forms.Button();
			this.buttonAddToLegend = new System.Windows.Forms.Button();
			this.toolTipControls = new System.Windows.Forms.ToolTip(this.components);
			this.progressBarMain = new System.Windows.Forms.ProgressBar();
			this.columnSearchFormID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchEditorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchLockLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchChance = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchLocationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnSearchIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridViewSearchResults)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewLegend)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNPCSpawnThreshold)).BeginInit();
			this.tabControlSimpleNPCJunk.SuspendLayout();
			this.tabPageSimple.SuspendLayout();
			this.groupBoxFilterByLockLevel.SuspendLayout();
			this.groupBoxFilterByCategory.SuspendLayout();
			this.tabPageNpcSearch.SuspendLayout();
			this.tabPageScrapSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapPreview)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
			this.splitContainerMain.Panel1.SuspendLayout();
			this.splitContainerMain.Panel2.SuspendLayout();
			this.splitContainerMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStripMain
			// 
			this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapMenuItem,
            this.searchSettingsMenuItem,
            this.plotSettingsMenuItem,
            this.helpMenuItem,
            this.donateMenuItem});
			this.menuStripMain.Location = new System.Drawing.Point(0, 0);
			this.menuStripMain.Name = "menuStripMain";
			this.menuStripMain.ShowItemToolTips = true;
			this.menuStripMain.Size = new System.Drawing.Size(1685, 24);
			this.menuStripMain.TabIndex = 0;
			// 
			// mapMenuItem
			// 
			this.mapMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMenuItem,
            this.advancedModeMenuItem,
            this.layerMenuItem,
            this.brightnessMenuItem,
            this.grayscaleMenuItem,
            this.exportToFileMenuItem,
            this.clearMenuItem,
            this.resetMenuItem});
			this.mapMenuItem.Name = "mapMenuItem";
			this.mapMenuItem.Size = new System.Drawing.Size(43, 20);
			this.mapMenuItem.Text = "Map";
			// 
			// viewMenuItem
			// 
			this.viewMenuItem.Name = "viewMenuItem";
			this.viewMenuItem.Size = new System.Drawing.Size(175, 22);
			this.viewMenuItem.Text = "View...";
			this.viewMenuItem.ToolTipText = "Open the map in the default image viewer.";
			this.viewMenuItem.Click += new System.EventHandler(this.Map_View);
			// 
			// advancedModeMenuItem
			// 
			this.advancedModeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchModeMenuItem});
			this.advancedModeMenuItem.Name = "advancedModeMenuItem";
			this.advancedModeMenuItem.Size = new System.Drawing.Size(175, 22);
			this.advancedModeMenuItem.Text = "Advanced Mode";
			// 
			// switchModeMenuItem
			// 
			this.switchModeMenuItem.Name = "switchModeMenuItem";
			this.switchModeMenuItem.Size = new System.Drawing.Size(180, 22);
			this.switchModeMenuItem.Text = "Switch to Cell mode";
			this.switchModeMenuItem.Click += new System.EventHandler(this.Map_SwitchMode);
			// 
			// layerMenuItem
			// 
			this.layerMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layerMilitaryMenuItem,
            this.layerNWFlatwoodsMenuItem,
            this.layerNWMorgantownMenuItem});
			this.layerMenuItem.Name = "layerMenuItem";
			this.layerMenuItem.Size = new System.Drawing.Size(175, 22);
			this.layerMenuItem.Text = "Layer";
			this.layerMenuItem.ToolTipText = "Add or remove a layer to the underlying map.";
			// 
			// layerMilitaryMenuItem
			// 
			this.layerMilitaryMenuItem.Name = "layerMilitaryMenuItem";
			this.layerMilitaryMenuItem.Size = new System.Drawing.Size(166, 22);
			this.layerMilitaryMenuItem.Text = "Military";
			this.layerMilitaryMenuItem.ToolTipText = "Change map to the version found on the Targeting Computer and in Train Stations.";
			this.layerMilitaryMenuItem.Click += new System.EventHandler(this.Map_Layer_Military);
			// 
			// layerNWFlatwoodsMenuItem
			// 
			this.layerNWFlatwoodsMenuItem.Name = "layerNWFlatwoodsMenuItem";
			this.layerNWFlatwoodsMenuItem.Size = new System.Drawing.Size(166, 22);
			this.layerNWFlatwoodsMenuItem.Text = "NW Flatwoods";
			this.layerNWFlatwoodsMenuItem.ToolTipText = "Add/Remove a layer for the Flatwoods Nuclear Winter map.";
			this.layerNWFlatwoodsMenuItem.Click += new System.EventHandler(this.Map_Layer_NWFlatwoods);
			// 
			// layerNWMorgantownMenuItem
			// 
			this.layerNWMorgantownMenuItem.Name = "layerNWMorgantownMenuItem";
			this.layerNWMorgantownMenuItem.Size = new System.Drawing.Size(166, 22);
			this.layerNWMorgantownMenuItem.Text = "NW Morgantown";
			this.layerNWMorgantownMenuItem.ToolTipText = "Add/Remove a layer for the Morgantown Nuclear Winter map.";
			this.layerNWMorgantownMenuItem.Click += new System.EventHandler(this.Map_Layer_NWMorgantown);
			// 
			// brightnessMenuItem
			// 
			this.brightnessMenuItem.Name = "brightnessMenuItem";
			this.brightnessMenuItem.Size = new System.Drawing.Size(175, 22);
			this.brightnessMenuItem.Text = "Adjust Brightness...";
			this.brightnessMenuItem.ToolTipText = "Adjust the brightness of the underlying map.";
			this.brightnessMenuItem.Click += new System.EventHandler(this.Map_Brightness);
			// 
			// grayscaleMenuItem
			// 
			this.grayscaleMenuItem.Name = "grayscaleMenuItem";
			this.grayscaleMenuItem.Size = new System.Drawing.Size(175, 22);
			this.grayscaleMenuItem.Text = "Grayscale";
			this.grayscaleMenuItem.ToolTipText = "Toggle if the underlying map image is in grayscale or full color.";
			this.grayscaleMenuItem.Click += new System.EventHandler(this.Map_Grayscale);
			// 
			// exportToFileMenuItem
			// 
			this.exportToFileMenuItem.Name = "exportToFileMenuItem";
			this.exportToFileMenuItem.Size = new System.Drawing.Size(175, 22);
			this.exportToFileMenuItem.Text = "Export To File...";
			this.exportToFileMenuItem.ToolTipText = "Save the current map image to a file.";
			this.exportToFileMenuItem.Click += new System.EventHandler(this.Map_Export);
			// 
			// clearMenuItem
			// 
			this.clearMenuItem.Name = "clearMenuItem";
			this.clearMenuItem.Size = new System.Drawing.Size(175, 22);
			this.clearMenuItem.Text = "Clear";
			this.clearMenuItem.ToolTipText = "Remove all mapped items from the legend and update the map.";
			this.clearMenuItem.Click += new System.EventHandler(this.Map_Clear);
			// 
			// resetMenuItem
			// 
			this.resetMenuItem.Name = "resetMenuItem";
			this.resetMenuItem.Size = new System.Drawing.Size(175, 22);
			this.resetMenuItem.Text = "Reset";
			this.resetMenuItem.ToolTipText = "Completely reset the map.";
			this.resetMenuItem.Click += new System.EventHandler(this.Map_Reset);
			// 
			// searchSettingsMenuItem
			// 
			this.searchSettingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interiorSearchMenuItem,
            this.showFormIDMenuItem});
			this.searchSettingsMenuItem.Name = "searchSettingsMenuItem";
			this.searchSettingsMenuItem.Size = new System.Drawing.Size(99, 20);
			this.searchSettingsMenuItem.Text = "Search Settings";
			// 
			// interiorSearchMenuItem
			// 
			this.interiorSearchMenuItem.Name = "interiorSearchMenuItem";
			this.interiorSearchMenuItem.Size = new System.Drawing.Size(155, 22);
			this.interiorSearchMenuItem.Text = "Search Interiors";
			this.interiorSearchMenuItem.ToolTipText = "Show search results from interiors in addition to just the surface world (such re" +
    "sults cannot be mapped).";
			this.interiorSearchMenuItem.Click += new System.EventHandler(this.Search_Interior);
			// 
			// showFormIDMenuItem
			// 
			this.showFormIDMenuItem.Name = "showFormIDMenuItem";
			this.showFormIDMenuItem.Size = new System.Drawing.Size(155, 22);
			this.showFormIDMenuItem.Text = "Show FormID";
			this.showFormIDMenuItem.ToolTipText = "Toggle visibility of the FormID column.";
			this.showFormIDMenuItem.Click += new System.EventHandler(this.Search_FormID);
			// 
			// plotSettingsMenuItem
			// 
			this.plotSettingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotModeMenuItem,
            this.plotIconSettingsMenuItem,
            this.heatmapSettingsMenuItem,
            this.drawVolumesMenuItem});
			this.plotSettingsMenuItem.Name = "plotSettingsMenuItem";
			this.plotSettingsMenuItem.Size = new System.Drawing.Size(85, 20);
			this.plotSettingsMenuItem.Text = "Plot Settings";
			// 
			// plotModeMenuItem
			// 
			this.plotModeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modeIconMenuItem,
            this.modeHeatmapMenuItem});
			this.plotModeMenuItem.Name = "plotModeMenuItem";
			this.plotModeMenuItem.Size = new System.Drawing.Size(175, 22);
			this.plotModeMenuItem.Text = "Plot Mode";
			this.plotModeMenuItem.ToolTipText = "Change the way Mappalachia represents items on the map.";
			// 
			// modeIconMenuItem
			// 
			this.modeIconMenuItem.Name = "modeIconMenuItem";
			this.modeIconMenuItem.Size = new System.Drawing.Size(123, 22);
			this.modeIconMenuItem.Text = "Icon";
			this.modeIconMenuItem.ToolTipText = "Use icons to represent locations of multiple items on the map.";
			this.modeIconMenuItem.Click += new System.EventHandler(this.Plot_Mode_Icon);
			// 
			// modeHeatmapMenuItem
			// 
			this.modeHeatmapMenuItem.Name = "modeHeatmapMenuItem";
			this.modeHeatmapMenuItem.Size = new System.Drawing.Size(123, 22);
			this.modeHeatmapMenuItem.Text = "Heatmap";
			this.modeHeatmapMenuItem.ToolTipText = "Use a heatmap to represent the density distribution of items.";
			this.modeHeatmapMenuItem.Click += new System.EventHandler(this.Plot_Mode_Heatmap);
			// 
			// plotIconSettingsMenuItem
			// 
			this.plotIconSettingsMenuItem.Name = "plotIconSettingsMenuItem";
			this.plotIconSettingsMenuItem.Size = new System.Drawing.Size(175, 22);
			this.plotIconSettingsMenuItem.Text = "Plot Icon Settings...";
			this.plotIconSettingsMenuItem.ToolTipText = "Adjust the appearance of the icons used for plotting items on the map.";
			this.plotIconSettingsMenuItem.Click += new System.EventHandler(this.Plot_PlotIconSettings);
			// 
			// heatmapSettingsMenuItem
			// 
			this.heatmapSettingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorModeMenuItem,
            this.resolutionMenuItem});
			this.heatmapSettingsMenuItem.Name = "heatmapSettingsMenuItem";
			this.heatmapSettingsMenuItem.Size = new System.Drawing.Size(175, 22);
			this.heatmapSettingsMenuItem.Text = "Heatmap Settings";
			this.heatmapSettingsMenuItem.ToolTipText = "Adjust settings related to Heatmap mode.";
			// 
			// colorModeMenuItem
			// 
			this.colorModeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monoColorModeMenuItem,
            this.duoColorModeMenuItem});
			this.colorModeMenuItem.Name = "colorModeMenuItem";
			this.colorModeMenuItem.Size = new System.Drawing.Size(137, 22);
			this.colorModeMenuItem.Text = "Color Mode";
			this.colorModeMenuItem.ToolTipText = "Change the heatmap between mono (one color) and duo (two colors).";
			// 
			// monoColorModeMenuItem
			// 
			this.monoColorModeMenuItem.Name = "monoColorModeMenuItem";
			this.monoColorModeMenuItem.Size = new System.Drawing.Size(106, 22);
			this.monoColorModeMenuItem.Text = "Mono";
			this.monoColorModeMenuItem.ToolTipText = "Mono: Everything mapped in heatmap mode will be represented by one color.";
			this.monoColorModeMenuItem.Click += new System.EventHandler(this.Plot_HeatMap_ColorMode_Mono);
			// 
			// duoColorModeMenuItem
			// 
			this.duoColorModeMenuItem.Name = "duoColorModeMenuItem";
			this.duoColorModeMenuItem.Size = new System.Drawing.Size(106, 22);
			this.duoColorModeMenuItem.Text = "Duo";
			this.duoColorModeMenuItem.ToolTipText = "Duo: Use two colors to compare two groups in heatmap mode by using odd or even nu" +
    "mbers in the legend group field.";
			this.duoColorModeMenuItem.Click += new System.EventHandler(this.Plot_HeatMap_ColorMode_Duo);
			// 
			// resolutionMenuItem
			// 
			this.resolutionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolution128MenuItem,
            this.resolution256MenuItem,
            this.resolution512MenuItem,
            this.resolution1024MenuItem});
			this.resolutionMenuItem.Name = "resolutionMenuItem";
			this.resolutionMenuItem.Size = new System.Drawing.Size(137, 22);
			this.resolutionMenuItem.Text = "Resolution";
			this.resolutionMenuItem.ToolTipText = "Change the resolution, and hence precision of the heatmap.";
			// 
			// resolution128MenuItem
			// 
			this.resolution128MenuItem.Name = "resolution128MenuItem";
			this.resolution128MenuItem.Size = new System.Drawing.Size(98, 22);
			this.resolution128MenuItem.Text = "128";
			this.resolution128MenuItem.ToolTipText = "128x128 squares for the heatmap.";
			this.resolution128MenuItem.Click += new System.EventHandler(this.Plot_HeatMap_Resolution_128);
			// 
			// resolution256MenuItem
			// 
			this.resolution256MenuItem.Name = "resolution256MenuItem";
			this.resolution256MenuItem.Size = new System.Drawing.Size(98, 22);
			this.resolution256MenuItem.Text = "256";
			this.resolution256MenuItem.ToolTipText = "256x256 squares for the heatmap.";
			this.resolution256MenuItem.Click += new System.EventHandler(this.Plot_HeatMap_Resolution_256);
			// 
			// resolution512MenuItem
			// 
			this.resolution512MenuItem.Name = "resolution512MenuItem";
			this.resolution512MenuItem.Size = new System.Drawing.Size(98, 22);
			this.resolution512MenuItem.Text = "512";
			this.resolution512MenuItem.ToolTipText = "512x512 squares for the heatmap.";
			this.resolution512MenuItem.Click += new System.EventHandler(this.Plot_HeatMap_Resolution_512);
			// 
			// resolution1024MenuItem
			// 
			this.resolution1024MenuItem.Name = "resolution1024MenuItem";
			this.resolution1024MenuItem.Size = new System.Drawing.Size(98, 22);
			this.resolution1024MenuItem.Text = "1024";
			this.resolution1024MenuItem.ToolTipText = "1024x1024 squares for the heatmap.";
			this.resolution1024MenuItem.Click += new System.EventHandler(this.Plot_HeatMap_Resolution_1024);
			// 
			// drawVolumesMenuItem
			// 
			this.drawVolumesMenuItem.Name = "drawVolumesMenuItem";
			this.drawVolumesMenuItem.Size = new System.Drawing.Size(175, 22);
			this.drawVolumesMenuItem.Text = "Draw Volumes";
			this.drawVolumesMenuItem.ToolTipText = "(Where applicable, in Icon Mode) In-game volumes such as triggers/activators have" +
    " their boundaries drawn instead of a plot icon.";
			this.drawVolumesMenuItem.Click += new System.EventHandler(this.Plot_DrawVolumes);
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenuItem,
            this.checkForUpdatesMenuItem,
            this.userGuidesMenuItem});
			this.helpMenuItem.Name = "helpMenuItem";
			this.helpMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpMenuItem.Text = "Help";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Name = "aboutMenuItem";
			this.aboutMenuItem.Size = new System.Drawing.Size(171, 22);
			this.aboutMenuItem.Text = "About...";
			this.aboutMenuItem.ToolTipText = "Information about Mappalachia.";
			this.aboutMenuItem.Click += new System.EventHandler(this.Help_About);
			// 
			// checkForUpdatesMenuItem
			// 
			this.checkForUpdatesMenuItem.Name = "checkForUpdatesMenuItem";
			this.checkForUpdatesMenuItem.Size = new System.Drawing.Size(171, 22);
			this.checkForUpdatesMenuItem.Text = "Check for Updates";
			this.checkForUpdatesMenuItem.ToolTipText = "Automatically check if a new version is available.";
			this.checkForUpdatesMenuItem.Click += new System.EventHandler(this.Help_CheckForUpdates);
			// 
			// userGuidesMenuItem
			// 
			this.userGuidesMenuItem.Name = "userGuidesMenuItem";
			this.userGuidesMenuItem.Size = new System.Drawing.Size(171, 22);
			this.userGuidesMenuItem.Text = "User Guides";
			this.userGuidesMenuItem.ToolTipText = "View the user guide documentation online.";
			this.userGuidesMenuItem.Click += new System.EventHandler(this.Help_UserGuides);
			// 
			// donateMenuItem
			// 
			this.donateMenuItem.Name = "donateMenuItem";
			this.donateMenuItem.Size = new System.Drawing.Size(131, 20);
			this.donateMenuItem.Text = "Donate to the Author";
			this.donateMenuItem.ToolTipText = "Say thanks!";
			this.donateMenuItem.Click += new System.EventHandler(this.Donate);
			// 
			// gridViewSearchResults
			// 
			this.gridViewSearchResults.AllowUserToAddRows = false;
			this.gridViewSearchResults.AllowUserToDeleteRows = false;
			this.gridViewSearchResults.AllowUserToOrderColumns = true;
			this.gridViewSearchResults.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
			this.gridViewSearchResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.gridViewSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridViewSearchResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.gridViewSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridViewSearchResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			this.gridViewSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridViewSearchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSearchFormID,
            this.columnSearchEditorID,
            this.columnSearchDisplayName,
            this.columnSearchCategory,
            this.columnSearchLockLevel,
            this.columnSearchChance,
            this.columnSearchAmount,
            this.columnSearchLocation,
            this.columnSearchLocationID,
            this.columnSearchIndex});
			this.gridViewSearchResults.Location = new System.Drawing.Point(8, 269);
			this.gridViewSearchResults.Name = "gridViewSearchResults";
			this.gridViewSearchResults.ReadOnly = true;
			this.gridViewSearchResults.RowHeadersVisible = false;
			this.gridViewSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridViewSearchResults.Size = new System.Drawing.Size(798, 359);
			this.gridViewSearchResults.TabIndex = 2;
			this.gridViewSearchResults.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewSearchResults_CellMouseEnter);
			// 
			// buttonSearch
			// 
			this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonSearch.Location = new System.Drawing.Point(185, 17);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(122, 23);
			this.buttonSearch.TabIndex = 3;
			this.buttonSearch.Text = "Simple Search";
			this.toolTipControls.SetToolTip(this.buttonSearch, "Search for the given text.");
			this.buttonSearch.UseVisualStyleBackColor = true;
			this.buttonSearch.Click += new System.EventHandler(this.ButtonSearchSimple);
			// 
			// textBoxSearch
			// 
			this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBoxSearch.Location = new System.Drawing.Point(6, 19);
			this.textBoxSearch.MaxLength = 100;
			this.textBoxSearch.Name = "textBoxSearch";
			this.textBoxSearch.Size = new System.Drawing.Size(173, 20);
			this.textBoxSearch.TabIndex = 2;
			this.toolTipControls.SetToolTip(this.textBoxSearch, "The name to be searched for.");
			this.textBoxSearch.WordWrap = false;
			// 
			// listViewFilterSignatures
			// 
			this.listViewFilterSignatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewFilterSignatures.BackColor = System.Drawing.SystemColors.ControlDark;
			this.listViewFilterSignatures.CheckBoxes = true;
			this.listViewFilterSignatures.HideSelection = false;
			this.listViewFilterSignatures.Location = new System.Drawing.Point(6, 19);
			this.listViewFilterSignatures.Name = "listViewFilterSignatures";
			this.listViewFilterSignatures.ShowItemToolTips = true;
			this.listViewFilterSignatures.Size = new System.Drawing.Size(317, 116);
			this.listViewFilterSignatures.TabIndex = 0;
			this.listViewFilterSignatures.UseCompatibleStateImageBehavior = false;
			this.listViewFilterSignatures.View = System.Windows.Forms.View.SmallIcon;
			this.listViewFilterSignatures.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewFilterSignatures_SelectionChanged);
			// 
			// listViewFilterLockTypes
			// 
			this.listViewFilterLockTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewFilterLockTypes.BackColor = System.Drawing.SystemColors.ControlDark;
			this.listViewFilterLockTypes.CheckBoxes = true;
			this.listViewFilterLockTypes.HideSelection = false;
			this.listViewFilterLockTypes.Location = new System.Drawing.Point(6, 19);
			this.listViewFilterLockTypes.Name = "listViewFilterLockTypes";
			this.listViewFilterLockTypes.ShowItemToolTips = true;
			this.listViewFilterLockTypes.Size = new System.Drawing.Size(225, 116);
			this.listViewFilterLockTypes.TabIndex = 0;
			this.listViewFilterLockTypes.UseCompatibleStateImageBehavior = false;
			this.listViewFilterLockTypes.View = System.Windows.Forms.View.SmallIcon;
			this.listViewFilterLockTypes.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewFilterLockTypes_SelectionChanged);
			// 
			// buttonSelectAllSignature
			// 
			this.buttonSelectAllSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSelectAllSignature.Location = new System.Drawing.Point(6, 141);
			this.buttonSelectAllSignature.Name = "buttonSelectAllSignature";
			this.buttonSelectAllSignature.Size = new System.Drawing.Size(75, 23);
			this.buttonSelectAllSignature.TabIndex = 1;
			this.buttonSelectAllSignature.Text = "Select All";
			this.toolTipControls.SetToolTip(this.buttonSelectAllSignature, "Enable all categories, therefore filtering none.");
			this.buttonSelectAllSignature.UseVisualStyleBackColor = true;
			this.buttonSelectAllSignature.Click += new System.EventHandler(this.ButtonSelectAllSignature);
			// 
			// buttonDeselectAllSignature
			// 
			this.buttonDeselectAllSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDeselectAllSignature.Location = new System.Drawing.Point(215, 141);
			this.buttonDeselectAllSignature.Name = "buttonDeselectAllSignature";
			this.buttonDeselectAllSignature.Size = new System.Drawing.Size(75, 23);
			this.buttonDeselectAllSignature.TabIndex = 2;
			this.buttonDeselectAllSignature.Text = "Deselect All";
			this.toolTipControls.SetToolTip(this.buttonDeselectAllSignature, "Remove all categories (used to then select just one).");
			this.buttonDeselectAllSignature.UseVisualStyleBackColor = true;
			this.buttonDeselectAllSignature.Click += new System.EventHandler(this.ButtonDeselectAllSignature);
			// 
			// buttonDeselectAllLock
			// 
			this.buttonDeselectAllLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDeselectAllLock.Location = new System.Drawing.Point(89, 141);
			this.buttonDeselectAllLock.Name = "buttonDeselectAllLock";
			this.buttonDeselectAllLock.Size = new System.Drawing.Size(75, 23);
			this.buttonDeselectAllLock.TabIndex = 2;
			this.buttonDeselectAllLock.Text = "Deselect All";
			this.toolTipControls.SetToolTip(this.buttonDeselectAllLock, "Remove all lock levels (used to then select just one).");
			this.buttonDeselectAllLock.UseVisualStyleBackColor = true;
			this.buttonDeselectAllLock.Click += new System.EventHandler(this.ButtonDeselectAllLock);
			// 
			// buttonSelectAllLock
			// 
			this.buttonSelectAllLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSelectAllLock.Location = new System.Drawing.Point(6, 141);
			this.buttonSelectAllLock.Name = "buttonSelectAllLock";
			this.buttonSelectAllLock.Size = new System.Drawing.Size(77, 23);
			this.buttonSelectAllLock.TabIndex = 1;
			this.buttonSelectAllLock.Text = "Select All";
			this.toolTipControls.SetToolTip(this.buttonSelectAllLock, "Enable all lock levels, therefore filtering none.");
			this.buttonSelectAllLock.UseVisualStyleBackColor = true;
			this.buttonSelectAllLock.Click += new System.EventHandler(this.ButtonSelectAllLock);
			// 
			// gridViewLegend
			// 
			this.gridViewLegend.AllowUserToAddRows = false;
			this.gridViewLegend.AllowUserToDeleteRows = false;
			this.gridViewLegend.AllowUserToOrderColumns = true;
			this.gridViewLegend.AllowUserToResizeRows = false;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
			this.gridViewLegend.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
			this.gridViewLegend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridViewLegend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridViewLegend.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			this.gridViewLegend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridViewLegend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLegendGroup,
            this.columnLegendEditorID,
            this.columnLegendDisplayName,
            this.columnLegendLockType});
			this.gridViewLegend.Location = new System.Drawing.Point(6, 663);
			this.gridViewLegend.Name = "gridViewLegend";
			this.gridViewLegend.RowHeadersVisible = false;
			this.gridViewLegend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridViewLegend.Size = new System.Drawing.Size(800, 173);
			this.gridViewLegend.TabIndex = 3;
			this.gridViewLegend.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewLegend_CellEndEdit);
			this.gridViewLegend.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewLegend_CellMouseEnter);
			// 
			// columnLegendGroup
			// 
			this.columnLegendGroup.HeaderText = "Legend Group";
			this.columnLegendGroup.Name = "columnLegendGroup";
			this.columnLegendGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// columnLegendEditorID
			// 
			this.columnLegendEditorID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnLegendEditorID.FillWeight = 400F;
			this.columnLegendEditorID.HeaderText = "Technical Name";
			this.columnLegendEditorID.Name = "columnLegendEditorID";
			this.columnLegendEditorID.ReadOnly = true;
			this.columnLegendEditorID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// columnLegendDisplayName
			// 
			this.columnLegendDisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnLegendDisplayName.FillWeight = 400F;
			this.columnLegendDisplayName.HeaderText = "Display Name";
			this.columnLegendDisplayName.Name = "columnLegendDisplayName";
			this.columnLegendDisplayName.ReadOnly = true;
			this.columnLegendDisplayName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// columnLegendLockType
			// 
			this.columnLegendLockType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnLegendLockType.HeaderText = "Lock Type";
			this.columnLegendLockType.Name = "columnLegendLockType";
			this.columnLegendLockType.ReadOnly = true;
			this.columnLegendLockType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.columnLegendLockType.Visible = false;
			// 
			// labelLegend
			// 
			this.labelLegend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelLegend.AutoSize = true;
			this.labelLegend.Location = new System.Drawing.Point(5, 647);
			this.labelLegend.Name = "labelLegend";
			this.labelLegend.Size = new System.Drawing.Size(64, 13);
			this.labelLegend.TabIndex = 1;
			this.labelLegend.Text = "Items to plot";
			// 
			// labelMinSpawnChance
			// 
			this.labelMinSpawnChance.AutoSize = true;
			this.labelMinSpawnChance.Location = new System.Drawing.Point(6, 179);
			this.labelMinSpawnChance.Name = "labelMinSpawnChance";
			this.labelMinSpawnChance.Size = new System.Drawing.Size(141, 13);
			this.labelMinSpawnChance.TabIndex = 5;
			this.labelMinSpawnChance.Text = "Minimum Spawn Chance (%)";
			// 
			// numericUpDownNPCSpawnThreshold
			// 
			this.numericUpDownNPCSpawnThreshold.Location = new System.Drawing.Point(6, 195);
			this.numericUpDownNPCSpawnThreshold.Name = "numericUpDownNPCSpawnThreshold";
			this.numericUpDownNPCSpawnThreshold.Size = new System.Drawing.Size(94, 20);
			this.numericUpDownNPCSpawnThreshold.TabIndex = 1;
			this.numericUpDownNPCSpawnThreshold.ValueChanged += new System.EventHandler(this.NumericUpDownNPCSpawnThreshold_ValueChanged);
			this.numericUpDownNPCSpawnThreshold.Enter += new System.EventHandler(this.NumericUpDownNPCSpawnThreshold_MouseEnter);
			// 
			// buttonSearchScrap
			// 
			this.buttonSearchScrap.Location = new System.Drawing.Point(6, 192);
			this.buttonSearchScrap.Name = "buttonSearchScrap";
			this.buttonSearchScrap.Size = new System.Drawing.Size(90, 23);
			this.buttonSearchScrap.TabIndex = 1;
			this.buttonSearchScrap.Text = "Scrap Search";
			this.buttonSearchScrap.UseVisualStyleBackColor = true;
			this.buttonSearchScrap.Click += new System.EventHandler(this.ButtonSearchScrap);
			// 
			// buttonSearchNPC
			// 
			this.buttonSearchNPC.Location = new System.Drawing.Point(106, 193);
			this.buttonSearchNPC.Name = "buttonSearchNPC";
			this.buttonSearchNPC.Size = new System.Drawing.Size(90, 23);
			this.buttonSearchNPC.TabIndex = 2;
			this.buttonSearchNPC.Text = "NPC Search";
			this.buttonSearchNPC.UseVisualStyleBackColor = true;
			this.buttonSearchNPC.Click += new System.EventHandler(this.ButtonSearchNPC);
			// 
			// listBoxScrap
			// 
			this.listBoxScrap.FormattingEnabled = true;
			this.listBoxScrap.Location = new System.Drawing.Point(6, 6);
			this.listBoxScrap.Name = "listBoxScrap";
			this.listBoxScrap.Size = new System.Drawing.Size(190, 173);
			this.listBoxScrap.TabIndex = 0;
			this.listBoxScrap.SelectedIndexChanged += new System.EventHandler(this.ListBoxScrap_SelectedIndexChanged);
			// 
			// listBoxNPC
			// 
			this.listBoxNPC.FormattingEnabled = true;
			this.listBoxNPC.Location = new System.Drawing.Point(6, 6);
			this.listBoxNPC.Name = "listBoxNPC";
			this.listBoxNPC.Size = new System.Drawing.Size(190, 160);
			this.listBoxNPC.TabIndex = 0;
			this.listBoxNPC.Enter += new System.EventHandler(this.ListBoxNPC_MouseEnter);
			// 
			// buttonDrawMap
			// 
			this.buttonDrawMap.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonDrawMap.Location = new System.Drawing.Point(337, 841);
			this.buttonDrawMap.Name = "buttonDrawMap";
			this.buttonDrawMap.Size = new System.Drawing.Size(136, 26);
			this.buttonDrawMap.TabIndex = 4;
			this.buttonDrawMap.Text = "Update Map";
			this.toolTipControls.SetToolTip(this.buttonDrawMap, "Re-draw the map by plotting every item in the \'items to plot\' list.");
			this.buttonDrawMap.UseVisualStyleBackColor = true;
			this.buttonDrawMap.Click += new System.EventHandler(this.ButtonDrawMap);
			// 
			// labelSearchResults
			// 
			this.labelSearchResults.AutoSize = true;
			this.labelSearchResults.Location = new System.Drawing.Point(5, 253);
			this.labelSearchResults.Name = "labelSearchResults";
			this.labelSearchResults.Size = new System.Drawing.Size(79, 13);
			this.labelSearchResults.TabIndex = 0;
			this.labelSearchResults.Text = "Search Results";
			// 
			// tabControlSimpleNPCJunk
			// 
			this.tabControlSimpleNPCJunk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlSimpleNPCJunk.Controls.Add(this.tabPageSimple);
			this.tabControlSimpleNPCJunk.Controls.Add(this.tabPageNpcSearch);
			this.tabControlSimpleNPCJunk.Controls.Add(this.tabPageScrapSearch);
			this.tabControlSimpleNPCJunk.Location = new System.Drawing.Point(8, 3);
			this.tabControlSimpleNPCJunk.Name = "tabControlSimpleNPCJunk";
			this.tabControlSimpleNPCJunk.SelectedIndex = 0;
			this.tabControlSimpleNPCJunk.ShowToolTips = true;
			this.tabControlSimpleNPCJunk.Size = new System.Drawing.Size(798, 247);
			this.tabControlSimpleNPCJunk.TabIndex = 1;
			this.tabControlSimpleNPCJunk.SelectedIndexChanged += new System.EventHandler(this.TabControlMain_SelectedIndexChanged);
			// 
			// tabPageSimple
			// 
			this.tabPageSimple.AutoScroll = true;
			this.tabPageSimple.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabPageSimple.Controls.Add(this.comboBoxCell);
			this.tabPageSimple.Controls.Add(this.groupBoxFilterByLockLevel);
			this.tabPageSimple.Controls.Add(this.groupBoxFilterByCategory);
			this.tabPageSimple.Controls.Add(this.buttonSearch);
			this.tabPageSimple.Controls.Add(this.textBoxSearch);
			this.tabPageSimple.Location = new System.Drawing.Point(4, 22);
			this.tabPageSimple.Name = "tabPageSimple";
			this.tabPageSimple.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSimple.Size = new System.Drawing.Size(790, 221);
			this.tabPageSimple.TabIndex = 0;
			this.tabPageSimple.Text = "Simple Search";
			// 
			// comboBoxCell
			// 
			this.comboBoxCell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCell.FormattingEnabled = true;
			this.comboBoxCell.Location = new System.Drawing.Point(469, 6);
			this.comboBoxCell.Name = "comboBoxCell";
			this.comboBoxCell.Size = new System.Drawing.Size(315, 21);
			this.comboBoxCell.TabIndex = 4;
			this.comboBoxCell.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCell_SelectedIndexChanged);
			// 
			// groupBoxFilterByLockLevel
			// 
			this.groupBoxFilterByLockLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBoxFilterByLockLevel.Controls.Add(this.listViewFilterLockTypes);
			this.groupBoxFilterByLockLevel.Controls.Add(this.buttonDeselectAllLock);
			this.groupBoxFilterByLockLevel.Controls.Add(this.buttonSelectAllLock);
			this.groupBoxFilterByLockLevel.Location = new System.Drawing.Point(341, 45);
			this.groupBoxFilterByLockLevel.Name = "groupBoxFilterByLockLevel";
			this.groupBoxFilterByLockLevel.Size = new System.Drawing.Size(237, 170);
			this.groupBoxFilterByLockLevel.TabIndex = 1;
			this.groupBoxFilterByLockLevel.TabStop = false;
			this.groupBoxFilterByLockLevel.Text = "Filter by lock level";
			// 
			// groupBoxFilterByCategory
			// 
			this.groupBoxFilterByCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBoxFilterByCategory.Controls.Add(this.buttonSelectRecommended);
			this.groupBoxFilterByCategory.Controls.Add(this.listViewFilterSignatures);
			this.groupBoxFilterByCategory.Controls.Add(this.buttonDeselectAllSignature);
			this.groupBoxFilterByCategory.Controls.Add(this.buttonSelectAllSignature);
			this.groupBoxFilterByCategory.Location = new System.Drawing.Point(6, 45);
			this.groupBoxFilterByCategory.Name = "groupBoxFilterByCategory";
			this.groupBoxFilterByCategory.Size = new System.Drawing.Size(329, 170);
			this.groupBoxFilterByCategory.TabIndex = 0;
			this.groupBoxFilterByCategory.TabStop = false;
			this.groupBoxFilterByCategory.Text = "Filter by category";
			// 
			// buttonSelectRecommended
			// 
			this.buttonSelectRecommended.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSelectRecommended.Location = new System.Drawing.Point(87, 141);
			this.buttonSelectRecommended.Name = "buttonSelectRecommended";
			this.buttonSelectRecommended.Size = new System.Drawing.Size(122, 23);
			this.buttonSelectRecommended.TabIndex = 3;
			this.buttonSelectRecommended.Text = "Select Recommended";
			this.toolTipControls.SetToolTip(this.buttonSelectRecommended, "Refine results by selecting only the most commonly used categories.");
			this.buttonSelectRecommended.UseVisualStyleBackColor = true;
			this.buttonSelectRecommended.Click += new System.EventHandler(this.ButtonSelectRecommendedSignature);
			// 
			// tabPageNpcSearch
			// 
			this.tabPageNpcSearch.AutoScroll = true;
			this.tabPageNpcSearch.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabPageNpcSearch.Controls.Add(this.buttonSearchNPC);
			this.tabPageNpcSearch.Controls.Add(this.numericUpDownNPCSpawnThreshold);
			this.tabPageNpcSearch.Controls.Add(this.listBoxNPC);
			this.tabPageNpcSearch.Controls.Add(this.labelMinSpawnChance);
			this.tabPageNpcSearch.Location = new System.Drawing.Point(4, 22);
			this.tabPageNpcSearch.Name = "tabPageNpcSearch";
			this.tabPageNpcSearch.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageNpcSearch.Size = new System.Drawing.Size(790, 221);
			this.tabPageNpcSearch.TabIndex = 1;
			this.tabPageNpcSearch.Text = "NPC Search";
			// 
			// tabPageScrapSearch
			// 
			this.tabPageScrapSearch.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabPageScrapSearch.Controls.Add(this.listBoxScrap);
			this.tabPageScrapSearch.Controls.Add(this.buttonSearchScrap);
			this.tabPageScrapSearch.Location = new System.Drawing.Point(4, 22);
			this.tabPageScrapSearch.Name = "tabPageScrapSearch";
			this.tabPageScrapSearch.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageScrapSearch.Size = new System.Drawing.Size(790, 221);
			this.tabPageScrapSearch.TabIndex = 2;
			this.tabPageScrapSearch.Text = "Scrap Search";
			// 
			// pictureBoxMapPreview
			// 
			this.pictureBoxMapPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxMapPreview.Location = new System.Drawing.Point(4, 3);
			this.pictureBoxMapPreview.Name = "pictureBoxMapPreview";
			this.pictureBoxMapPreview.Size = new System.Drawing.Size(863, 865);
			this.pictureBoxMapPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxMapPreview.TabIndex = 5;
			this.pictureBoxMapPreview.TabStop = false;
			this.pictureBoxMapPreview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MapPreview_MouseDoubleClick);
			this.pictureBoxMapPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPreview_MouseDown);
			this.pictureBoxMapPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPreview_MouseMove);
			// 
			// splitContainerMain
			// 
			this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerMain.Location = new System.Drawing.Point(0, 27);
			this.splitContainerMain.Name = "splitContainerMain";
			// 
			// splitContainerMain.Panel1
			// 
			this.splitContainerMain.Panel1.Controls.Add(this.buttonRemoveFromLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.buttonAddToLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.tabControlSimpleNPCJunk);
			this.splitContainerMain.Panel1.Controls.Add(this.gridViewSearchResults);
			this.splitContainerMain.Panel1.Controls.Add(this.gridViewLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.labelSearchResults);
			this.splitContainerMain.Panel1.Controls.Add(this.labelLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.buttonDrawMap);
			this.splitContainerMain.Panel1MinSize = 250;
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.pictureBoxMapPreview);
			this.splitContainerMain.Panel2MinSize = 0;
			this.splitContainerMain.Size = new System.Drawing.Size(1685, 871);
			this.splitContainerMain.SplitterDistance = 811;
			this.splitContainerMain.TabIndex = 6;
			// 
			// buttonRemoveFromLegend
			// 
			this.buttonRemoveFromLegend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonRemoveFromLegend.Location = new System.Drawing.Point(408, 634);
			this.buttonRemoveFromLegend.Name = "buttonRemoveFromLegend";
			this.buttonRemoveFromLegend.Size = new System.Drawing.Size(124, 23);
			this.buttonRemoveFromLegend.TabIndex = 6;
			this.buttonRemoveFromLegend.Text = "Remove from Map";
			this.toolTipControls.SetToolTip(this.buttonRemoveFromLegend, "Remove the selected item(s) from the \'items to plot\' list.");
			this.buttonRemoveFromLegend.UseVisualStyleBackColor = true;
			this.buttonRemoveFromLegend.Click += new System.EventHandler(this.ButtonRemoveFromLegend);
			// 
			// buttonAddToLegend
			// 
			this.buttonAddToLegend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonAddToLegend.Location = new System.Drawing.Point(278, 634);
			this.buttonAddToLegend.Name = "buttonAddToLegend";
			this.buttonAddToLegend.Size = new System.Drawing.Size(124, 23);
			this.buttonAddToLegend.TabIndex = 5;
			this.buttonAddToLegend.Text = "Add to Map";
			this.toolTipControls.SetToolTip(this.buttonAddToLegend, "Add the selected item(s) from the search results onto the \'items to plot\' list.");
			this.buttonAddToLegend.UseVisualStyleBackColor = true;
			this.buttonAddToLegend.Click += new System.EventHandler(this.ButtonAddToLegend);
			// 
			// progressBarMain
			// 
			this.progressBarMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarMain.Location = new System.Drawing.Point(12, 904);
			this.progressBarMain.Maximum = 10000;
			this.progressBarMain.Name = "progressBarMain";
			this.progressBarMain.Size = new System.Drawing.Size(1661, 19);
			this.progressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBarMain.TabIndex = 7;
			// 
			// columnSearchFormID
			// 
			this.columnSearchFormID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.columnSearchFormID.DefaultCellStyle = dataGridViewCellStyle2;
			this.columnSearchFormID.HeaderText = "FormID";
			this.columnSearchFormID.Name = "columnSearchFormID";
			this.columnSearchFormID.ReadOnly = true;
			this.columnSearchFormID.ToolTipText = "A unique reference to this entity. Useful for dataminers and modders.";
			this.columnSearchFormID.Visible = false;
			// 
			// columnSearchEditorID
			// 
			this.columnSearchEditorID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchEditorID.FillWeight = 250F;
			this.columnSearchEditorID.HeaderText = "Technical Name";
			this.columnSearchEditorID.Name = "columnSearchEditorID";
			this.columnSearchEditorID.ReadOnly = true;
			this.columnSearchEditorID.ToolTipText = "The developer\'s technical name for the entity.";
			// 
			// columnSearchDisplayName
			// 
			this.columnSearchDisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchDisplayName.FillWeight = 180F;
			this.columnSearchDisplayName.HeaderText = "Display Name";
			this.columnSearchDisplayName.Name = "columnSearchDisplayName";
			this.columnSearchDisplayName.ReadOnly = true;
			this.columnSearchDisplayName.ToolTipText = "The name displayed in-game where applicable.";
			// 
			// columnSearchCategory
			// 
			this.columnSearchCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchCategory.HeaderText = "Category";
			this.columnSearchCategory.Name = "columnSearchCategory";
			this.columnSearchCategory.ReadOnly = true;
			this.columnSearchCategory.ToolTipText = "A classification for this entity.";
			// 
			// columnSearchLockLevel
			// 
			this.columnSearchLockLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchLockLevel.HeaderText = "Lock Type";
			this.columnSearchLockLevel.Name = "columnSearchLockLevel";
			this.columnSearchLockLevel.ReadOnly = true;
			this.columnSearchLockLevel.ToolTipText = "The Level or type of the lock, if any";
			this.columnSearchLockLevel.Visible = false;
			// 
			// columnSearchChance
			// 
			this.columnSearchChance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchChance.FillWeight = 90F;
			this.columnSearchChance.HeaderText = "Spawn Chance (%)";
			this.columnSearchChance.Name = "columnSearchChance";
			this.columnSearchChance.ReadOnly = true;
			// 
			// columnSearchAmount
			// 
			this.columnSearchAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchAmount.FillWeight = 70F;
			this.columnSearchAmount.HeaderText = "Amount";
			this.columnSearchAmount.Name = "columnSearchAmount";
			this.columnSearchAmount.ReadOnly = true;
			this.columnSearchAmount.ToolTipText = "The amount of instances which can be found in the listed location.";
			// 
			// columnSearchLocation
			// 
			this.columnSearchLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnSearchLocation.FillWeight = 170F;
			this.columnSearchLocation.HeaderText = "Location";
			this.columnSearchLocation.Name = "columnSearchLocation";
			this.columnSearchLocation.ReadOnly = true;
			this.columnSearchLocation.ToolTipText = "The location where these items can be found. Either the surface world (Apppalachi" +
    "a) or an interior \'cell\'.";
			// 
			// columnSearchLocationID
			// 
			this.columnSearchLocationID.HeaderText = "LocationID";
			this.columnSearchLocationID.Name = "columnSearchLocationID";
			this.columnSearchLocationID.ReadOnly = true;
			this.columnSearchLocationID.Visible = false;
			// 
			// columnSearchIndex
			// 
			this.columnSearchIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.columnSearchIndex.HeaderText = "Index";
			this.columnSearchIndex.Name = "columnSearchIndex";
			this.columnSearchIndex.ReadOnly = true;
			this.columnSearchIndex.Visible = false;
			// 
			// FormMaster
			// 
			this.AcceptButton = this.buttonSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.ClientSize = new System.Drawing.Size(1685, 935);
			this.Controls.Add(this.progressBarMain);
			this.Controls.Add(this.splitContainerMain);
			this.Controls.Add(this.menuStripMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStripMain;
			this.MinimumSize = new System.Drawing.Size(822, 730);
			this.Name = "FormMaster";
			this.Text = "Mappalachia";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
			this.menuStripMain.ResumeLayout(false);
			this.menuStripMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridViewSearchResults)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewLegend)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNPCSpawnThreshold)).EndInit();
			this.tabControlSimpleNPCJunk.ResumeLayout(false);
			this.tabPageSimple.ResumeLayout(false);
			this.tabPageSimple.PerformLayout();
			this.groupBoxFilterByLockLevel.ResumeLayout(false);
			this.groupBoxFilterByCategory.ResumeLayout(false);
			this.tabPageNpcSearch.ResumeLayout(false);
			this.tabPageNpcSearch.PerformLayout();
			this.tabPageScrapSearch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxMapPreview)).EndInit();
			this.splitContainerMain.Panel1.ResumeLayout(false);
			this.splitContainerMain.Panel1.PerformLayout();
			this.splitContainerMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
			this.splitContainerMain.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStripMain;
		private System.Windows.Forms.ToolStripMenuItem mapMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToFileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
		private System.Windows.Forms.DataGridView gridViewSearchResults;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.TextBox textBoxSearch;
		private System.Windows.Forms.ListView listViewFilterSignatures;
		private System.Windows.Forms.ListView listViewFilterLockTypes;
		private System.Windows.Forms.Button buttonSelectAllSignature;
		private System.Windows.Forms.Button buttonDeselectAllSignature;
		private System.Windows.Forms.Button buttonDeselectAllLock;
		private System.Windows.Forms.Button buttonSelectAllLock;
		private System.Windows.Forms.DataGridView gridViewLegend;
		private System.Windows.Forms.Label labelLegend;
		private System.Windows.Forms.ToolStripMenuItem layerMenuItem;
		private System.Windows.Forms.ToolStripMenuItem layerMilitaryMenuItem;
		private System.Windows.Forms.ToolStripMenuItem layerNWFlatwoodsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem layerNWMorgantownMenuItem;
		private System.Windows.Forms.ToolStripMenuItem brightnessMenuItem;
		private System.Windows.Forms.Button buttonSearchScrap;
		private System.Windows.Forms.Button buttonSearchNPC;
		private System.Windows.Forms.ListBox listBoxScrap;
		private System.Windows.Forms.ListBox listBoxNPC;
		private System.Windows.Forms.Label labelMinSpawnChance;
		private System.Windows.Forms.NumericUpDown numericUpDownNPCSpawnThreshold;
		private System.Windows.Forms.ToolStripMenuItem searchSettingsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem interiorSearchMenuItem;
		private System.Windows.Forms.ToolStripMenuItem userGuidesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showFormIDMenuItem;
		private System.Windows.Forms.Button buttonDrawMap;
		private System.Windows.Forms.ToolStripMenuItem clearMenuItem;
		private System.Windows.Forms.Label labelSearchResults;
		private System.Windows.Forms.ToolStripMenuItem plotSettingsMenuItem;
		private System.Windows.Forms.TabControl tabControlSimpleNPCJunk;
		private System.Windows.Forms.TabPage tabPageSimple;
		private System.Windows.Forms.TabPage tabPageNpcSearch;
		private System.Windows.Forms.GroupBox groupBoxFilterByLockLevel;
		private System.Windows.Forms.GroupBox groupBoxFilterByCategory;
		private System.Windows.Forms.PictureBox pictureBoxMapPreview;
		private System.Windows.Forms.SplitContainer splitContainerMain;
		private System.Windows.Forms.Button buttonRemoveFromLegend;
		private System.Windows.Forms.Button buttonAddToLegend;
		private System.Windows.Forms.ToolStripMenuItem plotIconSettingsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawVolumesMenuItem;
		private System.Windows.Forms.ToolTip toolTipControls;
		private System.Windows.Forms.ToolStripMenuItem plotModeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modeIconMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modeHeatmapMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendGroup;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendEditorID;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendDisplayName;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendLockType;
		private System.Windows.Forms.ToolStripMenuItem donateMenuItem;
		private System.Windows.Forms.ToolStripMenuItem heatmapSettingsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem colorModeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem monoColorModeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem duoColorModeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resolutionMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resolution128MenuItem;
		private System.Windows.Forms.ToolStripMenuItem resolution256MenuItem;
		private System.Windows.Forms.ToolStripMenuItem resolution512MenuItem;
		private System.Windows.Forms.ToolStripMenuItem resolution1024MenuItem;
		private System.Windows.Forms.ToolStripMenuItem grayscaleMenuItem;
		private System.Windows.Forms.ProgressBar progressBarMain;
		private System.Windows.Forms.TabPage tabPageScrapSearch;
		private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem advancedModeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem switchModeMenuItem;
		private System.Windows.Forms.ComboBox comboBoxCell;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchFormID;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchEditorID;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchDisplayName;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchCategory;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchLockLevel;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchChance;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchAmount;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchLocation;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchLocationID;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnSearchIndex;
		private System.Windows.Forms.Button buttonSelectRecommended;
	}
}

