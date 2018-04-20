using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xia.Common.Extensions
{
    public static class SingleExtensions
    {
        /// <summary>
        /// 求平均
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Single Ave(this Single source,Single target)
        {
            return (source + target) / 2;
        }
    }
}
