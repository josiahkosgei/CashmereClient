
//Reporting.MSExcel.ExcelManager


using Cashmere.Library.Standard.Utilities;
using CashmereUtil.Reporting.CITReporting;
using CashmereUtil.Reporting.FailedPostsReporting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cashmere.API.CDM.Reporting.Uptime.Models.Reports;
using Cashmere.Library.CashmereDataAccess.Logging;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;

namespace CashmereUtil.Reporting.MSExcel
{
  public class ExcelManager
  {
    private UtilDepositorLogger log = new UtilDepositorLogger(Assembly.GetAssembly(typeof (ExcelManager)).GetName().Version, nameof (ExcelManager));

    public static byte[] GenerateCITExcelAttachment(CITReport CITReport, string path)
    {
        using ExcelPackage excel = new ExcelPackage();
        ExcelWorksheet excelWorksheet1 = excel.Workbook.Worksheets.Add("All Transactions");
        excelWorksheet1.Column(2).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet1.Column(3).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet1.Column(7).Style.Numberformat.Format = "#,##0.00";
        excelWorksheet1.Cells[1, 1].LoadFromCollectionFiltered(CITReport.Transactions, true, TableStyles.Light13);
        for (int index = 0; index < 20; ++index)
        {
            excelWorksheet1.Column(index + 1).BestFit = true;
            excelWorksheet1.Column(index + 1).AutoFit();
        }
        ExcelWorksheet excelWorksheet2 = excel.Workbook.Worksheets.Add("Escrow Jams");
        excelWorksheet2.Column(1).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet2.Column(2).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet2.Column(10).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet2.Column(11).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet2.Column(3).Style.Numberformat.Format = "#,##0.00";
        excelWorksheet2.Column(4).Style.Numberformat.Format = "#,##0.00";
        excelWorksheet2.Column(5).Style.Numberformat.Format = "#,##0.00";
        excelWorksheet2.Column(6).Style.Numberformat.Format = "#,##0.00";
        excelWorksheet2.Cells[1, 1].LoadFromCollectionFiltered(CITReport.EscrowJams, true, TableStyles.Light13);
        for (int index = 0; index < 20; ++index)
        {
            excelWorksheet2.Column(index + 1).BestFit = true;
            excelWorksheet2.Column(index + 1).AutoFit();
        }
        foreach (IGrouping<string, CITDenomination> source in CITReport.CITDenominations.GroupBy(x => x.Currency).OrderBy(y => y.Key))
        {
            ExcelWorksheet excelWorksheet3 = excel.Workbook.Worksheets.Add(source.Key + " Denominations");
            excelWorksheet3.Column(2).Style.Numberformat.Format = "#,##0.00";
            excelWorksheet3.Column(3).Style.Numberformat.Format = "#,##0";
            excelWorksheet3.Column(4).Style.Numberformat.Format = "#,##0.00";
            excelWorksheet3.Cells[1, 1].LoadFromCollectionFiltered(source.OrderBy(x => x.Denom), true, TableStyles.Light9);
            ExcelTable excelTable = excelWorksheet3.Tables.First<ExcelTable>();
            excelTable.Name = source.Key + " Denominations";
            excelTable.ShowFilter = true;
            excelTable.ShowTotal = true;
            excelTable.Columns[0].TotalsRowLabel = "Total";
            excelTable.Columns[2].TotalsRowFormula = "SUBTOTAL(109,[Count])";
            excelTable.Columns[3].TotalsRowFormula = "SUBTOTAL(109,[SubTotal])";
            for (int index = 0; index < 5; ++index)
            {
                excelWorksheet3.Column(index + 1).BestFit = true;
                excelWorksheet3.Column(index + 1).AutoFit();
            }
        }
        ExcelWorksheet excelWorksheet4 = excel.Workbook.Worksheets.Add("Summary");
        excelWorksheet4.Column(2).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet4.Column(3).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet4.Column(6).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet4.Column(7).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet4.Cells[1, 1].LoadFromCollectionFiltered(new List<CIT>()
        {
            CITReport.CIT
        }, true, TableStyles.Light9);
        for (int index = 0; index < 20; ++index)
        {
            excelWorksheet4.Column(index + 1).BestFit = true;
            excelWorksheet4.Column(index + 1).AutoFit();
        }
        return SaveExcelToPath(path, excel);
    }

    private static ExcelPackage LoadExcelFromPath(string path) => new ExcelPackage(File.ReadAllBytes(path).ToMemoryStream());

    private static byte[] SaveExcelToPath(string path, ExcelPackage excel)
    {
      FileInfo fileInfo = new FileInfo(path);
      excel.File = fileInfo;
      byte[] asByteArray = excel.GetAsByteArray();
      File.WriteAllBytes(path, asByteArray);
      return asByteArray;
    }

    public static byte[] GenerateFailedPostReportExcelAttachment(
      FailedPostReport failedPostReport,
      string path)
    {
        using ExcelPackage excel = new ExcelPackage();
        ExcelWorksheet excelWorksheet = excel.Workbook.Worksheets.Add("All Transactions");
        excelWorksheet.Column(1).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet.Column(2).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet.Column(7).Style.Numberformat.Format = "#,##0.00";
        excelWorksheet.Column(22).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        excelWorksheet.Cells[1, 1].LoadFromCollectionFiltered(failedPostReport.FailedTransactions, true, TableStyles.Light13);
        for (int index = 0; index < 30; ++index)
        {
            excelWorksheet.Column(index + 1).BestFit = true;
            excelWorksheet.Column(index + 1).AutoFit();
        }
        return SaveExcelToPath(path, excel);
    }

    public static byte[] GenerateUptimeReportExcelAttachment(
      UptimeReport UptimeReport,
      string templatePath,
      string path)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      using ExcelPackage excel = LoadExcelFromPath(templatePath);
      ExcelWorksheet excelWorksheet = excel.Workbook.Worksheets.First<ExcelWorksheet>();
      excelWorksheet.SetValue("B2", UptimeReport.UptimeReportSummary.StartDate);
      excelWorksheet.SetValue("B3", UptimeReport.UptimeReportSummary.EndDate);
      excelWorksheet.SetValue("B4", UptimeReport.UptimeReportSummary.Days);
      excelWorksheet.SetValue("B5", UptimeReport.UptimeReportSummary.DeviceName);
      excelWorksheet.SetValue("B6", UptimeReport.UptimeReportSummary.DeviceLocation);
      excelWorksheet.SetValue("B7", UptimeReport.UptimeReportSummary.DeviceNumber);
      ExcelWorksheet worksheet1 = excel.Workbook.Worksheets[2];
      object obj = excelWorksheet.Cells["A9"].Value;
      excelWorksheet.InsertRow(10, UptimeReport.UptimeReportSummary.UptimeSummary.Count() - 1);
      excelWorksheet.Cells[9, 1].LoadFromCollection(UptimeReport.UptimeReportSummary.UptimeSummary.OrderBy(x => x.Start), true);
      worksheet1.Cells[1, 1].LoadFromCollectionFiltered(UptimeReport.ModeData, true, TableStyles.Light13);
      excelWorksheet.Cells[9, 1, 100, 2].Style.Numberformat.Format = "yyyy-MM-dd";
      worksheet1.Column(3).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
      worksheet1.Column(4).Style.Numberformat.Format = "d HH:mm:ss";
      worksheet1.Column(5).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
      for (int index = 0; index < 10; ++index)
      {
          worksheet1.Column(index + 1).BestFit = true;
          worksheet1.Column(index + 1).AutoFit();
      }
      ExcelWorksheet worksheet2 = excel.Workbook.Worksheets[3];
      ExcelWorksheet worksheet3 = excel.Workbook.Worksheets[4];
      worksheet2.SetValue("B2", UptimeReport.UptimeReportSummary.StartDate);
      worksheet2.SetValue("B3", UptimeReport.UptimeReportSummary.EndDate);
      worksheet2.SetValue("B4", UptimeReport.UptimeReportSummary.Days);
      worksheet2.SetValue("B5", UptimeReport.UptimeReportSummary.DeviceName);
      worksheet2.SetValue("B6", UptimeReport.UptimeReportSummary.DeviceLocation);
      worksheet2.SetValue("B7", UptimeReport.UptimeReportSummary.DeviceNumber);
      worksheet3.Cells[1, 1].LoadFromCollection(UptimeReport.ComponentData, true, TableStyles.Light13);
      for (int index = 0; index < 10; ++index)
      {
          worksheet3.Column(index + 1).BestFit = true;
          worksheet3.Column(index + 1).AutoFit();
      }
      ExcelPivotTable excelPivotTable = worksheet2.PivotTables.Add(worksheet2.Cells["A9"], worksheet3.Cells[worksheet3.Dimension.Address], "ComponentStateSummaryPivot");
      excelPivotTable.RowFields.Add(excelPivotTable.Fields["Start Date"]);
      excelPivotTable.DataOnRows = false;
      excelPivotTable.RowFields[0].AddDateGrouping(eDateGroupBy.Days);
      excelPivotTable.ColumnFields.Add(excelPivotTable.Fields["Component"]);
      ExcelPivotTableDataField pivotTableDataField = excelPivotTable.DataFields.Add(excelPivotTable.Fields["Duration"]);
      pivotTableDataField.Name = "Sum of Duration";
      pivotTableDataField.Function = DataFieldFunctions.Sum;
      pivotTableDataField.Format = "d HH:mm:ss";
      worksheet3.Column(3).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
      worksheet3.Column(5).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
      excel.Workbook.Calculate();
      excel.Workbook.Worksheets.First<ExcelWorksheet>().Select();
      return SaveExcelToPath(path, excel);
    }
  }
}
