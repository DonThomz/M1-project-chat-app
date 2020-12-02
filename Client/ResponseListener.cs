using System;
using System.Threading;

namespace Client
{
    public class ResponseListener
    {
        public ResponseListener(string threadName)
        {
            var responseListenerThread = new Thread(Run);
            responseListenerThread.Name = threadName;
            responseListenerThread.Start();
        }

        private void Run()
        {
            Console.WriteLine("response listener is listening in thread {0}", Thread.CurrentThread.Name);
            //Thread.Sleep(10000);
            Close();
        }

        private void Close()
        {
            Console.WriteLine("response listener is stopping in thread {0}", Thread.CurrentThread.Name);
        }
    }
}