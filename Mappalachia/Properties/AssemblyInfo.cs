using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Mappalachia")]
[assembly: AssemblyDescription("The complete mapping tool for Fallout 76.\r\n" +
	"Mappalachia is a Windows Forms GUI for generating and exporting complex maps of entities within the Fallout 76 world.\r\n" +
	"For Fallout 76 version " + Mappalachia.AssemblyInfo.gameVersion + ".")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Mappalachia")]
[assembly: AssemblyCopyright("AHeroicLlama")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4e44c396-6917-402e-a3e0-1ab2791624ff")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.2.1.0")]
[assembly: AssemblyFileVersion("1.2.1.0")]
[assembly: NeutralResourcesLanguage("en-US")]

namespace Mappalachia
{
	public static class AssemblyInfo
	{
		public const string gameVersion = "1.5.1.26";
	}
}