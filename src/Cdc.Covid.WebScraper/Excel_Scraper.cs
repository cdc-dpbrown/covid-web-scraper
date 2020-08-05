using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cdc.Covid.WebScraper
{
    public class Excel_Scraper : IStateScraper
    {
        private string _state = string.Empty;
        private string _stateAbbreviation = string.Empty;
        private SourceTypes _sourceType;
        private string _source = string.Empty;
        private Expression _expression = new Expression();

        public Excel_Scraper(StateScrapeInfo info)
        {
            _state = info.State;
            _stateAbbreviation = info.StateAbbreviation;
            _sourceType = info.SourceType;
            _source = info.Source;
            _expression = info.ExpressionObject;
        }


        public async Task<StateReport> ExecuteScrapeAsync()
        {
            List<CountyReport> countyReports = new List<CountyReport>(250);

            HttpClient client = new HttpClient();
            using (Stream file = await client.GetStreamAsync(_source).ConfigureAwait(false))
            {
                var wb = new XSSFWorkbook(file);
                ISheet sheet = wb.GetSheetAt(0);

                if (sheet != null)
                {
                    int rowCount = sheet.LastRowNum; 

                    for (int i = 1; i <= rowCount; i++)
                    {
                        IRow curRow = sheet.GetRow(i);
                        
                        if (curRow == null)
                        {
                            rowCount = i - 1;
                            break;
                        }

                        string name = curRow.GetCell(0).StringCellValue.Trim();
                        string status = curRow.GetCell(1).StringCellValue.Trim();
                        int cases = (int)curRow.GetCell(2).NumericCellValue;
                        int deaths = (int)curRow.GetCell(3).NumericCellValue;

                        var countyReport = countyReports.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                        if (countyReport == null)
                        {
                            countyReport = new CountyReport(
                                name: name,
                                confirmed: -1,
                                probable: -1,
                                deaths: deaths,
                                hospitalizations: -1,
                                rate: -1);
                        }

                        if (status == "Confirmed")
                        {
                            countyReport.Confirmed = cases;
                        }
                        else if (status == "Probable")
                        {
                            countyReport.Probable = cases;
                        }

                        countyReports.Add(countyReport);
                    }
                }
            }

            return new StateReport(state: _state, abbreviation: _stateAbbreviation, reports: countyReports);
        }
    }
}
