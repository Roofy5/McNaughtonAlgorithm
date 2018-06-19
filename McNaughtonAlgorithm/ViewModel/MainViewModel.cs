using McNaughtonAlgorithm.Helpers;
using McNaughtonAlgorithm.Model;
using McNaughtonAlgorithm.Model.Loader;
using McNaughtonAlgorithm.Model.Schedulers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace McNaughtonAlgorithm.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private IScheduler _scheduler;
        private ILoader _loader;
        private IList<Job> _jobs;
        private IList<Machine> _machines;
        private int _numberOfMachines;

        public IList<Job> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; OnPropertyChanged(nameof(Jobs), nameof(CanExecuteScheduler)); }
        }
        public IList<Machine> Machines
        {
            get { return _machines; }
            set { _machines = value; OnPropertyChanged(nameof(Machines)); }
        }
        public int NumberOfMachines
        {
            get { return _numberOfMachines; }
            set { _numberOfMachines = value; OnPropertyChanged(nameof(NumberOfMachines), nameof(CanExecuteScheduler)); }
        }
        public bool CanExecuteScheduler
        {
            get { return NumberOfMachines > 0 && Jobs.Count > 0; }
            set { OnPropertyChanged(nameof(CanExecuteScheduler)); }
        }

        public ICommand ScheduleCommand { get; private set; }
        public ICommand DeleteJobCommand { get; private set; }
        public ICommand AddJobCommand { get; private set; }
        public ICommand LoadDataCommand { get; private set; }
        public ICommand SaveDataCommand { get; private set; }
        public ICommand InfoCommand { get; private set; }

        private EventHandler _drawImageHandler;
        public event EventHandler DrawImage
        {
            add { _drawImageHandler += value; }
            remove { _drawImageHandler -= value; }
        }

        public MainViewModel()
        {
            Jobs = new ObservableCollection<Job>()
            {
                new Job(1, 1)
            };
            Machines = new ObservableCollection<Machine>();
            NumberOfMachines = 1;
            _loader = new PlainTextLoader();

            ScheduleCommand = new RelayCommand(Schedule);
            DeleteJobCommand = new RelayCommand(DeleteJob);
            AddJobCommand = new RelayCommand(AddJob);
            LoadDataCommand = new RelayCommand(LoadData);
            SaveDataCommand = new RelayCommand(SaveData);
            InfoCommand = new RelayCommand(ShowInfo);
        }

        private void Schedule(object parameter)
        {
            IList<Job> jobs = new List<Job>();
            foreach (var job in _jobs)
                jobs.Add(new Job(job.Number, job.Time));

            _scheduler = new McNaughtonScheduler(jobs, _numberOfMachines);
            Machines = _scheduler.Schedule();
            _drawImageHandler?.Invoke(this, null);
        }

        private void DeleteJob(object obj)
        {
            Jobs.Remove((Job)obj);
            for (int i = 0; i < Jobs.Count; i++)
                Jobs[i].Number = i + 1;
            Jobs = new ObservableCollection<Job>(Jobs);
        }

        private void AddJob(object obj)
        {
            Jobs.Add(new Job(Jobs.Count+1, 1));
            OnPropertyChanged(nameof(CanExecuteScheduler));
        }

        private void LoadData(object obj)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "MPD files (*.mpd)|*.mpd";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    int machines = 0;
                    Jobs = _loader.LoadFromFile(dialog.FileName, out machines);
                    NumberOfMachines = machines;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Data cannot be loaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SaveData(object obj)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "MPD files (*.mpd)|*.mpd";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _loader.SaveToFile(dialog.FileName, Jobs, NumberOfMachines);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Data cannot be saved!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ShowInfo(object obj)
        {
            MessageBox.Show("Application simulates McNaughton algorithm. Created by Michał Gucwa @ 2018", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
