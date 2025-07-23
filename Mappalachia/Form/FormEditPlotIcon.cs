using System.ComponentModel;

namespace Mappalachia
{
	public partial class FormEditPlotIcon : Form
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PlotIcon CurrentIcon { get; set; }

		FormMain FormMain { get; }

		public FormEditPlotIcon(FormMain formMain, PlotIcon plotIcon)
		{
			InitializeComponent();

			FormMain = formMain;
			CurrentIcon = plotIcon;

			trackBarIconSize.Minimum = FormMain.Settings.PlotSettings.PlotIconMinSize;
			trackBarIconSize.Maximum = FormMain.Settings.PlotSettings.PlotIconMaxSize;
			trackBarIconSize.Value = CurrentIcon.Size;

			RefreshPreviewImage();
		}

		private void ButtonSelectIcon_Click(object sender, EventArgs e)
		{
			// TODO open file picker at icon folder and load accordingly
		}

		private void ButtonSelectColor_Click(object sender, EventArgs e)
		{
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				CurrentIcon.Color = colorDialog.Color;
				RefreshPreviewImage();
			}
		}

		private void TrackBarIconSize_Scroll(object sender, EventArgs e)
		{
			CurrentIcon.Size = trackBarIconSize.Value;
			RefreshPreviewImage();
		}

		void RefreshPreviewImage()
		{
			pictureBoxIcon.Image = CurrentIcon.GetImage();
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
