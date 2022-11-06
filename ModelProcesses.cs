using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace Processes
{
    internal class ModelProcesses
    {
        public IEnumerable<Process> ProcessesList;

        public string? RefreshInterval;
        public string? FilterText;

        public Process? SelectedProcess;

        public ICommand RefreshProcesses;
        public ICommand ToggleRefreshing;
        public ICommand SortProcessesList;
        public ICommand FilterProcessList;
        
    }
}
