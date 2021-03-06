﻿
using System;

namespace UrlShortener.Services
{
    public static class StringGenerator
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        private readonly static Random _random = new Random();

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">Default length is 6</param>
        /// <returns>A random string</returns>
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
