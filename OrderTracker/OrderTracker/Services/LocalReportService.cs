using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderTracker
{
	public class LocalReportService : IReportService
	{
		public async Task<ReportGenResponse> Generate<T>(IEnumerable<T> data, string fileName)
		{
			ReportGenResponse response = ReportGenResponse.Empty;

			if (data != null)
			{
				var fileLocation = Path.Combine(DependencyService.Get<IFileAccessService>().GetFileLocation(), string.Concat(fileName, Constants.EXCEL_XTSN));
				await ExcelHelper.ToExcel(data, fileLocation);
				response.IsGenerated = true;
				response.FileLocation = fileLocation;
			}

			return response;
		}

		public async Task<ReportGenResponse> Generate<TKey, TData>(IEnumerable<IGrouping<TKey, TData>> groupedData, string fileName)
		{
			ReportGenResponse response = ReportGenResponse.Empty;

			if (groupedData != null)
			{
				var fileLocation = Path.Combine(DependencyService.Get<IFileAccessService>().GetFileLocation(), string.Concat(fileName, Constants.EXCEL_XTSN));
				await ExcelHelper.ToExcel(groupedData, fileLocation);
				response.IsGenerated = true;
				response.FileLocation = fileLocation;
			}

			return response;
		}
	}
}