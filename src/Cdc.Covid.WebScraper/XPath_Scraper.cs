using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cdc.Covid.WebScraper
{
    public class XPath_Scraper : IStateScraper
    {
        private string _state = string.Empty;
        private string _stateAbbreviation = string.Empty;
        private SourceTypes _sourceType;
        private string _source = string.Empty;
        private Expression _expression = new Expression();


        public XPath_Scraper(StateScrapeInfo info)
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
            using (var file = await client.GetStreamAsync(_source).ConfigureAwait(false))
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                using (ZipArchive z = new ZipArchive(memoryStream))
                {
                    ZipArchiveEntry zEntry = z.Entries.FirstOrDefault(e => e.Name.Contains(_expression.FileName));

                    using (Stream csvStream = zEntry.Open())
                    using (StreamReader reader = new StreamReader(csvStream))
                    {
                        bool headersLine = true;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            var cells = line.Split(',');

                            if (headersLine)
                            {
                                headersLine = false;
                            }
                            else
                            {
                                string name = cells[0];
                                int confirmed = int.Parse(cells[1]);
                                int deaths = int.Parse(cells[2]);
                                int hospitalizations = int.Parse(cells[3]);
                                double rate = double.Parse(cells[4]);

                                CountyReport report = new CountyReport(
                                    name: name, 
                                    confirmed: confirmed, 
                                    probable: -1, 
                                    deaths: deaths, 
                                    hospitalizations: hospitalizations,
                                    rate: rate);

                                countyReports.Add(report);
                            }
                        }
                    }
                }
            }

            return new StateReport(state: _state, abbreviation: _stateAbbreviation, reports: countyReports);
        }
    }
}
