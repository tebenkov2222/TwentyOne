using System;

namespace RussianMunchkin.Common
{
    public static class UidGenerator
    {
        private const int _lenghtPassword = 5;
        private static string _latters = "QWERTYASDFGHZXCVBNUIOOPJKLM9871236540";
        private static int _count => _latters.Length;
        public static string GenerateUid(int id)
        {
            var now = DateTime.Now;
            string res = "";
            res += GetLatter(now.Second);
            res += GetLatter(id + now.Day);
            res += GetLatter(now.Hour);
            res += GetLatter(id/_count);
            res += GetLatter(now.Minute);
            res += GetLatter(id%_count + id);
            res += GetLatter(id + now.Second);
            return res;
        }
        public static string GeneratePassword()
        {
            Random random = new Random();
            string res = "";
            for (int i = 0; i < _lenghtPassword; i++)
            {
                res += random.Next(0, 10);
            }

            return res;
        }
        private static char GetLatter(int value)
        {
            return _latters[value%_count];
        }
    }
}