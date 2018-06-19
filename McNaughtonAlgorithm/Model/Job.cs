using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McNaughtonAlgorithm.Model
{
    public class Job
    {
        private int _time;
        public int Number { get; set; }
        public int Time
        {
            get { return _time; }
            set { _time = value < 0 ? value * (-1) : value; }
        }

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
