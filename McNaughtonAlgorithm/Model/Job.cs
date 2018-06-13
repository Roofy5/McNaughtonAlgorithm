using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McNaughtonAlgorithm.Model
{
    public class Job
    {
        public int Number { get; set; }
        public int Time { get; set; }

        public Job() : this(-1, -1)
        {
        }
        public Job(int number, int time)
        {
            this.Number = number;
            this.Time = time;
        }
    }
}
