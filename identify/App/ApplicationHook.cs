using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace identify.App
{
    class ApplicationHook
    {
        private List<Process> PreviouslyCheckedRunning { get; set; }
        public ApplicationHook()
        {
            //Initialize the starting list.
            PreviouslyCheckedRunning = new List<Process>();
        }
        
        public IEnumerable<Process> CheckRunningProcesses()
        {
            List<Process> currentCheckRunning = new List<Process>();
            foreach (var process in Process.GetProcesses())
            {
                currentCheckRunning.Add(process);
            }

            var newlyRunningProcesses = CompareProcesses(currentCheckRunning, PreviouslyCheckedRunning);

            PreviouslyCheckedRunning.Clear();
            PreviouslyCheckedRunning.AddRange(currentCheckRunning);
            return newlyRunningProcesses;
        }
        
        private IEnumerable<Process> CompareProcesses(IEnumerable<Process> currentlyRunningProcesses, IEnumerable<Process> previouslyRunningProcesses)
        {
            if (previouslyRunningProcesses != null)
            {
                IEnumerable<Process> newlyExecutedProcesses = currentlyRunningProcesses.Where(x => !previouslyRunningProcesses.Any(y => y.ProcessName == x.ProcessName));
                return newlyExecutedProcesses; // Differences in process to show new processes.   
            }
            return currentlyRunningProcesses;
        }
    }
}
