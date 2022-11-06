using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Processes
{
    internal class ModelViewProcesses : INotifyPropertyChanged
    {
        private readonly ModelProcesses _model = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ModelViewProcesses()
        {
            _model.ProcessesList = new ObservableCollection<Process>(Process.GetProcesses()).Where(HasPriorityModifiable);
        }

        public IEnumerable<Process> ProcessesList => _model.ProcessesList;

        public string? ProcessId => _model.ProcessId;
        public string? ProcessName => _model.ProcessName;
        public string? ProcessTotalActiveTime => _model.ProcessTotalActiveTime;

        private static bool HasPriorityModifiable(Process process)
        {
            try
            {
                process.PriorityClass = process.PriorityClass;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
