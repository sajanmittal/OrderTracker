namespace OrderTracker
{
	public class ReportGenResponse
	{
		public string FileLocation { get; set; }

		public bool IsGenerated { get; set; }

		public static ReportGenResponse Empty => new ReportGenResponse { IsGenerated = false, FileLocation = string.Empty };
	}
}