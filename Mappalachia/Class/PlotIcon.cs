using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mappalachia.Class
{
	//A customisable Image used to represent a plot on the map
	public class PlotIcon
	{
		//Pull PlotIcon settings from the currently applied PlotSettings
		readonly int size = SettingsPlotIcon.iconSize;
		readonly int lineWidth = SettingsPlotIcon.lineWidth;
		readonly int iconOpacityPercent = SettingsPlotIcon.iconOpacityPercent;
		readonly int shadowOpacityPercent = SettingsPlotIcon.shadowOpacityPercent;
		readonly float halfSize;
		readonly float quartSize;
		readonly float threeQuartSize;
		readonly Pen pen;
		readonly Brush brush;
		readonly Bitmap bitmap;
		readonly Graphics icon;

		public Color color;
		public PlotIconShape shape;

		public PlotIcon(Color color, PlotIconShape shape)
		{
			this.color = color;
			this.shape = shape;

			//All icon shape coordinates target points on a 4x4 grid
			//This way, icon shapes can be mixed & matched and still look proper
			halfSize = size / 2f;
			quartSize = size / 4f;
			threeQuartSize = quartSize * 3;

			pen = new Pen(Color.White, lineWidth);
			brush = new SolidBrush(Color.White);
			bitmap = new Bitmap(size, size);
			icon = Graphics.FromImage(bitmap);
			icon.SmoothingMode = SmoothingMode.AntiAlias;
		}

		//Draw and return the icon image
		public Image GetIconImage()
		{
			if (shape.crosshairInner)
			{
				icon.DrawLine(pen, halfSize, quartSize, halfSize, threeQuartSize); //Vertical
				icon.DrawLine(pen, quartSize, halfSize, threeQuartSize, halfSize); //Horizontal
			}

			if (shape.crosshairOuter)
			{
				icon.DrawLine(pen, halfSize, size, halfSize, threeQuartSize); //Top
				icon.DrawLine(pen, halfSize, 0, halfSize, quartSize); //Bottom
				icon.DrawLine(pen, 0, halfSize, quartSize, halfSize); //Left
				icon.DrawLine(pen, size, halfSize, threeQuartSize, halfSize); //Right
			}

			if (shape.diamond)
			{
				PointF[] diamondCorners =
				{
					new PointF(halfSize, quartSize), //Top
					new PointF(quartSize, halfSize), //Bottom
					new PointF(halfSize, threeQuartSize), //Left
					new PointF(threeQuartSize, halfSize), //Right
				};

				if (shape.fill)
				{
					icon.FillPolygon(brush, diamondCorners);
				}
				else
				{
					icon.DrawPolygon(pen, diamondCorners);
				}
			}

			if (shape.square || shape.circle)
			{
				Rectangle halfRadiusRect = new Rectangle((int)quartSize, (int)quartSize, (int)halfSize, (int)halfSize);

				if (shape.square)
				{
					if (shape.fill)
					{
						icon.FillRectangle(brush, halfRadiusRect);
					}
					else
					{
						icon.DrawRectangle(pen, halfRadiusRect);
					}
				}

				if (shape.circle)
				{
					if (shape.fill)
					{
						icon.FillEllipse(brush, halfRadiusRect);
					}
					else
					{
						icon.DrawEllipse(pen, halfRadiusRect);
					}
				}
			}

			Image iconImage = ImageTools.AdjustARGB(bitmap, Color.FromArgb((int)(iconOpacityPercent / 100f * 255f), color));
			iconImage = ImageTools.AddDropShadow(iconImage, lineWidth, (int)(shadowOpacityPercent / 100f * 255f));

			return iconImage;
		}
	}
}
