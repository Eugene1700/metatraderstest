using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace IpGeoInformer.Tests
{
    public static class Waiter
    {
        private static readonly Stack<int> delays = new Stack<int>();
        private static readonly Stack<int> timeouts = new Stack<int>();

        static Waiter()
        {
            timeouts.Push(DefaultTimeout);
            delays.Push(100);
        }

        public static int DefaultTimeout
        {
            get
            {
                var ci = Environment.GetEnvironmentVariable("KNAPP_CI");
                if (!string.IsNullOrWhiteSpace(ci) && ci == "1")
                    return 30000;
                return 15000;
            }
        }

        public static int Delay => delays.Peek();

        public static int Timeout => timeouts.Peek();

        public static bool Wait(Func<bool> tryFunc, string actionDescription = "Waiting for some action",
            int? timeoutMillis = null,
            Func<string> getState = null)
        {
            var timeoutMs = timeoutMillis ?? Timeout;
            return Wait(tryFunc, attempts => ThrowWaitException(timeoutMs, attempts,
                actionDescription + (getState == null ? "" : ", state [" + getState() + "]")), timeoutMs);
        }

        public static bool Wait(Func<bool> tryFunc, int timeoutMillis)
        {
            return Wait(tryFunc, attempts => ThrowWaitException(timeoutMillis, attempts), timeoutMillis);
        }

        public static bool Wait(Func<bool> tryFunc, Action<int> onWaitFailed)
        {
            return Wait(tryFunc, onWaitFailed, Timeout);
        }

        public static bool WaitIgnoringErrors(Func<bool> tryFunc, int timeoutMs, params Type[] exceptionTypes)
        {
            return Wait(tryFunc, attempts => ThrowWaitException(timeoutMs, attempts), timeoutMs, exceptionTypes);
        }

        public static void EnsureForDuration(Action action, TimeSpan duration)
        {
            Assert.That(!Wait(() =>
                {
                    action();
                    return false;
                },
                attempts => { },
                (int) duration.TotalMilliseconds));
        }

        public static bool Wait(Func<bool> tryFunc, Action<int> onWaitFailed, int timeoutMillis,
            params Type[] exceptionsToIgnore)
        {
            var w = Stopwatch.StartNew();
            var tryResult = false;
            var iteration = 0;
            do
            {
                try
                {
                    iteration++;

                    tryResult = tryFunc();
                    if (tryResult)
                        break;
                    Thread.Sleep(Delay);
                }
                catch (Exception e)
                {
                    if (!exceptionsToIgnore.Any(x => x.IsInstanceOfType(e)))
                        throw;
                }
            } while (w.ElapsedMilliseconds < timeoutMillis);

            if (!tryResult)
            {
                onWaitFailed(iteration);
                throw new Exception("onWaitFailed() callback should do Assert.Fail");
            }

            return true;
        }

        private static void ThrowWaitException(int timeoutMs, int attempts, string actionDescription = null)
        {
            var actionDescriptionOrEmpty = actionDescription == null ? "" : $" {actionDescription}";
            Assert.Fail($"действие{actionDescriptionOrEmpty} не выполнилось за {timeoutMs} мс, {attempts} попыток");
        }
    }
}