using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //  set problem
            Problem problem = new Problem(10000000);


            //  get solutions
            var result1 = problem.GetViaOneThread();
            var result2 = problem.GetViaParallel(4);
            var result3 = problem.GetViaParallel(8);
            var result4 = problem.GetViaParallel(12);
            var result5 = problem.GetViaParallel(16);
            var result6 = await problem.GetViaTasks(4);
            var result7 = await problem.GetViaTasks(8);
            var result8 = await problem.GetViaTasks(12);
            var result9 = await problem.GetViaTasks(16);
            var resultA = problem.GetViaThreads(4);
            var resultB = problem.GetViaThreads(8);
            var resultC = problem.GetViaThreads(12);
            var resultD = problem.GetViaThreads(16);


            //  print results
            Console.WriteLine($"Basic           :: time: { result1.ElapsedTime } ms \t| primes count: { result1.PrimesCount }");
            Console.WriteLine($"Parallel (4)    :: time: { result2.ElapsedTime } ms \t| primes count: { result2.PrimesCount }");
            Console.WriteLine($"Parallel (8)    :: time: { result3.ElapsedTime } ms \t| primes count: { result3.PrimesCount }");
            Console.WriteLine($"Parallel (12)   :: time: { result4.ElapsedTime } ms \t| primes count: { result4.PrimesCount }");
            Console.WriteLine($"Parallel (16)   :: time: { result5.ElapsedTime } ms \t| primes count: { result5.PrimesCount }");
            Console.WriteLine($"Tasks (4)       :: time: { result6.ElapsedTime } ms \t| primes count: { result6.PrimesCount }");
            Console.WriteLine($"Tasks (8)       :: time: { result7.ElapsedTime } ms \t| primes count: { result7.PrimesCount }");
            Console.WriteLine($"Tasks (12)      :: time: { result8.ElapsedTime } ms \t| primes count: { result8.PrimesCount }");
            Console.WriteLine($"Tasks (16)      :: time: { result9.ElapsedTime } ms \t| primes count: { result9.PrimesCount }");
            Console.WriteLine($"Threads (4)     :: time: { resultA.ElapsedTime } ms \t| primes count: { resultA.PrimesCount }");
            Console.WriteLine($"Threads (8)     :: time: { resultB.ElapsedTime } ms \t| primes count: { resultB.PrimesCount }");
            Console.WriteLine($"Threads (12)    :: time: { resultC.ElapsedTime } ms \t| primes count: { resultC.PrimesCount }");
            Console.WriteLine($"Threads (16)    :: time: { resultD.ElapsedTime } ms \t| primes count: { resultD.PrimesCount }");


            //  defer
            Console.Read();
        }
    }
}
