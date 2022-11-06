using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace Processes
{
    internal class ModelProcesses
    {
        public IEnumerable<Process> ProcessesList;

        public string? ProcessId;
        public string? ProcessName;
        public string? ProcessTotalActiveTime;
        public string? RefreshInterval;

        public ICommand RefreshProcesses;
        public ICommand ToggleRefreshing;
        public ICommand SortProcessesList;
    }
}
