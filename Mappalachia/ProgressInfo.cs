namespace Mappalachia
{
	public class ProgressInfo(int percent, string status = "")
	{
		public int Percent { get; set; } = percent;

		public string Status { get; set; } = status;
	}
}
