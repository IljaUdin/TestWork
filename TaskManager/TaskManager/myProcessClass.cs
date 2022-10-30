using System;
using System.Diagnostics;
using System.Linq;


namespace TaskManager
{
    internal class myProcessClass
    {
        string[] _lastProcesses = new string[0];
        public string[] getProcess(string inform)
        {
            var processes = Process.GetProcesses();

            string[] result;

            if (inform == "list")
                result = processes.Select(p => string.Format(p.ProcessName + "  (" + p.Id + " Id)")).ToArray();
            else
                result = processes.Select(p => string.Format(p.ProcessName + " " +
                                             Convert.ToInt32(p.PagedMemorySize64) / (1000 * 1000) + " MB " +
                                                             p.Responding)).ToArray();
            if (result.SequenceEqual(_lastProcesses))
                return null;

            return _lastProcesses = result;
        }
    }
}
