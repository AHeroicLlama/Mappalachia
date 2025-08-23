using System.ComponentModel;
using KGySoft.CoreLibraries;

namespace Mappalachia
{
	public partial class FormEditPlotIcon : GenericToolForm
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PlotIcon CurrentIcon { get; set; }

		public FormEditPlotIcon(PlotIcon plotIcon)
		{
			InitializeComponent();

			CurrentIcon = plotIcon;

			trackBarIconSize.Minimum = PlotStyleSettings.MinSize;
			trackBarIconSize.Maximum = PlotStyleSettings.MaxSize;
			trackBarIconSize.Value = Math.Clamp(CurrentIcon.Size, trackBarIconSize.Minimum, trackBarIconSize.Maximum);

			SetSizeLabel();

			if (CurrentIcon.ParentIsRegion)
			{
				buttonSelectIcon.Enabled = false;
				trackBarIconSize.Enabled = false;
			}

			RefreshPreviewImage();
		}

		private void ButtonSelectIcon_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog() { InitialDirectory = Path.GetFullPath(Paths.IconsPath), Filter = "PNG|*.png" };

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				CurrentIcon.BaseIconImage = FileIO.GetPlotIconImage(openFileDialog.FileName);
				CurrentIcon.BaseIconIndex = Directory.GetFiles(Paths.IconsPath).Select(p => Path.GetFullPath(p)).IndexOf(openFileDialog.FileName);
				RefreshPreviewImage();
			}
		}

		private void ButtonSelectColor_Click(object sender, EventArgs e)
		{
			if (ColorDialog.ShowDialog() == DialogResult.OK)
			{
				CurrentIcon.Color = ColorDialog.Color;
				RefreshPreviewImage();
			}
		}

		private void TrackBarIconSize_Scroll(object sender, EventArgs e)
		{
			CurrentIcon.Size = trackBarIconSize.Value;
			SetSizeLabel();
			RefreshPreviewImage();
		}

		void SetSizeLabel()
		{
			labelSize.Text = $"Size ({CurrentIcon.Size})";
		}

		void RefreshPreviewImage()
		{
			pictureBoxIcon.Image = CurrentIcon.GetImage();
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
