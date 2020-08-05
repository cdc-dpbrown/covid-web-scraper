using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cdc.Covid.WebScraper
{
    public sealed class ReportGenerator
    {
        private readonly object _syncLock = new object();

        public List<StateReport> Generate()
        {
            List<StateReport> stateReports = new List<StateReport>();

            List<IStateScraper> scrapers = GetScraperList();

            Parallel.ForEach(scrapers, scraper =>
            {
                StateReport stateReport = scraper.ExecuteScrapeAsync().Result;
                lock (_syncLock) { stateReports.Add(stateReport); }
            });

            return stateReports;
        }

        private List<IStateScraper> GetScraperList()
        {
            List<IStateScraper> scrapers = new List<IStateScraper>();

            MetadataReport metadataReport = new MetadataReport();
            var infos = metadataReport.StateInfos;

            foreach(var info in infos)
            {
                switch (info.SourceType)
                {
                    case SourceTypes.ArcGIS:
                    {
                        scrapers.Add(new Scraper(info));
                        break;
                    }
                    case SourceTypes.Custom:
                    {
                        scrapers.Add(new Scraper(info));
                        break;
                    }
                    case SourceTypes.Excel:
                    {
                        scrapers.Add(new Excel_Scraper(info));
                        break;
                    }
                    case SourceTypes.HTML:
                    {
                        scrapers.Add(new Scraper(info));
                        break;
                    }
                    case SourceTypes.XPATH:
                    {
                        scrapers.Add(new Scraper(info));
                        break;
                    }
                    case SourceTypes.Zip:
                    {
                        scrapers.Add(new Zip_Scraper(info));
                        break;
                    }
                }
            }

            return scrapers;
        }
    }
}
