using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Mappalachia.Class;

namespace Mappalachia
{
	public partial class FormPlotIconSettings : Form
	{
		static List<PlotIconShape> placeholderShapePalette = new List<PlotIconShape>(); //Holds the currently *selected* shape settings (not currently applied ones)
		static readonly Regex htmlColor = new Regex("ff[0-9a-f]{6}"); //Match the alpha-less 'html' color returned from a ColorDialog
		static int lastSelectedShapeIndex = 0;

		public FormPlotIconSettings()
		{
			InitializeComponent();

			trackBarIconSize.Minimum = SettingsPlotIcon.iconSizeMin / 10;
			trackBarIconSize.Maximum = SettingsPlotIcon.iconSizeMax / 10;
			trackBarIconWidth.Minimum = SettingsPlotIcon.lineWidthMin;
			trackBarIconWidth.Maximum = SettingsPlotIcon.lineWidthMax;
			trackBarIconOpacity.Minimum = SettingsPlotIcon.iconOpacityPercentMin / 10;
			trackBarIconOpacity.Maximum = SettingsPlotIcon.iconOpacityPercentMax / 10;
			trackBarShadowOpacity.Minimum = SettingsPlotIcon.iconOpacityPercentMin / 10;
			trackBarShadowOpacity.Maximum = SettingsPlotIcon.iconOpacityPercentMax / 10;

			LoadSettingsIntoForm();
		}

		//Reverse-normalise values to fit form and apply them to the form
		void LoadSettingsIntoForm()
		{
			trackBarIconSize.Value = SettingsPlotIcon.iconSize / 10;
			trackBarIconWidth.Value = SettingsPlotIcon.lineWidth;
			trackBarIconOpacity.Value = SettingsPlotIcon.iconOpacityPercent / 10;
			trackBarShadowOpacity.Value = SettingsPlotIcon.shadowOpacityPercent / 10;

			PopulateColorPaletteUI(SettingsPlotIcon.paletteColor);
			PopulateShapePaletteUI(SettingsPlotIcon.paletteShape);
			placeholderShapePalette = new List<PlotIconShape>(SettingsPlotIcon.paletteShape);
			SelectShapeAtIndex(0);
		}

		//Normalise and apply the settings from the form to the class properties
		void SaveSettingsFromForm()
		{
			SettingsPlotIcon.iconSize = trackBarIconSize.Value * 10;
			SettingsPlotIcon.lineWidth = trackBarIconWidth.Value;
			SettingsPlotIcon.iconOpacityPercent = trackBarIconOpacity.Value * 10;
			SettingsPlotIcon.shadowOpacityPercent = trackBarShadowOpacity.Value * 10;

			//Rebuild the palette with colors from the UI
			List<Color> tempColorPalette = new List<Color>();
			foreach (ListViewItem colorName in listViewColorPalette.Items)
			{
				tempColorPalette.Add(GetColorFromText(colorName.Text));
			}

			SettingsPlotIcon.paletteColor = new List<Color>(tempColorPalette);

			//Iterate through the settings in UI to recreate the shape palette
			List<PlotIconShape> tempShapePalette = new List<PlotIconShape>();
			foreach (ListViewItem shape in listViewShapePalette.Items)
			{
				shape.Selected = true;
				tempShapePalette.Add(new PlotIconShape(checkBoxDiamond.Checked, checkBoxSquare.Checked, checkBoxCircle.Checked, checkBoxCrosshairInner.Checked, checkBoxCrosshairOuter.Checked, checkBoxFill.Checked));
			}

			SettingsPlotIcon.paletteShape = new List<PlotIconShape>(tempShapePalette);
		}

		//Find a color by its name or html code
		Color GetColorFromText(string colorNameOrCode)
		{
			try
			{
				return htmlColor.Match(colorNameOrCode).Success ?
					ColorTranslator.FromHtml("#" + colorNameOrCode) :
					Color.FromName(colorNameOrCode);
			}
			catch (Exception)
			{
				Notify.Error("Invalid color name " + colorNameOrCode + ". Unable to display color on palette.");
				return Color.Gray;
			}
		}

		//Apply a color to a ListViewItem matching the color on its text
		void AddBackColor(ListViewItem item)
		{
			item.BackColor = GetColorFromText(item.Text);
		}

		//Load the palette selected in the UI, to the UI list
		void LoadSelectedColorPalette()
		{
			switch (comboBoxPalette.Text)
			{
				case "Default":
					PopulateColorPaletteUI(SettingsPlotIcon.paletteColorDefault);
					break;
				case "Colorblind (IBM)":
					PopulateColorPaletteUI(SettingsPlotIcon.paletteColorBlindIBM);
					break;
				case "Colorblind (Wong)":
					PopulateColorPaletteUI(SettingsPlotIcon.paletteColorBlindWong);
					break;
				case "Colorblind (Tol)":
					PopulateColorPaletteUI(SettingsPlotIcon.paletteColorBlindTol);
					break;
				default:
					Notify.Error("Unexpected color palette " + comboBoxPalette.Text + ". The palette cannot be loaded.");
					return;
			}
		}

		//Populate the UI with a given color palette
		void PopulateColorPaletteUI(List<Color> newPalette)
		{
			//Wipe the UI items
			listViewColorPalette.Items.Clear();

			foreach (Color color in newPalette)
			{
				//Add the color items back to the UI, and backcolor them with themselves
				AddBackColor(listViewColorPalette.Items.Add(color.Name));
			}
		}

		//Populate the shape palette UI with the provided list of shapes
		void PopulateShapePaletteUI(List<PlotIconShape> newPalette)
		{
			//Wipe the UI items
			listViewShapePalette.Items.Clear();

			//Add a UI item which represents each shape
			for (int i = 0; i < newPalette.Count; i++)
			{
				listViewShapePalette.Items.Add("Shape " + (i + 1));
			}

			SetShapeOptionsEnabled(newPalette.Count != 0);
		}

		//Verify the color palette is not empty
		bool CheckForEmptyColorPalette()
		{
			if (listViewColorPalette.SelectedItems.Count >= listViewColorPalette.Items.Count)
			{
				Notify.Warn("Color palette cannot be empty. Please select at least one color before continuing.");
				return false;
			}
			else
			{
				return true;
			}
		}

		//Verify the shape palette is not empty
		bool CheckForEmptyShapePalette()
		{
			if (listViewShapePalette.Items.Count == 0)
			{
				Notify.Warn("Shape palette cannot be empty. Please add at least one shape before continuing.");
				return false;
			}
			else
			{
				return true;
			}
		}

		//Verify every shape has at least one option checked
		bool CheckForEmptyShapes()
		{
			foreach (ListViewItem shape in listViewShapePalette.Items)
			{
				shape.Selected = true;
				if (!(checkBoxDiamond.Checked || checkBoxSquare.Checked || checkBoxCircle.Checked || checkBoxCrosshairInner.Checked || checkBoxCrosshairOuter.Checked))
				{
					Notify.Info(shape.Text + " has no shape options selected and would be invisible. Please select at least one shape option for each shape in the palette.");
					return false;
				}
			}

			return true;
		}

		//Select a shape from the palette within the UI
		void SelectShapeAtIndex(int index)
		{
			//Skip if this selection is impossible.
			if (index < 0 || index > listViewShapePalette.Items.Count)
			{
				return;
			}

			listViewShapePalette.Items[index].Selected = true;
		}

		//Allow us to visually disable shape options if the shape palette is empty, and re-enable once populated
		void SetShapeOptionsEnabled(bool isEnabled)
		{
			checkBoxDiamond.Enabled = isEnabled;
			checkBoxSquare.Enabled = isEnabled;
			checkBoxCircle.Enabled = isEnabled;
			checkBoxCrosshairInner.Enabled = isEnabled;
			checkBoxCrosshairOuter.Enabled = isEnabled;
			checkBoxFill.Enabled = isEnabled;

			if (!isEnabled)
			{
				checkBoxDiamond.Checked = isEnabled;
				checkBoxSquare.Checked = isEnabled;
				checkBoxCircle.Checked = isEnabled;
				checkBoxCrosshairInner.Checked = isEnabled;
				checkBoxCrosshairOuter.Checked = isEnabled;
				checkBoxFill.Checked = isEnabled;
			}
		}

		private void ButtonAddColor_Click(object sender, EventArgs e)
		{
			if (colorDialogPalette.ShowDialog() == DialogResult.OK)
			{
				listViewColorPalette.Items.Add(colorDialogPalette.Color.Name);

				//Send the newly added color name to be colored
				int addedIndex = listViewColorPalette.Items.Count - 1;
				ListViewItem addedItem = listViewColorPalette.Items[addedIndex];
				AddBackColor(addedItem);
			}
		}

		private void ButtonRemoveColor_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listViewColorPalette.SelectedItems)
			{
				item.Remove();
			}
		}

		private void ButtonAddShape_Click(object sender, EventArgs e)
		{
			placeholderShapePalette.Add(new PlotIconShape());
			PopulateShapePaletteUI(placeholderShapePalette);

			//Select the last item, being the one we just added
			SelectShapeAtIndex(listViewShapePalette.Items.Count - 1);
		}

		private void ButtonRemoveShape_Click(object sender, EventArgs e)
		{
			//Skip if none selected
			if (listViewShapePalette.SelectedItems.Count == 0)
			{
				Notify.Info("Select a shape to remove.");
				return;
			}

			int selectedIndex = listViewShapePalette.SelectedItems[0].Index;
			placeholderShapePalette.RemoveAt(selectedIndex);
			PopulateShapePaletteUI(placeholderShapePalette);

			SelectShapeAtIndex(listViewShapePalette.Items.Count - 1);
		}

		//Load the premade color palette selected from the dropdown
		private void ComboBoxColorPalette_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadSelectedColorPalette();
		}

		//Update the shape option checkboxes in line with the currently selected shape
		private void ListViewShapePalette_SelectedIndexChanged(object sender, EventArgs e)
		{
			/*This fires twice when the index changes - once to un-select the last selected item
			Then once more to re-select the new item.
			We need to save the old item once unselected, and load the new one once selected.*/

			//Unselecting - save the last selected.
			if (listViewShapePalette.SelectedItems.Count == 0)
			{
				placeholderShapePalette[lastSelectedShapeIndex] =
					new PlotIconShape(checkBoxDiamond.Checked, checkBoxSquare.Checked, checkBoxCircle.Checked, checkBoxCrosshairInner.Checked, checkBoxCrosshairOuter.Checked, checkBoxFill.Checked);
			}

			//Re-selecting - load in the settings.
			else
			{
				//Only one should be selectable, so taking the first
				int selectedShapeIndex = listViewShapePalette.SelectedItems[0].Index;
				PlotIconShape selectedShape = placeholderShapePalette[selectedShapeIndex];

				checkBoxDiamond.Checked = selectedShape.diamond;
				checkBoxSquare.Checked = selectedShape.square;
				checkBoxCircle.Checked = selectedShape.circle;
				checkBoxCrosshairInner.Checked = selectedShape.crosshairInner;
				checkBoxCrosshairOuter.Checked = selectedShape.crosshairOuter;
				checkBoxFill.Checked = selectedShape.fill;

				lastSelectedShapeIndex = selectedShapeIndex;
			}
		}

		private void ButtonApply_Click(object sender, EventArgs e)
		{
			if (!CheckForEmptyColorPalette() || !CheckForEmptyShapePalette() || !CheckForEmptyShapes())
			{
				return;
			}

			SaveSettingsFromForm();
			PlotIconCache.ResetCache(); //Reset the cached legend icons, as they are now chagning.
			Close();

			if (SettingsPlot.mode == SettingsPlot.Mode.Icon)
			{
				Map.Draw();
			}
		}

		private void ButtonReset_Click(object sender, EventArgs e)
		{
			SettingsPlotIcon.Initialise();
			PlotIconCache.ResetCache(); //Reset the cached legend icons, as they are now changing.
			LoadSettingsIntoForm();
			Close();

			Map.Draw();
		}
	}
}
