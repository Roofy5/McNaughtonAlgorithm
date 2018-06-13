using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McNaughtonAlgorithm.Model
{
    public class Machine
    {
        public IList<Job> Jobs { get; private set; }
        public IList<int> Chunks { get; private set; }

        public Machine()
        {
            Jobs = new List<Job>();
            Chunks = new List<int>();
        }

        public int CalculateMaxTime()
        {
            return Jobs.Max(j => j.Number);
        }
        public int CalculateAllJobsTime()
        {
            return Jobs.Sum(j => j.Time);
        }
        public void AddJobToMachine(Job job)
        {
            Jobs.Add(job);
            for (int i = 0; i < job.Time; i++)
            {
                Chunks.Add(job.Number);
            }
        }
        public void AddPauseToMachine()
        {
            Chunks.Add(-1);
        }
        public int CurrentMachineTimeUsage()
        {
            return Chunks.Count;
        }
    }
}
