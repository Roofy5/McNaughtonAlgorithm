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
                //new Job(1, 50),
                //new Job(2, 1),
                //new Job(3, 10),
                //new Job(1, 50),
                //new Job(2, 1),
                //new Job(3, 10),
                //new Job(1, 50),
                //new Job(2, 1),
                //new Job(3, 10)
                /*new Job(1, 3),
                new Job(2, 6),
                new Job(3, 4),
                new Job(4, 1),
                new Job(5, 3)*/
                new Job(1, 5),
                new Job(2, 6),
                new Job(3, 7),
                new Job(4, 2),
                new Job(5, 1),
                new Job(6, 5),
                new Job(7, 2),
                new Job(8, 3),
                new Job(9, 4),
                new Job(10, 9),
                new Job(11, 1),
                new Job(12, 3),
                new Job(13, 3),
                new Job(14, 1),
                new Job(15, 1)
            };
            Machines = new ObservableCollection<Machine>();
            NumberOfMachines = 0;

            ScheduleCommand = new RelayCommand(Schedule);
        }

        private void Schedule(object parameter)
        {
            IList<Job> jobs = new List<Job>(_jobs);
            _scheduler = new McNaughtonScheduler(jobs, _numberOfMachines);
            Machines = _scheduler.Schedule();
        }
    }
}
