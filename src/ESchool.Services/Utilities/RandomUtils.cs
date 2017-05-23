using System;

namespace ESchool.Services.Utilities
{
    public static class RandomUtils
    {
        private const string PASSWORD_CHARS_NUMERIC = "0123456789";

        public static string Numberic(int size)
        {
            return RandomString(PASSWORD_CHARS_NUMERIC, size);
        }

        private static string RandomString(string allowChars, int size)
        {
            int length = allowChars.Length;
            char[] valueArray = new char[size];

            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                valueArray[i] = allowChars[random.Next(length)];
            }

            return new string(valueArray);
        }
    }
}
