﻿using System;
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

            _timer.Tick += RefreshProcessesCommand;
        }

        public IEnumerable<Process> ProcessesList => _model.ProcessesList;
        public ICommand RefreshProcesses => _model.RefreshProcesses;
        public ICommand ToggleAutomaticRefreshing => _model.ToggleRefreshing;

        public string? RefreshInterval
        {
            get => _model.RefreshInterval;
            set => _model.RefreshInterval = value;
        }

        public string? ProcessId => _model.ProcessId;
        public string? ProcessName => _model.ProcessName;
        public string? ProcessTotalActiveTime => _model.ProcessTotalActiveTime;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessesList)));
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessesList)));
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

    }
}