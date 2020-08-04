using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Cdc.Covid.WebScraper.UI
{
    public sealed class MetadataViewModel : ObservableObject
    {
        private readonly ReportGenerator _generator = new ReportGenerator();
        private MetadataReport _selectedReport;

        public MetadataReport SelectedReport
        {
            get => _selectedReport;
            set
            {
                _selectedReport = value;
                RaisePropertyChanged(nameof(SelectedReport));
            }
        }

        public ObservableCollection<MetadataReport> Reports { get; private set; } = new ObservableCollection<MetadataReport>();

        public ICommand GenerateReportCommand { get { return new RelayCommand(GenerateReportCommandExecute, CanExecuteGenerateReportCommand); } }
        private bool CanExecuteGenerateReportCommand() => true;
        private void GenerateReportCommandExecute()
        {
            List<MetadataReport> reports = new List<MetadataReport>(); //_generator.Generate();
            
            foreach (var report in reports)
            {
                Reports.Add(report);
            }
        }
    }
}
