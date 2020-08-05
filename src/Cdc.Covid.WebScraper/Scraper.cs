using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cdc.Covid.WebScraper
{
    public class Scraper : IStateScraper
    {
        private string _state = string.Empty;
        private string _stateAbbreviation = string.Empty;
        private SourceTypes _sourceType;
        private string _source = string.Empty;
        private Expression _expression = new Expression();


        public Scraper(StateScrapeInfo info)
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

            return new StateReport(state: _state, abbreviation: _stateAbbreviation, reports: countyReports);
        }
    }
}
