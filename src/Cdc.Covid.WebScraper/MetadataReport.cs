using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cdc.Covid.WebScraper
{
    [DebuggerDisplay("{MetadataReport}")]
    public sealed class MetadataReport
    {
        public Dictionary<SourceTypes, string> SourceTypeText { get; private set; } = new Dictionary<SourceTypes, string>();

        public List<StateScrapeInfo> StateInfos { get; private set; } = new List<StateScrapeInfo>();

        public MetadataReport()
        {
            SourceTypeText.Add(SourceTypes.ArcGIS, "ArcGIS");
            SourceTypeText.Add(SourceTypes.Custom, "Custom Code");
            SourceTypeText.Add(SourceTypes.Excel, "HTML (.html)");
            SourceTypeText.Add(SourceTypes.HTML, "HTML FULL XPATH");
            SourceTypeText.Add(SourceTypes.XPATH, "Excel (.xlsx)");
            SourceTypeText.Add(SourceTypes.Zip, "Zip File (.zip)");

            //StateInfos.Add(new StateScrapeInfo("Alabama", "AL", SourceTypes.Custom, "https://dph1.adph.state.al.us/covid-19/", ""));
            //StateInfos.Add(new StateScrapeInfo("Florida", "FL", SourceTypes.ArcGIS, "https://experience.arcgis.com/experience/96dd742462124fa0b38ddedb9b25e429", ""));

            Expression expression = new Expression();
            expression.FileName = "countycases.csv";
            StateInfos.Add(new StateScrapeInfo("Georgia", "GA", SourceTypes.Zip, "https://ga-covid19.ondemand.sas.com/docs/ga_covid_data.zip", JsonSerializer.Serialize(expression)));
            
            //StateInfos.Add(new StateScrapeInfo("Kentucky", "KY", SourceTypes.Custom, "https://kygeonet.maps.arcgis.com/apps/opsdashboard/index.html#/543ac64bc40445918cf8bc34dc40e334", ""));

            expression = new Expression();
            expression.XPath = "/html/body/div[1]/div[3]/div[5]/div/table[1]";
            StateInfos.Add(new StateScrapeInfo("Mississippi", "MS", SourceTypes.XPATH, "https://msdh.ms.gov/msdhsite/_static/14,0,420.html#Mississippi", JsonSerializer.Serialize(expression)));
            
            //StateInfos.Add(new StateScrapeInfo("North Carolina", "NC", SourceTypes.HTML, "https://covid19.ncdhhs.gov/dashboard/about-data", ""));
            //StateInfos.Add(new StateScrapeInfo("South Carolina", "SC", SourceTypes.HTML, "https://www.scdhec.gov/infectious-diseases/viruses/coronavirus-disease-2019-covid-19/sc-cases-county-zip-code-covid-19", ""));
            //StateInfos.Add(new StateScrapeInfo("Tennesee", "TN", SourceTypes.ArcGIS, "https://experience.arcgis.com/experience/885e479b688b4750837ba1d291b85aed", ""));
            
            StateInfos.Add(new StateScrapeInfo("Michigan", "MI", SourceTypes.Excel, "https://www.michigan.gov/documents/coronavirus/Cases_and_Deaths_by_County_2020-07-24_697248_7.xlsx", ""));
        }
    }

    public enum SourceTypes 
    {
        ArcGIS,
        Custom,
        Excel,
        HTML,
        XPATH,
        Zip
    }

}
