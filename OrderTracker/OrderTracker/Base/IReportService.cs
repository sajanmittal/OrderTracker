using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTracker
{
    public interface IReportService
    {
        Task<ReportGenResponse> Generate<T>(IEnumerable<T> data, string fileName) where T : BaseModel;
        Task<ReportGenResponse> Generate<TKey, TData>(IEnumerable<IGrouping<TKey, TData>> groupedData, string fileName) where TData : BaseModel;

    }
}
