using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;

namespace Model
{
    /// <summary>
    /// This class is provided as a way to easily test how many times the counter is invoked.
    /// </summary>
    internal class Counter
    {
        public volatile int Count;
    }

    /// <summary>
    /// Summary description for ExecutorTest
    /// </summary>
    [TestClass]
    public class ExecutorTest
    { 
        /// <summary>
        /// This method tests that tasks are run multiple times by the executor.
        /// </summary>
        [TestMethod]
        public void TestRunsMultipleTimes()
        {
            PeriodicTaskExecutor executor = null;
            try
            {
                Counter count = new Counter();
                executor = new PeriodicTaskExecutor(() => count.Count++, 0);

                Stopwatch watch = new Stopwatch();
                watch.Start();

                // Give it one second to complete at maximum.
                while (watch.ElapsedMilliseconds < 1000)
                    if (count.Count >= 2)
                        return;
                Assert.Fail("Count was not incremented.");
            }
            finally
            {
                executor.Stopped = true;
            }
        }

        /// <summary>
        /// This method tests that the scheduler can be stopped.
        /// </summary>
        [TestMethod]
        public void CanStopScheduler()
        {
            PeriodicTaskExecutor exec = new PeriodicTaskExecutor(() => { }, 0);
            exec.Stopped = true;
            Assert.IsTrue(exec.Stopped);
        }

        /// <summary>
        /// This method tests that the exec scheduler can change its time.
        /// </summary>
        [TestMethod]
        public void CanChangeSchedulerTime()
        {
            PeriodicTaskExecutor exec = null;
            try
            {
                Counter count = new Counter();
                exec = new PeriodicTaskExecutor(() => count.Count++, 0);

                exec.ThreadWait = 0.1;
                count.Count = 0;
                Thread.Sleep(100);

                // This is slightly non-deterministic, so give it a little margin for error.
                Assert.IsTrue(count.Count < 3);
            }
            finally
            {
                exec.Stopped = true;
            }
        }

        /// <summary>
        /// This method checks that an exeception is thrown when a PeriodicThreadExecutor has been
        /// stopped and then a user attempts to set Stopped to true.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IllegalThreadRestart()
        {
            PeriodicTaskExecutor exec = new PeriodicTaskExecutor(() => { }, 0);
            exec.Stopped = true;
            try
            {
                exec.Stopped = false;
            }
            finally
            {
                exec.Stopped = true;
            }
        }

        /// <summary>
        /// This method tests that the ThreadWait time is correctly computed in seconds.
        /// </summary>
        [TestMethod]
        public void TestGetThreadWait()
        {
            PeriodicTaskExecutor exec = null;
            try
            {
                exec = new PeriodicTaskExecutor(()=> { }, 2.5);
                Assert.AreEqual(2.5, exec.ThreadWait, 0.0001);
            }
            finally
            {
                exec.Stopped = true;
            }
        }
    }
}
