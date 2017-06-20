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

            Assert.AreEqual(stringLength, result.Length);
        }

        [TestCase(100000)]
        public void Generate_Performance_IsLessThan50ms(int loopCount)
        {
            var timer = Stopwatch.StartNew();

            for (int i = 0; i < loopCount; i++)
            {
                string result = StringGenerator.Generate();
            }

            timer.Stop();
            var timespan = timer.Elapsed;

            Console.WriteLine(string.Format("Completed in {0} ms", timespan.TotalMilliseconds));
            Assert.LessOrEqual(timespan.TotalMilliseconds, 50);
        }
        
        [TestCase(100000)]
        public void Generate_Collision_IsLessThanOnePercent(int loopCount)
        {
            int collision = 0;
            var hashSet = new HashSet<string>();
            var timer = Stopwatch.StartNew();

            for (int i = 0; i < loopCount; i++)
            {
                string result = StringGenerator.Generate();

                if (hashSet.Contains(result))
                {
                    collision++;
                }
                else
                {
                    hashSet.Add(result);
                }
            }

            timer.Stop();
            var timespan = timer.Elapsed;

            // Stats
            var collisionPercent = collision / (double)loopCount * 100;
            Console.WriteLine($"Ran {loopCount} with {collision} ({collisionPercent}%) collision.");
            Console.WriteLine($"Completed in {timespan.TotalMilliseconds} ms");

            Assert.Less(collisionPercent, 1);
        }
    }
}
