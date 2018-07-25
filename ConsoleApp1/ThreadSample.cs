using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class ThreadSample
    {
        public bool _isStooped = false;
        
        public void Stop()
        {
            _isStooped = true;
        }
        
        public void CountNumbers()
        {
            long counter = 0;

            while (!_isStooped)
                counter++;

            Console.WriteLine("{0} with {1, 11} priority " + "has a count {2, 13}", Thread.CurrentThread.Name, Thread.CurrentThread.Priority, counter.ToString("N0"));
        }

        public ThreadSample()
        {
        }
    }
}