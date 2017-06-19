using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UrlShortener.Services.Tests.Framework;

namespace UrlShortener.Services.Tests
{
    public class StringGeneratorTest
    {
        [Test, AutoMoqData]
        public void Generate_WhenLengthIsSpecified_ReturnsEqualsLength(int stringLength)
        {
            string result = StringGenerator.Generate(stringLength);

            Assert.Equals(stringLength, result.Length);
        }

        [TestCase(100000)]
        public void Generate_Performance_Check(int loopCount)
        {
            var timer = Stopwatch.StartNew();

            for (int i = 0; i <= loopCount; i++)
            {
                string result = StringGenerator.Generate();
            }

            timer.Stop();
            var timespan = timer.Elapsed;

            Console.WriteLine(string.Format("Completed in {0} ms", timespan.TotalMilliseconds));
            Assert.LessOrEqual(timespan.TotalMilliseconds, 30);
        }

        /// <summary>
        /// For 6 characters, 30k max count seems to be the safest bet.
        /// </summary>
        /// <param name="loopCount"></param>
        [TestCase(30000)]
        public void Generate_Collision_Check(int loopCount)
        {
            var hashSet = new HashSet<string>();
            var timer = Stopwatch.StartNew();

            for (int i = 0; i <= loopCount; i++)
            {
                string result = StringGenerator.Generate();

                Assert.IsFalse(hashSet.Contains(result), $"Duplicate string : {result} on {i} try");
                hashSet.Add(result);
            }

            timer.Stop();
            var timespan = timer.Elapsed;

            Console.WriteLine(string.Format("Completed in {0} ms", timespan.TotalMilliseconds));
        }
    }
}
