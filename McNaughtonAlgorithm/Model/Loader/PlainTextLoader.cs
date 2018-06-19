using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McNaughtonAlgorithm.Model.Loader
{
    class PlainTextLoader : ILoader
    {
        public IList<Job> LoadFromFile(string file, out int numberOfMachines)
        {
            var result = new List<Job>();
            var loadedData = String.Empty;
            using (StreamReader reader = new StreamReader(file))
            {
                loadedData = reader.ReadToEnd();
            }

            var values = loadedData.Split(';');
            var first = true;
            numberOfMachines = 0;
            foreach (var value in values)
            {
                int time = 0;
                if (int.TryParse(value, out time))
                {
                    if (first)
                    {
                        numberOfMachines = time;
                        first = false;
                    }
                    else
                        result.Add(new Job() { Number = result.Count + 1, Time = time });
                }
            }
            return result;
        }

        public void SaveToFile(string file, IList<Job> jobs, int numberOfMachines)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write("{0};", numberOfMachines);
                foreach (var job in jobs)
                    writer.Write("{0};", job.Time);
            }
        }
    }
}
