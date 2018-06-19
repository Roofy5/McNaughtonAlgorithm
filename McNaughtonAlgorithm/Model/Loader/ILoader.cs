using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McNaughtonAlgorithm.Model.Loader
{
    interface ILoader
    {
        IList<Job> LoadFromFile(string file, out int numberOfMachines);
        void SaveToFile(string file, IList<Job> jobs, int numberOfMachines);
    }
}
