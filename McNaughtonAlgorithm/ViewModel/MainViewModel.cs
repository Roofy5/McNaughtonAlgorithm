using McNaughtonAlgorithm.Helpers;
using McNaughtonAlgorithm.Model;
using McNaughtonAlgorithm.Model.Schedulers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace McNaughtonAlgorithm.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private IScheduler _scheduler;
        private IList<Job> _jobs;
        private IList<Machine> _machines;
        private int _numberOfMachines;

        public IList<Job> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; OnPropertyChanged(nameof(Jobs)); }
        }
        public IList<Machine> Machines
        {
            get { return _machines; }
            set { _machines = value; OnPropertyChanged(nameof(Machines)); }
        }
        public int NumberOfMachines
        {
            get { return _numberOfMachines; }
            set { _numberOfMachines = value; OnPropertyChanged(nameof(NumberOfMachines)); }
        }

        public ICommand ScheduleCommand { get; private set; }


        public MainViewModel()
        {
            Jobs = new ObservableCollection<Job>()
            {
                new Job(1, 50),
                new Job(2, 1),
                new Job(3, 10),
                new Job(1, 50),
                new Job(2, 1),
                new Job(3, 10),
                new Job(1, 50),
                new Job(2, 1),
                new Job(3, 10)
            };
            Machines = new ObservableCollection<Machine>();
            NumberOfMachines = 0;

            ScheduleCommand = new RelayCommand(Schedule);
        }

        private void Schedule(object parameter)
        {
            
        }
    }
}
