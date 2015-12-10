// This code is licensed under the GPL v2.0 by Dyllon Gagnier and Ross DiMassino.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Model
{
    public class PeriodicTaskExecutor
    {
        private readonly Action toPerform;

        // These are just helper variables for properties since C# properties aren't volatile.
        private volatile bool stopped;

        /// <summary>
        /// The time to wait in milliseconds.
        /// </summary>
        private volatile int threadWait;

        /// <summary>
        /// This property is the wait time of this executor in seconds.
        /// </summary>
        public double ThreadWait
        {
            get
            {
                return this.threadWait / 1000.0;
            }
            set
            {
                this.threadWait = (int)(value * 1000.0);
            }
        }

        /// <summary>
        /// This property indicates whether this executor has been shut down or not. Once stopped,
        /// it can not be restarted.
        /// </summary>
        public bool Stopped
        {
            get { return this.stopped; }
            set
            {
                lock(this)
                {
                    if (this.stopped && !value)
                        throw new ArgumentException("Executor has been stopped and can not be restarted.");
                    this.stopped = value;
                }
            }
        }

        /// <summary>
        /// This creates a new executor that performs the specified action and then waits
        /// for the timeout, looping until stopped. This will all be done in a new thread.
        /// </summary>
        /// <param name="toPerform">The action to perform.</param>
        /// <param name="threadWait">The time to wait between executions in seconds.</param>
        public PeriodicTaskExecutor(Action toPerform, double threadWait)
        {
            this.toPerform = toPerform;
            this.ThreadWait = threadWait;
            new Thread(this.Run).Start();
        }

        /// <summary>
        /// This is an internal method that is run by a new thread.
        /// </summary>
        private void Run()
        {
            Stopwatch timer = new Stopwatch();
            while(!this.Stopped)
            {
                timer.Restart();
                this.toPerform();
                int sleepyTime = Math.Max(0, (int)(this.threadWait - timer.ElapsedMilliseconds));
                Thread.Sleep(sleepyTime);
            }
        }
    }
}
