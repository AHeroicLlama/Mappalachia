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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMaster));
			this.menuStripMain = new System.Windows.Forms.MenuStrip();
			this.mapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.advancedModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cellModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.listViewFilterSignatures = new System.Windows.Forms.ListView();
			this.listViewFilterLockTypes = new System.Windows.Forms.ListView();
			this.buttonSelectAllSignature = new System.Windows.Forms.Button();
			this.buttonDeselectAllSignature = new System.Windows.Forms.Button();
			this.buttonDeselectAllLock = new System.Windows.Forms.Button();
			this.buttonSelectAllLock = new System.Windows.Forms.Button();
			this.gridViewLegend = new System.Windows.Forms.DataGridView();
			this.labelLegend = new System.Windows.Forms.Label();
			this.labelMinSpawnChance = new System.Windows.Forms.Label();
			this.numericUpDownNPCSpawnThreshold = new System.Windows.Forms.NumericUpDown();
			this.buttonSearchScrap = new System.Windows.Forms.Button();
			this.buttonSearchNPC = new System.Windows.Forms.Button();
			this.listBoxScrap = new System.Windows.Forms.ListBox();
			this.listBoxNPC = new System.Windows.Forms.ListBox();
			this.buttonDrawMap = new System.Windows.Forms.Button();
			this.labelSearchResults = new System.Windows.Forms.Label();
			this.tabControlStandardNPCJunk = new System.Windows.Forms.TabControl();
			this.tabPageStandard = new System.Windows.Forms.TabPage();
			this.groupBoxCellModeSettings = new System.Windows.Forms.GroupBox();
			this.checkBoxCellDrawOutline = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelMaxHeight = new System.Windows.Forms.Label();
			this.labelMinHeight = new System.Windows.Forms.Label();
			this.numericMaxZ = new System.Windows.Forms.NumericUpDown();
			this.numericMinz = new System.Windows.Forms.NumericUpDown();
			this.buttonCellHeightDistribution = new System.Windows.Forms.Button();
			this.labelCell = new System.Windows.Forms.Label();
			this.comboBoxCell = new System.Windows.Forms.ComboBox();
			this.groupBoxFilterByLockLevel = new System.Windows.Forms.GroupBox();
			this.groupBoxFilterByCategory = new System.Windows.Forms.GroupBox();
			this.buttonSelectRecommended = new System.Windows.Forms.Button();
			this.tabPageNpcScrapSearch = new System.Windows.Forms.TabPage();
			this.groupBoxScrapSearch = new System.Windows.Forms.GroupBox();
			this.groupBoxNPCSearch = new System.Windows.Forms.GroupBox();
			this.pictureBoxMapPreview = new System.Windows.Forms.PictureBox();
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.checkBoxAddAsGroup = new System.Windows.Forms.CheckBox();
			this.buttonRemoveFromLegend = new System.Windows.Forms.Button();
			this.buttonAddToLegend = new System.Windows.Forms.Button();
			this.toolTipControls = new System.Windows.Forms.ToolTip(this.components);
			this.progressBarMain = new System.Windows.Forms.ProgressBar();
			this.columnLegendGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.columnLegendDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.menuStripMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridViewSearchResults)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewLegend)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNPCSpawnThreshold)).BeginInit();
			this.tabControlStandardNPCJunk.SuspendLayout();
			this.tabPageStandard.SuspendLayout();
			this.groupBoxCellModeSettings.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMinz)).BeginInit();
			this.groupBoxFilterByLockLevel.SuspendLayout();
			this.groupBoxFilterByCategory.SuspendLayout();
			this.tabPageNpcScrapSearch.SuspendLayout();
			this.groupBoxScrapSearch.SuspendLayout();
			this.groupBoxNPCSearch.SuspendLayout();
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
            this.cellModeMenuItem});
			this.advancedModeMenuItem.Name = "advancedModeMenuItem";
			this.advancedModeMenuItem.Size = new System.Drawing.Size(175, 22);
			this.advancedModeMenuItem.Text = "Advanced Modes";
			this.advancedModeMenuItem.ToolTipText = "Switch to advanced mapping modes.";
			// 
			// cellModeMenuItem
			// 
			this.cellModeMenuItem.Name = "cellModeMenuItem";
			this.cellModeMenuItem.Size = new System.Drawing.Size(128, 22);
			this.cellModeMenuItem.Text = "Cell mode";
			this.cellModeMenuItem.ToolTipText = "A mode specifically for plotting individual cells.";
			this.cellModeMenuItem.Click += new System.EventHandler(this.Map_CellMode);
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
			this.plotModeMenuItem.Size = new System.Drawing.Size(180, 22);
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
			this.plotIconSettingsMenuItem.Size = new System.Drawing.Size(180, 22);
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
			this.heatmapSettingsMenuItem.Size = new System.Drawing.Size(180, 22);
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
			this.drawVolumesMenuItem.Size = new System.Drawing.Size(180, 22);
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
			this.gridViewSearchResults.Location = new System.Drawing.Point(8, 355);
			this.gridViewSearchResults.Name = "gridViewSearchResults";
			this.gridViewSearchResults.ReadOnly = true;
			this.gridViewSearchResults.RowHeadersVisible = false;
			this.gridViewSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridViewSearchResults.Size = new System.Drawing.Size(798, 273);
			this.gridViewSearchResults.TabIndex = 1;
			this.gridViewSearchResults.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewSearchResults_CellMouseEnter);
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
			// buttonSearch
			// 
			this.buttonSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonSearch.Location = new System.Drawing.Point(185, 4);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(122, 23);
			this.buttonSearch.TabIndex = 3;
			this.buttonSearch.Text = "Standard Search";
			this.toolTipControls.SetToolTip(this.buttonSearch, "Search for the given text.");
			this.buttonSearch.UseVisualStyleBackColor = true;
			this.buttonSearch.Click += new System.EventHandler(this.ButtonSearchStandard);
			// 
			// textBoxSearch
			// 
			this.textBoxSearch.Location = new System.Drawing.Point(6, 6);
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
			this.listViewFilterSignatures.Size = new System.Drawing.Size(317, 199);
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
			this.listViewFilterLockTypes.Size = new System.Drawing.Size(158, 199);
			this.listViewFilterLockTypes.TabIndex = 0;
			this.listViewFilterLockTypes.UseCompatibleStateImageBehavior = false;
			this.listViewFilterLockTypes.View = System.Windows.Forms.View.SmallIcon;
			this.listViewFilterLockTypes.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewFilterLockTypes_SelectionChanged);
			// 
			// buttonSelectAllSignature
			// 
			this.buttonSelectAllSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSelectAllSignature.Location = new System.Drawing.Point(22, 224);
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
			this.buttonDeselectAllSignature.Location = new System.Drawing.Point(231, 224);
			this.buttonDeselectAllSignature.Name = "buttonDeselectAllSignature";
			this.buttonDeselectAllSignature.Size = new System.Drawing.Size(75, 23);
			this.buttonDeselectAllSignature.TabIndex = 3;
			this.buttonDeselectAllSignature.Text = "Deselect All";
			this.toolTipControls.SetToolTip(this.buttonDeselectAllSignature, "Remove all categories (used to then select just one).");
			this.buttonDeselectAllSignature.UseVisualStyleBackColor = true;
			this.buttonDeselectAllSignature.Click += new System.EventHandler(this.ButtonDeselectAllSignature);
			// 
			// buttonDeselectAllLock
			// 
			this.buttonDeselectAllLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDeselectAllLock.Location = new System.Drawing.Point(89, 224);
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
			this.buttonSelectAllLock.Location = new System.Drawing.Point(6, 224);
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
            this.columnLegendDisplayName});
			this.gridViewLegend.Location = new System.Drawing.Point(6, 663);
			this.gridViewLegend.Name = "gridViewLegend";
			this.gridViewLegend.RowHeadersVisible = false;
			this.gridViewLegend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridViewLegend.Size = new System.Drawing.Size(800, 173);
			this.gridViewLegend.TabIndex = 7;
			this.gridViewLegend.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewLegend_CellEndEdit);
			this.gridViewLegend.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewLegend_CellMouseEnter);
			// 
			// labelLegend
			// 
			this.labelLegend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelLegend.AutoSize = true;
			this.labelLegend.Location = new System.Drawing.Point(9, 647);
			this.labelLegend.Name = "labelLegend";
			this.labelLegend.Size = new System.Drawing.Size(64, 13);
			this.labelLegend.TabIndex = 0;
			this.labelLegend.Text = "Items to plot";
			// 
			// labelMinSpawnChance
			// 
			this.labelMinSpawnChance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMinSpawnChance.AutoSize = true;
			this.labelMinSpawnChance.Location = new System.Drawing.Point(3, 234);
			this.labelMinSpawnChance.Name = "labelMinSpawnChance";
			this.labelMinSpawnChance.Size = new System.Drawing.Size(141, 13);
			this.labelMinSpawnChance.TabIndex = 5;
			this.labelMinSpawnChance.Text = "Minimum Spawn Chance (%)";
			this.toolTipControls.SetToolTip(this.labelMinSpawnChance, "The minimum spawn chance to show results for.");
			// 
			// numericUpDownNPCSpawnThreshold
			// 
			this.numericUpDownNPCSpawnThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numericUpDownNPCSpawnThreshold.Location = new System.Drawing.Point(6, 250);
			this.numericUpDownNPCSpawnThreshold.Name = "numericUpDownNPCSpawnThreshold";
			this.numericUpDownNPCSpawnThreshold.Size = new System.Drawing.Size(94, 20);
			this.numericUpDownNPCSpawnThreshold.TabIndex = 1;
			this.toolTipControls.SetToolTip(this.numericUpDownNPCSpawnThreshold, "The minimum spawn chance to show results for.");
			this.numericUpDownNPCSpawnThreshold.ValueChanged += new System.EventHandler(this.NumericUpDownNPCSpawnThreshold_ValueChanged);
			this.numericUpDownNPCSpawnThreshold.Enter += new System.EventHandler(this.NumericUpDownNPCSpawnThreshold_MouseEnter);
			// 
			// buttonSearchScrap
			// 
			this.buttonSearchScrap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSearchScrap.Location = new System.Drawing.Point(56, 247);
			this.buttonSearchScrap.Name = "buttonSearchScrap";
			this.buttonSearchScrap.Size = new System.Drawing.Size(90, 23);
			this.buttonSearchScrap.TabIndex = 1;
			this.buttonSearchScrap.Text = "Scrap Search";
			this.toolTipControls.SetToolTip(this.buttonSearchScrap, "Search for junk containing the selected scrap type.");
			this.buttonSearchScrap.UseVisualStyleBackColor = true;
			this.buttonSearchScrap.Click += new System.EventHandler(this.ButtonSearchScrap);
			// 
			// buttonSearchNPC
			// 
			this.buttonSearchNPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSearchNPC.Location = new System.Drawing.Point(106, 248);
			this.buttonSearchNPC.Name = "buttonSearchNPC";
			this.buttonSearchNPC.Size = new System.Drawing.Size(90, 23);
			this.buttonSearchNPC.TabIndex = 2;
			this.buttonSearchNPC.Text = "NPC Search";
			this.toolTipControls.SetToolTip(this.buttonSearchNPC, "Search for the selected NPC.");
			this.buttonSearchNPC.UseVisualStyleBackColor = true;
			this.buttonSearchNPC.Click += new System.EventHandler(this.ButtonSearchNPC);
			// 
			// listBoxScrap
			// 
			this.listBoxScrap.FormattingEnabled = true;
			this.listBoxScrap.Location = new System.Drawing.Point(6, 19);
			this.listBoxScrap.Name = "listBoxScrap";
			this.listBoxScrap.Size = new System.Drawing.Size(191, 212);
			this.listBoxScrap.TabIndex = 0;
			this.listBoxScrap.SelectedIndexChanged += new System.EventHandler(this.ListBoxScrap_SelectedIndexChanged);
			// 
			// listBoxNPC
			// 
			this.listBoxNPC.FormattingEnabled = true;
			this.listBoxNPC.Location = new System.Drawing.Point(6, 19);
			this.listBoxNPC.Name = "listBoxNPC";
			this.listBoxNPC.Size = new System.Drawing.Size(190, 212);
			this.listBoxNPC.TabIndex = 0;
			this.listBoxNPC.Enter += new System.EventHandler(this.ListBoxNPC_MouseEnter);
			// 
			// buttonDrawMap
			// 
			this.buttonDrawMap.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonDrawMap.Location = new System.Drawing.Point(337, 841);
			this.buttonDrawMap.Name = "buttonDrawMap";
			this.buttonDrawMap.Size = new System.Drawing.Size(136, 26);
			this.buttonDrawMap.TabIndex = 6;
			this.buttonDrawMap.Text = "Update Map";
			this.toolTipControls.SetToolTip(this.buttonDrawMap, "Re-draw the map by plotting every item in the \'items to plot\' list.");
			this.buttonDrawMap.UseVisualStyleBackColor = true;
			this.buttonDrawMap.Click += new System.EventHandler(this.ButtonDrawMap);
			// 
			// labelSearchResults
			// 
			this.labelSearchResults.AutoSize = true;
			this.labelSearchResults.Location = new System.Drawing.Point(9, 339);
			this.labelSearchResults.Name = "labelSearchResults";
			this.labelSearchResults.Size = new System.Drawing.Size(79, 13);
			this.labelSearchResults.TabIndex = 0;
			this.labelSearchResults.Text = "Search Results";
			// 
			// tabControlStandardNPCJunk
			// 
			this.tabControlStandardNPCJunk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlStandardNPCJunk.Controls.Add(this.tabPageStandard);
			this.tabControlStandardNPCJunk.Controls.Add(this.tabPageNpcScrapSearch);
			this.tabControlStandardNPCJunk.Location = new System.Drawing.Point(8, 3);
			this.tabControlStandardNPCJunk.Name = "tabControlStandardNPCJunk";
			this.tabControlStandardNPCJunk.SelectedIndex = 0;
			this.tabControlStandardNPCJunk.ShowToolTips = true;
			this.tabControlStandardNPCJunk.Size = new System.Drawing.Size(798, 333);
			this.tabControlStandardNPCJunk.TabIndex = 0;
			this.tabControlStandardNPCJunk.SelectedIndexChanged += new System.EventHandler(this.TabControlMain_SelectedIndexChanged);
			// 
			// tabPageStandard
			// 
			this.tabPageStandard.AutoScroll = true;
			this.tabPageStandard.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabPageStandard.Controls.Add(this.groupBoxCellModeSettings);
			this.tabPageStandard.Controls.Add(this.groupBoxFilterByLockLevel);
			this.tabPageStandard.Controls.Add(this.groupBoxFilterByCategory);
			this.tabPageStandard.Controls.Add(this.buttonSearch);
			this.tabPageStandard.Controls.Add(this.textBoxSearch);
			this.tabPageStandard.Location = new System.Drawing.Point(4, 22);
			this.tabPageStandard.Name = "tabPageStandard";
			this.tabPageStandard.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageStandard.Size = new System.Drawing.Size(790, 307);
			this.tabPageStandard.TabIndex = 0;
			this.tabPageStandard.Text = "Standard Search";
			// 
			// groupBoxCellModeSettings
			// 
			this.groupBoxCellModeSettings.Controls.Add(this.checkBoxCellDrawOutline);
			this.groupBoxCellModeSettings.Controls.Add(this.groupBox1);
			this.groupBoxCellModeSettings.Controls.Add(this.labelCell);
			this.groupBoxCellModeSettings.Controls.Add(this.comboBoxCell);
			this.groupBoxCellModeSettings.Location = new System.Drawing.Point(517, 32);
			this.groupBoxCellModeSettings.Name = "groupBoxCellModeSettings";
			this.groupBoxCellModeSettings.Size = new System.Drawing.Size(267, 196);
			this.groupBoxCellModeSettings.TabIndex = 5;
			this.groupBoxCellModeSettings.TabStop = false;
			this.groupBoxCellModeSettings.Text = "Cell Mode Settings";
			this.toolTipControls.SetToolTip(this.groupBoxCellModeSettings, "Settings specific to Cell Mode.");
			this.groupBoxCellModeSettings.Visible = false;
			// 
			// checkBoxCellDrawOutline
			// 
			this.checkBoxCellDrawOutline.AutoSize = true;
			this.checkBoxCellDrawOutline.Checked = true;
			this.checkBoxCellDrawOutline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCellDrawOutline.Location = new System.Drawing.Point(10, 62);
			this.checkBoxCellDrawOutline.Name = "checkBoxCellDrawOutline";
			this.checkBoxCellDrawOutline.Size = new System.Drawing.Size(87, 17);
			this.checkBoxCellDrawOutline.TabIndex = 8;
			this.checkBoxCellDrawOutline.Text = "Draw Outline";
			this.toolTipControls.SetToolTip(this.checkBoxCellDrawOutline, "Renders a subtle outline of all items in the cell as a background to visualize th" +
        "e cell structure.");
			this.checkBoxCellDrawOutline.UseVisualStyleBackColor = true;
			this.checkBoxCellDrawOutline.CheckedChanged += new System.EventHandler(this.CheckBoxCellDrawOutline_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.labelMaxHeight);
			this.groupBox1.Controls.Add(this.labelMinHeight);
			this.groupBox1.Controls.Add(this.numericMaxZ);
			this.groupBox1.Controls.Add(this.numericMinz);
			this.groupBox1.Controls.Add(this.buttonCellHeightDistribution);
			this.groupBox1.Location = new System.Drawing.Point(6, 91);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(255, 99);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Height Cropping";
			this.toolTipControls.SetToolTip(this.groupBox1, "Allows you to constrain the map to certain height bands, for example to map a giv" +
        "en floor.");
			// 
			// labelMaxHeight
			// 
			this.labelMaxHeight.AutoSize = true;
			this.labelMaxHeight.Location = new System.Drawing.Point(147, 53);
			this.labelMaxHeight.Name = "labelMaxHeight";
			this.labelMaxHeight.Size = new System.Drawing.Size(102, 13);
			this.labelMaxHeight.TabIndex = 10;
			this.labelMaxHeight.Text = "Maximum Height (%)";
			this.toolTipControls.SetToolTip(this.labelMaxHeight, "Select the maximum height of objects from the cell which to map.");
			// 
			// labelMinHeight
			// 
			this.labelMinHeight.AutoSize = true;
			this.labelMinHeight.Location = new System.Drawing.Point(6, 53);
			this.labelMinHeight.Name = "labelMinHeight";
			this.labelMinHeight.Size = new System.Drawing.Size(99, 13);
			this.labelMinHeight.TabIndex = 9;
			this.labelMinHeight.Text = "Minimum Height (%)";
			this.toolTipControls.SetToolTip(this.labelMinHeight, "Select the minimum height of objects from the cell which to map.");
			// 
			// numericMaxZ
			// 
			this.numericMaxZ.Location = new System.Drawing.Point(150, 69);
			this.numericMaxZ.Name = "numericMaxZ";
			this.numericMaxZ.Size = new System.Drawing.Size(70, 20);
			this.numericMaxZ.TabIndex = 8;
			this.toolTipControls.SetToolTip(this.numericMaxZ, "Select the maximum height of objects from the cell which to map.");
			this.numericMaxZ.ValueChanged += new System.EventHandler(this.NumericMaxY_ValueChanged);
			this.numericMaxZ.Enter += new System.EventHandler(this.NumericMaxY_Enter);
			this.numericMaxZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NumericMaxY_MouseDown);
			// 
			// numericMinz
			// 
			this.numericMinz.Location = new System.Drawing.Point(35, 69);
			this.numericMinz.Name = "numericMinz";
			this.numericMinz.Size = new System.Drawing.Size(70, 20);
			this.numericMinz.TabIndex = 7;
			this.toolTipControls.SetToolTip(this.numericMinz, "Select the minimum height of objects from the cell which to map.");
			this.numericMinz.ValueChanged += new System.EventHandler(this.NumericMinY_ValueChanged);
			this.numericMinz.Enter += new System.EventHandler(this.NumericMinY_Enter);
			this.numericMinz.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NumericMinY_MouseDown);
			// 
			// buttonCellHeightDistribution
			// 
			this.buttonCellHeightDistribution.Location = new System.Drawing.Point(49, 19);
			this.buttonCellHeightDistribution.Name = "buttonCellHeightDistribution";
			this.buttonCellHeightDistribution.Size = new System.Drawing.Size(152, 23);
			this.buttonCellHeightDistribution.TabIndex = 6;
			this.buttonCellHeightDistribution.Text = "Visualize Height Distribution";
			this.toolTipControls.SetToolTip(this.buttonCellHeightDistribution, "Displays a visualization of the distribution of entities in the selected cell by " +
        "height.");
			this.buttonCellHeightDistribution.UseVisualStyleBackColor = true;
			this.buttonCellHeightDistribution.Click += new System.EventHandler(this.ButtonCellHeightDistribution_Click);
			// 
			// labelCell
			// 
			this.labelCell.AutoSize = true;
			this.labelCell.Location = new System.Drawing.Point(7, 19);
			this.labelCell.Name = "labelCell";
			this.labelCell.Size = new System.Drawing.Size(69, 13);
			this.labelCell.TabIndex = 5;
			this.labelCell.Text = "Selected Cell";
			// 
			// comboBoxCell
			// 
			this.comboBoxCell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCell.FormattingEnabled = true;
			this.comboBoxCell.Location = new System.Drawing.Point(6, 35);
			this.comboBoxCell.Name = "comboBoxCell";
			this.comboBoxCell.Size = new System.Drawing.Size(255, 21);
			this.comboBoxCell.TabIndex = 4;
			this.toolTipControls.SetToolTip(this.comboBoxCell, "Select the specific cell to search and plot within.");
			this.comboBoxCell.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCell_SelectedIndexChanged);
			// 
			// groupBoxFilterByLockLevel
			// 
			this.groupBoxFilterByLockLevel.Controls.Add(this.listViewFilterLockTypes);
			this.groupBoxFilterByLockLevel.Controls.Add(this.buttonDeselectAllLock);
			this.groupBoxFilterByLockLevel.Controls.Add(this.buttonSelectAllLock);
			this.groupBoxFilterByLockLevel.Location = new System.Drawing.Point(341, 32);
			this.groupBoxFilterByLockLevel.Name = "groupBoxFilterByLockLevel";
			this.groupBoxFilterByLockLevel.Size = new System.Drawing.Size(170, 253);
			this.groupBoxFilterByLockLevel.TabIndex = 1;
			this.groupBoxFilterByLockLevel.TabStop = false;
			this.groupBoxFilterByLockLevel.Text = "Filter by lock level";
			// 
			// groupBoxFilterByCategory
			// 
			this.groupBoxFilterByCategory.Controls.Add(this.buttonSelectRecommended);
			this.groupBoxFilterByCategory.Controls.Add(this.listViewFilterSignatures);
			this.groupBoxFilterByCategory.Controls.Add(this.buttonDeselectAllSignature);
			this.groupBoxFilterByCategory.Controls.Add(this.buttonSelectAllSignature);
			this.groupBoxFilterByCategory.Location = new System.Drawing.Point(6, 32);
			this.groupBoxFilterByCategory.Name = "groupBoxFilterByCategory";
			this.groupBoxFilterByCategory.Size = new System.Drawing.Size(329, 253);
			this.groupBoxFilterByCategory.TabIndex = 0;
			this.groupBoxFilterByCategory.TabStop = false;
			this.groupBoxFilterByCategory.Text = "Filter by category";
			// 
			// buttonSelectRecommended
			// 
			this.buttonSelectRecommended.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSelectRecommended.Location = new System.Drawing.Point(103, 224);
			this.buttonSelectRecommended.Name = "buttonSelectRecommended";
			this.buttonSelectRecommended.Size = new System.Drawing.Size(122, 23);
			this.buttonSelectRecommended.TabIndex = 2;
			this.buttonSelectRecommended.Text = "Select Recommended";
			this.toolTipControls.SetToolTip(this.buttonSelectRecommended, "Refine results by selecting only the most commonly used categories.");
			this.buttonSelectRecommended.UseVisualStyleBackColor = true;
			this.buttonSelectRecommended.Click += new System.EventHandler(this.ButtonSelectRecommendedSignature);
			// 
			// tabPageNpcScrapSearch
			// 
			this.tabPageNpcScrapSearch.AutoScroll = true;
			this.tabPageNpcScrapSearch.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabPageNpcScrapSearch.Controls.Add(this.groupBoxScrapSearch);
			this.tabPageNpcScrapSearch.Controls.Add(this.groupBoxNPCSearch);
			this.tabPageNpcScrapSearch.Location = new System.Drawing.Point(4, 22);
			this.tabPageNpcScrapSearch.Name = "tabPageNpcScrapSearch";
			this.tabPageNpcScrapSearch.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageNpcScrapSearch.Size = new System.Drawing.Size(790, 307);
			this.tabPageNpcScrapSearch.TabIndex = 1;
			this.tabPageNpcScrapSearch.Text = "NPC or Scrap Search";
			// 
			// groupBoxScrapSearch
			// 
			this.groupBoxScrapSearch.Controls.Add(this.listBoxScrap);
			this.groupBoxScrapSearch.Controls.Add(this.buttonSearchScrap);
			this.groupBoxScrapSearch.Location = new System.Drawing.Point(215, 6);
			this.groupBoxScrapSearch.Name = "groupBoxScrapSearch";
			this.groupBoxScrapSearch.Size = new System.Drawing.Size(203, 277);
			this.groupBoxScrapSearch.TabIndex = 7;
			this.groupBoxScrapSearch.TabStop = false;
			this.groupBoxScrapSearch.Text = "Scrap Search";
			this.toolTipControls.SetToolTip(this.groupBoxScrapSearch, "Search for junk items containing specific scrap.");
			// 
			// groupBoxNPCSearch
			// 
			this.groupBoxNPCSearch.Controls.Add(this.listBoxNPC);
			this.groupBoxNPCSearch.Controls.Add(this.labelMinSpawnChance);
			this.groupBoxNPCSearch.Controls.Add(this.numericUpDownNPCSpawnThreshold);
			this.groupBoxNPCSearch.Controls.Add(this.buttonSearchNPC);
			this.groupBoxNPCSearch.Location = new System.Drawing.Point(6, 6);
			this.groupBoxNPCSearch.Name = "groupBoxNPCSearch";
			this.groupBoxNPCSearch.Size = new System.Drawing.Size(203, 277);
			this.groupBoxNPCSearch.TabIndex = 6;
			this.groupBoxNPCSearch.TabStop = false;
			this.groupBoxNPCSearch.Text = "NPC Search";
			this.toolTipControls.SetToolTip(this.groupBoxNPCSearch, "Search for all spawns of an NPC (Both variable and guaranteed spawns).");
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
			this.splitContainerMain.Panel1.Controls.Add(this.checkBoxAddAsGroup);
			this.splitContainerMain.Panel1.Controls.Add(this.buttonRemoveFromLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.buttonAddToLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.tabControlStandardNPCJunk);
			this.splitContainerMain.Panel1.Controls.Add(this.gridViewSearchResults);
			this.splitContainerMain.Panel1.Controls.Add(this.gridViewLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.labelSearchResults);
			this.splitContainerMain.Panel1.Controls.Add(this.labelLegend);
			this.splitContainerMain.Panel1.Controls.Add(this.buttonDrawMap);
			this.splitContainerMain.Panel1MinSize = 460;
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.pictureBoxMapPreview);
			this.splitContainerMain.Panel2MinSize = 0;
			this.splitContainerMain.Size = new System.Drawing.Size(1685, 871);
			this.splitContainerMain.SplitterDistance = 811;
			this.splitContainerMain.TabIndex = 6;
			// 
			// checkBoxAddAsGroup
			// 
			this.checkBoxAddAsGroup.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.checkBoxAddAsGroup.AutoSize = true;
			this.checkBoxAddAsGroup.Location = new System.Drawing.Point(181, 634);
			this.checkBoxAddAsGroup.Name = "checkBoxAddAsGroup";
			this.checkBoxAddAsGroup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkBoxAddAsGroup.Size = new System.Drawing.Size(91, 17);
			this.checkBoxAddAsGroup.TabIndex = 2;
			this.checkBoxAddAsGroup.Text = "Add as Group";
			this.toolTipControls.SetToolTip(this.checkBoxAddAsGroup, "Add all selected items under the same Legend Group.");
			this.checkBoxAddAsGroup.UseVisualStyleBackColor = true;
			// 
			// buttonRemoveFromLegend
			// 
			this.buttonRemoveFromLegend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonRemoveFromLegend.Location = new System.Drawing.Point(408, 634);
			this.buttonRemoveFromLegend.Name = "buttonRemoveFromLegend";
			this.buttonRemoveFromLegend.Size = new System.Drawing.Size(124, 23);
			this.buttonRemoveFromLegend.TabIndex = 4;
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
			this.buttonAddToLegend.TabIndex = 3;
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
			// columnLegendGroup
			// 
			this.columnLegendGroup.HeaderText = "Legend Group";
			this.columnLegendGroup.Name = "columnLegendGroup";
			this.columnLegendGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// columnLegendDisplayName
			// 
			this.columnLegendDisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.columnLegendDisplayName.FillWeight = 400F;
			this.columnLegendDisplayName.HeaderText = "Display Name";
			this.columnLegendDisplayName.Name = "columnLegendDisplayName";
			this.columnLegendDisplayName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
			this.MinimumSize = new System.Drawing.Size(640, 840);
			this.Name = "FormMaster";
			this.Text = "Mappalachia";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
			this.menuStripMain.ResumeLayout(false);
			this.menuStripMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridViewSearchResults)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewLegend)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownNPCSpawnThreshold)).EndInit();
			this.tabControlStandardNPCJunk.ResumeLayout(false);
			this.tabPageStandard.ResumeLayout(false);
			this.tabPageStandard.PerformLayout();
			this.groupBoxCellModeSettings.ResumeLayout(false);
			this.groupBoxCellModeSettings.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMinz)).EndInit();
			this.groupBoxFilterByLockLevel.ResumeLayout(false);
			this.groupBoxFilterByCategory.ResumeLayout(false);
			this.tabPageNpcScrapSearch.ResumeLayout(false);
			this.groupBoxScrapSearch.ResumeLayout(false);
			this.groupBoxNPCSearch.ResumeLayout(false);
			this.groupBoxNPCSearch.PerformLayout();
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
		private System.Windows.Forms.TabControl tabControlStandardNPCJunk;
		private System.Windows.Forms.TabPage tabPageStandard;
		private System.Windows.Forms.TabPage tabPageNpcScrapSearch;
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
		private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem advancedModeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cellModeMenuItem;
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
		private System.Windows.Forms.CheckBox checkBoxAddAsGroup;
		private System.Windows.Forms.GroupBox groupBoxScrapSearch;
		private System.Windows.Forms.GroupBox groupBoxNPCSearch;
		private System.Windows.Forms.GroupBox groupBoxCellModeSettings;
		private System.Windows.Forms.Button buttonCellHeightDistribution;
		private System.Windows.Forms.Label labelCell;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label labelMaxHeight;
		private System.Windows.Forms.Label labelMinHeight;
		private System.Windows.Forms.NumericUpDown numericMaxZ;
		private System.Windows.Forms.NumericUpDown numericMinz;
		private System.Windows.Forms.CheckBox checkBoxCellDrawOutline;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendGroup;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnLegendDisplayName;
	}
}

