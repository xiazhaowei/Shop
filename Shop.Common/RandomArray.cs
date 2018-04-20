using System;
using Xia.Common.Extensions;

namespace Shop.Common
{
    public class RandomArray
    {
        /// <summary>
        /// 善心指数
        /// </summary>
        /// <returns></returns>
        public static decimal BenevolenceIndex()
        {
            var randomArray = new decimal[] {
                0.00011M,0.00012M,0.00013M,0.00014M,0.00015M,0.00016M,0.00017M,0.00018M,0.00019M,
                0.0002M,0.00021M,0.00022M,0.00023M,0.00024M,0.00025M,0.00026M,0.00027M,0.00028M,0.00029M,
                0.0003M,0.00031M};
            return new Random().NextItem(randomArray);
        }

        /// <summary>
        /// 新用户红包
        /// </summary>
        /// <returns></returns>
        public static decimal NewUserRedPacket()
        {
            var randomArray = new decimal[] {
                1M,1.22M,1.34M,1.4M,1.44M,1.5M, 1.8M, 1.83M,1.9M, 1.91M,1.99M,
                2M, 2.21M, 2.22M,2.31M,2.3M, 2.4M,2.45M,2.55M,2.66M,2.72M,2.73M,2.78M,2.79M,2.8M,2.9M,
                3M, 3.1M,3.22M,3.23M,3.3M,3.33M,3.55M,3.6M,3.7M,3.8M,3.9M,3.99M,
                4.01M, 4.2M,4.26M,
                5M};
            return new Random().NextItem(randomArray);
        }
        /// <summary>
        /// 新用户购物券
        /// </summary>
        /// <returns></returns>
        public static decimal NewUserShopCash()
        {
            return 5M;
        }

        /// <summary>
        /// 推荐用户奖励红包
        /// </summary>
        /// <returns></returns>
        public static decimal InvoteUserRedPacket()
        {
            return 5M;
        }

    }
}
