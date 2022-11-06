using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Processes
{
    internal class ModelViewProcesses : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new();
        private readonly ModelProcesses _model = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ModelViewProcesses()
        {
            _model.ProcessesList = new List<Process>(Process.GetProcesses()).Where(HasPriorityModifiable);

            _model.RefreshProcesses = new RelayCommand(RefreshProcessesCommand);
            _model.ToggleRefreshing = new RelayCommand(ToggleAutomaticRefreshingCommand);
            _model.SortProcessesList = new RelayCommand(SortProcessesListCommand);
            _model.ApplyPriorityClass = new RelayCommand(ApplyPriorityClassCommand);
            _model.KillProcess = new RelayCommand(KillProcessCommand);

            _timer.Tick += RefreshProcessesCommand;
        }

        public IEnumerable<Process> ProcessesList => _model.ProcessesList;

        public ICommand RefreshProcesses => _model.RefreshProcesses;
        public ICommand ToggleAutomaticRefreshing => _model.ToggleRefreshing;
        public ICommand SortProcessesList => _model.SortProcessesList;
        public ICommand ApplyPriorityClass => _model.ApplyPriorityClass;
        public ICommand KillProcess => _model.KillProcess;
        
        public string? FilterText
        {
            get => _model.FilterText;
            set
            {
                _model.FilterText = value;
                if (value == null) return;

                _model.ProcessesList = _model.ProcessesList.Where(x => x.ProcessName.Contains(value));
                OnPropertyUpdated(nameof(ProcessesList));
            }
        }

        public string? RefreshInterval
        {
            get => _model.RefreshInterval;
            set => _model.RefreshInterval = value;
        }

        public Process? SelectedProcess
        {
            get => _model.SelectedProcess;
            set
            {
                _model.SelectedProcess = value;
                OnPropertyUpdated(nameof(SelectedProcess));
            }
        }

        public ProcessPriorityClass SelectedPriority
        {
            get => _model.SelectedPriority;
            set
            {
                _model.SelectedPriority = value;
                OnPropertyUpdated(nameof(SelectedPriority));
            }
        }

        private void KillProcessCommand(object obj)
        {
            if (SelectedProcess == null) return;
            SelectedProcess.Kill();
            RefreshProcessesList();
        }

        private void ApplyPriorityClassCommand(object obj)
        {
            if (SelectedProcess == null) return;
            if (SelectedProcess.PriorityClass == SelectedPriority) return;

            SelectedProcess.PriorityClass = SelectedPriority;
            OnPropertyUpdated(nameof(ProcessesList));
        }

        private void SortProcessesListCommand(object obj)
        {
            _model.ProcessesList = _model.ProcessesList.OrderBy(x => x.ProcessName).ThenBy(x => x.Id);
            OnPropertyUpdated(nameof(ProcessesList));
        }

        private void ToggleAutomaticRefreshingCommand(object sender)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                return;
            }

            try
            {
                if (RefreshInterval != null)
                {
                    _timer.Interval = TimeSpan.FromSeconds(int.Parse(RefreshInterval));
                }
                else
                {
                    MessageBox.Show("No interval specified!");
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            _timer.Start();
            _model.ProcessesList = new List<Process>(Process.GetProcesses()).Where(HasPriorityModifiable);
            OnPropertyUpdated(nameof(ProcessesList));
        }

        private void RefreshProcessesCommand(object? sender, EventArgs e)
        {
            RefreshProcessesList();
        }

        private void RefreshProcessesCommand(object sender)
        {
            RefreshProcessesList();
        }

        private void RefreshProcessesList()
        {
            _model.ProcessesList = new List<Process>(Process.GetProcesses()).Where(HasPriorityModifiable);
            OnPropertyUpdated(nameof(ProcessesList));
        }

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

        private void OnPropertyUpdated(string? nameOfProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameOfProperty));
        }
    }
}
