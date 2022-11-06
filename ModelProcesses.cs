using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processes
{
    internal class ModelProcesses
    {
        public IEnumerable<Process> ProcessesList;

        public string? ProcessId;
        public string? ProcessName;
        public string? ProcessTotalActiveTime;
    }
}
