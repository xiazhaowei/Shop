using System;

namespace Xia.Common
{
    /// <summary>
    /// 随机字符生成
    /// </summary>
    public static class StringGenerator
    {
        private static readonly Random rnd = new Random(DateTime.UtcNow.Millisecond);
        private static readonly char[] allowableChars = "ABCDEFGHJKMNPQRSTUVWXYZ123456789".ToCharArray();

        /// <summary>
        /// 生产指定长度的随机字符
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string Generate(int length)
        {
            var result = new char[length];
            lock (rnd)
            {
                for (int i = 0; i < length; i++)
                {
                    result[i] = allowableChars[rnd.Next(0, allowableChars.Length)];
                }
            }

            return new string(result);
        }
    }
}
