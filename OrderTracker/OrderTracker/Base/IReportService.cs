using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTracker
{
    public interface IReportService
    {
      Task<ReportGenResponse> Generate<T>(IEnumerable<T> data, string fileName);
      Task<ReportGenResponse> Generate<TKey, TData>(IEnumerable<IGrouping<TKey, TData>> groupedData, string fileName);

    }
}
