using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Cdc.Covid.WebScraper.UI
{
    public sealed class ReportViewModel : ObservableObject
    {
        private MetadataReport _metadataReport;
        private readonly ReportGenerator _generator = new ReportGenerator();
        private StateReport _selectedReport;
        
        public MetadataReport ScrapeMetadata
        {
            get => new MetadataReport();
            set
            {
                _metadataReport = value;
            }
        }

        public StateReport SelectedReport
        {
            get => _selectedReport;
            set
            {
                _selectedReport = value;
                RaisePropertyChanged(nameof(SelectedReport));
            }
        }

        public ObservableCollection<StateReport> Reports { get; private set; } = new ObservableCollection<StateReport>();
        public ObservableCollection<StateReport> SortedReports { get; private set; }

        public ICommand GenerateReportCommand { get { return new RelayCommand(GenerateReportCommandExecute, CanExecuteGenerateReportCommand); } }
        private bool CanExecuteGenerateReportCommand() => true;
        private void GenerateReportCommandExecute()
        {
            List<StateReport> reports = _generator.Generate();
            foreach (var report in reports)
            {
                Reports.Add(report);
            }
        }
    }
}
