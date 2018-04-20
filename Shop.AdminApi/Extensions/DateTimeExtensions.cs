using System;

namespace Shop.AdminApi.Utils
{
    public static class DateTimeExtensions
    {
        private static readonly long EpochMilliseconds = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).Ticks / 10000L;

        public static long ToEpochMilliseconds(this DateTime? time)
        {
            if (time.HasValue)
            {
                return (time.Value.Ticks / 10000L) - EpochMilliseconds;
            }

            return 0L;
        }
    }
}