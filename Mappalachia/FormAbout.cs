using System.Reflection;
using Library;

namespace Mappalachia
{
	// Largely auto-generated
	public partial class FormAbout : Form
	{
		public FormAbout()
		{
			InitializeComponent();
			Text = string.Format("About {0}", AssemblyTitle);
			labelProductName.Text = AssemblyProduct;
			labelVersion.Text = string.Format("Version {0}", AssemblyVersion);
			labelCopyright.Text = AssemblyCopyright;
			linkLabelGitHub.Text = "GitHub";
			textBoxDescription.Text = AssemblyDescription + "\r\nDatabase game version: " + CommonDatabase.GetGameVersion(Database.Connection).Result + "\r\n\r\n" +
				"Mappalachia is provided as a non-commercial, free tool solely for the benefit of players of Fallout 76. Mappalachia and its creator are neither affiliated with - nor endorsed by - ZeniMax Media or any of its subsidiaries including Bethesda Softworks LLC. Any and all game data and/or assets including but not limited to images, characters, names and other game data which are contained within this application are extracted from a purchased copy of Fallout 76 and are shared with the player community in good faith and for the explicit purpose of making maps for the benefit of said community, with an understanding that this lies within fair use.\r\n" +
				"Great care has been taken to minimize such data so that it cannot be reconstructed in any meaningful way.\r\n" +
				"If you have any concerns or queries, please direct them to mappalachia.feedback@gmail.com";
		}

		public static string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != string.Empty)
					{
						return titleAttribute.Title;
					}
				}

				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
			}
		}

		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? throw new NullReferenceException("Assembly version is null");
			}
		}

		public static string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

				if (attributes.Length == 0)
				{
					return string.Empty;
				}

				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public static string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

				if (attributes.Length == 0)
				{
					return string.Empty;
				}

				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

				if (attributes.Length == 0)
				{
					return string.Empty;
				}

				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public static string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

				if (attributes.Length == 0)
				{
					return string.Empty;
				}

				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		private void LinkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Common.OpenURI(URLs.GitHub);
			linkLabelGitHub.LinkVisited = true;
		}
	}
}
