using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Cdc.Covid.WebScraper
{
    public class XPath_Scraper : IStateScraper
    {
        private string _state = string.Empty;
        private string _stateAbbreviation = string.Empty;
        private SourceTypes _sourceType;
        private string _source = string.Empty;
        private Expression _expression = new Expression();

        private ArrayList _countyData = new ArrayList();

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

            var web = new HtmlWeb();
            var doc = web.Load(_source);

            var nodes = doc.DocumentNode.SelectNodes(_expression.XPath);

            if(nodes[0].Name == "table")
            {
                HtmlNode tableNode = nodes[0];

                var i = tableNode.ChildNodes;
                
                foreach( var node in tableNode.ChildNodes)
                {
                    if(node.NodeType == HtmlNodeType.Element)
                    {
                        if(node.Name == "thead")
                        {

                        }
                        if (node.Name == "tbody")
                        {
                            foreach (var countyRow in node.ChildNodes)
                            {
                                _countyData.Clear();
                                
                                if(countyRow.Name == "tr")
                                {
                                    foreach (var cell in countyRow.ChildNodes)
                                    {
                                        var innerText = cell.InnerText;
                                        _countyData.Add(innerText);
                                    }

                                    AddCounty(countyReports, _countyData);
                                }
                            }
                        }
                        if (node.Name == "tfoot")
                        {

                        }
                    }

                }
            }

            return new StateReport(state: _state, abbreviation: _stateAbbreviation, reports: countyReports);
        }

        private static void AddCounty(List<CountyReport> countyReports, ArrayList countyData)
        {
            string name = (string)countyData[0];
            int confirmed = (int)int.Parse((string)countyData[1]);
            int deaths = (int)int.Parse((string)countyData[2]);
            int hospitalizations = (int)int.Parse((string)countyData[3]);
            double rate = -1;

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
