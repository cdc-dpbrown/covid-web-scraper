using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cdc.Covid.WebScraper
{
    [DebuggerDisplay("{StateScrapeInfo}")]
    public sealed class StateScrapeInfo
    {
        public string State { get; private set; } = string.Empty;
        public string StateAbbreviation { get; private set; } = string.Empty;
        public string SourceType { get; private set; } = "";
        public string Source { get; private set; } = "";
        public string Expression { get; private set; } = "";

        public DateTime DateSourceUpdated { get; private set; } = DateTime.MinValue;
        public List<string> Status { get; private set; } = new List<string>();

        public StateScrapeInfo(string state, string abbreviation, string sourceType, string source, string expression)
        {
            State = state;
            StateAbbreviation = abbreviation;
            SourceType = sourceType;
            Source = source;
            Expression = expression;
        }
    }
}
