﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cdc.Covid.WebScraper
{
    [DebuggerDisplay("{StateScrapeInfo}")]
    public sealed class StateScrapeInfo
    {
        public string State { get; private set; } = string.Empty;
        public string StateAbbreviation { get; private set; } = string.Empty;
        public SourceTypes SourceType { get; private set; } = SourceTypes.Custom;
        public string Source { get; private set; } = "";
        public Expression ExpressionObject { get; private set; } = new Expression();

        public DateTime DateSourceUpdated { get; private set; } = DateTime.MinValue;
        public List<string> Status { get; private set; } = new List<string>();

        public StateScrapeInfo(string state, string abbreviation, SourceTypes sourceType, string source, string expression)
        {
            State = state;
            StateAbbreviation = abbreviation;
            SourceType = sourceType;
            Source = source;
            if(string.IsNullOrEmpty(expression) == false)
            {
                try
                {
                    ExpressionObject = (Expression)JsonSerializer.Deserialize<Expression>(expression, null);
                }
                catch { }
            }
        }
    }
}
