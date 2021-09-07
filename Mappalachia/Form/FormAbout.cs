using System;
using System.Reflection;
using System.Windows.Forms;

namespace Mappalachia
{
	internal partial class FormAbout : Form
	{
		public FormAbout()
		{
			InitializeComponent();
			Text = string.Format("About {0}", AssemblyTitle);
			labelProductName.Text = AssemblyProduct;
			labelVersion.Text = string.Format("Version {0}", AssemblyVersion);
			labelCopyright.Text = AssemblyCopyright;
			labelCompanyName.Text = AssemblyCompany;
			textBoxDescription.Text = AssemblyDescription + "\r\nDatabase game version: " + IOManager.GetGameVersion();
		}

		public string AssemblyTitle
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

				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			Close();
			Dispose();
		}
	}
}
