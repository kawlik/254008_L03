using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Parallelism
{
    public class ThreadedWork
    {
        //  threaded work storage
        public List<long>[] results;

        //  initialize threaded work mutex
        public ThreadedWork(int size)
        {
            this.results = new List<long>[size];
        }

        //  save work on results list
        public void SetWork(int index, List<long> partialResult)
        {
            this.results[index] = partialResult;
        }
    }

    public class Result
    {
        public double ElapsedTime;
        public long PrimesCount;

        public Result(double ElapsedTime, long PrimesCount)
        {
            this.ElapsedTime = ElapsedTime;
            this.PrimesCount = PrimesCount;
        }
    }

    public class Problem
    {
        private long max;
        private long min;

        public Problem(long max, long min = 2)
        {
            this.max = max;
            this.min = min;
        }

        /*  utilities
        /*   *   *   *   *   *   *   *   *   */

        public static bool IsPrimeNumber(long number)
        {
            //  initial cond
            if(number < 2) return false;

            //  set limit
            long limit = (long)Math.Sqrt(number);

            //  calc
            for (long i = 2; i <= limit; i++)
            {
                //  breaking cond
                if(number % i == 0) return false;
            }

            //  return true
            return true;
        }

        public static List<long> GetPrimeNumbersInRange(long minimum, long maximum)
        {
            //  set initial data
            List<long> primes = new List<long>();

            //  calc
            for (long i = minimum; i <= maximum; i++)
            {
                //  test current value
                if(IsPrimeNumber(i)) primes.Add(i);
            }

            //  return result
            return primes;
        }

        private static async Task<List<long>> GetPrimeNumbersInRangeAsync(long minimum, long maximum)
        {
            //  set initial 
            List<long> primes = new List<long>();

            //  count offsets
            var count = maximum - minimum + 1;

            //  return async task
            return await Task.Factory.StartNew(() =>
            {
                //  calc
                for (long i = minimum; i <= maximum; i++)
                {
                    //  test current value
                    if (IsPrimeNumber(i)) primes.Add(i);
                }

                //  return result
                return primes;
            });
        }

        /*  solutions
        /*   *   *   *   *   *   *   *   *   */

        //  basic version
        public Result GetViaOneThread()
        {
            //  watch start
            var watch = Stopwatch.StartNew();

            /*  start
            /*   *   *   *   *   *   *   *   */


            //  set result
            List<long> primes = Problem.GetPrimeNumbersInRange(this.min, this.max);


            /*  stop
            /*   *   *   *   *   *   *   *   */

            //  watch stop
            watch.Stop();

            //  return result
            return new Result(watch.Elapsed.TotalMilliseconds, primes.Count);
        }

        //  Parallel version
        public Result GetViaParallel(int numParallel)
        {
            //  watch start
            var watch = Stopwatch.StartNew();

            /*  start
            /*   *   *   *   *   *   *   *   */

            //  init parital results list

            var primes = new List<long>[numParallel];

            //  calc offset
            long offset = (this.max - this.min) / numParallel;

            //  execute in parallel
            Parallel.For(0, numParallel, i => primes[i] = Problem.GetPrimeNumbersInRange(i * offset, i + 1 < numParallel ? (i + 1) * offset - 1 : this.max));

            /*  stop
            /*   *   *   *   *   *   *   *   */

            //  watch stop
            watch.Stop();

            //  return result
            return new Result(watch.Elapsed.TotalMilliseconds, primes.Sum(partial => partial.Count));
        }

        //  Tasks version
        public async Task<Result> GetViaTasks(int numTasks)
        {
            //  watch start
            var watch = Stopwatch.StartNew();

            /*  start
            /*   *   *   *   *   *   *   *   */

            //  init parital results list
            var tasks = new Task<List<long>>[numTasks];

            //  calc offset
            long offset = (long)((this.max - this.min) / numTasks);

            //  exec
            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Problem.GetPrimeNumbersInRangeAsync(i * offset, i + 1 < numTasks ? (i + 1) * offset - 1 : this.max);
            }

            //  wait for result
            var primes = await Task.WhenAll(tasks);


            /*  stop
            /*   *   *   *   *   *   *   *   */

            //  watch stop
            watch.Stop();

            //  return result
            return new Result(watch.Elapsed.TotalMilliseconds, primes.Sum(partial => partial.Count));
        }

        //  Thread version
        public Result GetViaThreads(int numThreads)
        {
            //  watch start
            var watch = Stopwatch.StartNew();

            /*  start
            /*   *   *   *   *   *   *   *   */


            //  init parital results list
            Thread[] threads = new Thread[numThreads];
            ThreadedWork res = new ThreadedWork(numThreads);

            //  calc offset
            long offset = (this.max - this.min) / numThreads;

            //  exec
            for (int i = 0; i < numThreads; i++)
            {
                int index = i;

                threads[i] = new Thread(() => res.SetWork(index, Problem.GetPrimeNumbersInRange(index * offset, index + 1 < numThreads ? (index + 1) * offset - 1: this.max)));
            }

            //  start threads
            for (int i = 0; i < numThreads; i++) threads[i].Start();
            for (int i = 0; i < numThreads; i++) threads[i].Join();


            /*  stop
            /*   *   *   *   *   *   *   *   */

            //  watch stop
            watch.Stop();

            //  return result
            return new Result(watch.Elapsed.TotalMilliseconds, res.results.Sum(partial => partial.Count));
        }
    }
}
