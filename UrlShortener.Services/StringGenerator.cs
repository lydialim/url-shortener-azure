
using System;

namespace UrlShortener.Services
{
    public static class StringGenerator
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        private readonly static Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Generate(int length = 6)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = Alphabet[_random.Next(Alphabet.Length)];
            }

            return new string(chars);
        }
    }
}
