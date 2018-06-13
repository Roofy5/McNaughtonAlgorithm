using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McNaughtonAlgorithm.Model.Schedulers
{
    class McNaughtonScheduler : IScheduler
    {
        private IList<Job> _jobs;
        private IList<Machine> _machines;
        
        public McNaughtonScheduler()
        {
            _jobs = new List<Job>();
            _machines = new List<Machine>();
        }
        public McNaughtonScheduler(IList<Job> jobs, int numberOfMachines) : this()
        {
            _jobs = jobs;
            _machines = new List<Machine>();
            for (int i = 0; i < numberOfMachines; i++)
                _machines.Add(new Machine());
        }

        public IList<Machine> Schedule()
        {
            var cMax = CalculateCMax();

            foreach (var machine in _machines)
            {
                int i = 0;
                while (_jobs.Count > 0 /*&& i < _jobs.Count*/)
                {
                    //Job job = _jobs[i];
                    Job job = _jobs[0];
                    int machineUssage = machine.CurrentMachineTimeUsage();

                    if (machineUssage == cMax)
                        break;

                    if ((machineUssage + job.Time) > cMax)
                    {
                        int splitTime = cMax - machineUssage;
                        Job part1 = new Job()
                        {
                            Number = job.Number,
                            Time = splitTime
                        };
                        Job part2 = new Job()
                        {
                            Number = job.Number,
                            Time = job.Time - splitTime
                        };
                        machine.AddJobToMachine(part1);
                        job.Number = part2.Number;
                        job.Time = part2.Time;
                        continue;
                    }

                    machine.AddJobToMachine(job);
                    _jobs.RemoveAt(0);
                    //_jobs.RemoveAt(i++);
                }

                int restUssage = machine.CurrentMachineTimeUsage();
                if (restUssage == cMax)
                    continue;
                machine.AddJobToMachine(new Job(0, cMax - restUssage));
            }

            return _machines;
        }

        private int CalculateCMax()
        {
            var jobsAllTime = _jobs.Sum(j => j.Time);
            var longestJobTime = _jobs.Max(j => j.Time);

            var sum = jobsAllTime / _machines.Count;

            return sum > longestJobTime ? sum : longestJobTime;
        }
    }
}
