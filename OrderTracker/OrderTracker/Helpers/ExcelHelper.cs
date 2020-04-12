using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OrderTracker
{
	public class ExcelHelper
	{
		public async static Task ToExcel<TData>(IEnumerable<TData> data, string path)
		{
			using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook, true))
			{
				DataSet dataSet = new DataSet();
				dataSet.Tables.Add(await data.ToDataTable());

				await CreateExcel(dataSet, document);
				document.Save();
			}
		}

		public async static Task ToExcel<TKey, TData>(IEnumerable<IGrouping<TKey, TData>> groupedData, string path)
		{
			using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook, true))
			{
				DataSet dataSet = new DataSet();
				uint uniqueIndex = 1;
				foreach (var data in groupedData)
				{
					dataSet.Tables.Add(await data.ToDataTable($"{uniqueIndex}_{data.Key}"));
					uniqueIndex++;
				}
				await CreateExcel(dataSet, document);
				document.Save();
			}
		}

		private async static Task CreateExcel(DataSet dataSet, SpreadsheetDocument document)
		{
			document.AddWorkbookPart();
			document.WorkbookPart.Workbook = new Workbook();

			WorksheetPart worksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
			worksheetPart.Worksheet = new Worksheet();

			document.WorkbookPart.Workbook.AppendChild(new Sheets());

			await CreateExcelSheets(dataSet, document.WorkbookPart);

			document.WorkbookPart.Workbook.Save();
		}

		private async static Task CreateExcelSheets(DataSet dataSet, WorkbookPart workbook)
		{
			var workSheetTasks = new List<Task>();
			var workSheets = new List<Sheet>();
			uint sheetNumber = 1;
			foreach (DataTable table in dataSet.Tables)
			{
				workSheetTasks.Add(Task.Run(() =>
				{
					WorksheetPart newWorksheetPart = workbook.AddNewPart<WorksheetPart>();
					newWorksheetPart.Worksheet = new Worksheet();

					newWorksheetPart.Worksheet.AppendChild(new SheetData());

					WriteDataTableToExcelWorksheet(table, newWorksheetPart);

					var sheet = new Sheet
					{
						Id = workbook.GetIdOfPart(newWorksheetPart),
						SheetId = sheetNumber,
						Name = table.TableName
					};
					newWorksheetPart.Worksheet.Save();
					workSheets.Add(sheet);
				}));
				sheetNumber++;
			}

			var allTasks = Task.WhenAll(workSheetTasks);
			await allTasks;

			if (allTasks.IsCompleted)
			{
				var sheets = workbook.Workbook.GetFirstChild<Sheets>();
				workSheets.ForEach(s => sheets.AppendChild(s));
			}
		}

		private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
		{
			var worksheet = worksheetPart.Worksheet;
			var sheetData = worksheet.GetFirstChild<SheetData>();
			int numberOfColumns = dt.Columns.Count;

			string[] excelColumnNames = new string[numberOfColumns];
			for (int n = 0; n < numberOfColumns; n++)
				excelColumnNames[n] = GetExcelColumnName(n);

			uint rowIndex = 1;

			var headerRow = new Row { RowIndex = rowIndex };
			sheetData.Append(headerRow);

			for (int colInx = 0; colInx < numberOfColumns; colInx++)
			{
				DataColumn col = dt.Columns[colInx];
				AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
			}

			foreach (DataRow dr in dt.Rows)
			{
				++rowIndex;
				var newExcelRow = new Row { RowIndex = rowIndex };
				sheetData.Append(newExcelRow);

				for (int colInx = 0; colInx < numberOfColumns; colInx++)
				{
					string cellValue = dr.ItemArray[colInx].ToString();
					AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
				}
			}
		}

		private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
		{
			Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
			CellValue cellValue = new CellValue(cellStringValue);
			cell.Append(cellValue);
			excelRow.Append(cell);
		}

		private static string GetExcelColumnName(int columnIndex)
		{
			if (columnIndex < 26)
				return ((char)('A' + columnIndex)).ToString();

			char firstChar = (char)('A' + (columnIndex / 26) - 1);
			char secondChar = (char)('A' + (columnIndex % 26));

			return string.Format("{0}{1}", firstChar, secondChar);
		}
	}
}