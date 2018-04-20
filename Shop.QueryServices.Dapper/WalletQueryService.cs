using Dapper;
using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.Common.Enums;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// Q端查询服务 Dapper
    /// </summary>
    [Component]
    public class WalletQueryService: BaseQueryService,IWalletQueryService
    {
        /// <summary>
        /// 钱包信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Wallet Info(Guid id)
        {
            var sql = string.Format(@"select 
                a.*,
                b.Mobile as OwnerMobile 
                from {0} as a inner join {1} as b on a.UserId=b.Id 
                where a.Id='{2}'",
                ConfigSettings.WalletTable,
                ConfigSettings.UserTable,
                id
            );

            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<Wallet>(sql);
            }
        }
        public Wallet InfoByUserId(Guid userId)
        {
            var sql = string.Format(@"select 
                a.*,
                b.Mobile as OwnerMobile 
                from {0} as a inner join {1} as b on a.UserId=b.Id 
                where a.UserId='{2}'",
                ConfigSettings.WalletTable,
                ConfigSettings.UserTable,
                userId
            );

            using (var connection = GetConnection())
            {
                return connection.QueryFirstOrDefault<Wallet>(sql);
            }
           
        }

        /// <summary>
        /// 待分配善心量
        /// </summary>
        /// <returns></returns>
        public decimal TotalBenevolence()
        {
            using (var connection = GetConnection())
            {
                var stores = connection.QueryList<Wallet>(new { }, ConfigSettings.WalletTable);
                return stores.Sum(x => x.Benevolence);
            }
        }

        /// <summary>
        /// 获取用户的激励信息
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<IncentiveInfo> GetIncentiveInfos(Guid walletId, int count)
        {
            var sql = string.Format(@"select top {0}
                a.CreatedOn,
                a.Amount as BenevolenceAmount,
                a.Remark,
                b.Amount,
                b.Fee 
                from {1} as a inner join {2} as b on a.Number=b.Number 
                where a.Type={3} and a.WalletId='{4}' and b.WalletId='{4}' 
                order by a.CreatedOn desc" ,
                count,
                ConfigSettings.BenevolenceTransferTable,
                ConfigSettings.CashTransferTable,
                Convert.ToInt32(BenevolenceTransferType.Incentive),
                walletId
            );

            using (var connection = GetConnection())
            {
                return connection.Query<IncentiveInfo>(sql);
            }
        }

        public IEnumerable<WalletAlis> BenevolenceRank(int count)
        {
            var sql = string.Format(@"select top {0} 
                a.Id,
                a.UserId,
                a.Cash,
                a.ShopCash,
                a.Benevolence,
                a.Earnings,
                b.NickName,
                b.Portrait,
                b.Mobile 
                from {1} as a inner join {2} as b on a.Id=b.WalletId 
                order by a.Benevolence desc",
               count,
               ConfigSettings.WalletTable,
               ConfigSettings.UserTable
           );

            using (var connection = GetConnection())
            {
                return connection.Query<WalletAlis>(sql);
            }
        }

        #region 现金记录
        
        /// <summary>
        /// 获取用户现金记录
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public IEnumerable<CashTransfer> GetCashTransfers(Guid walletId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<CashTransfer>(new { WalletId = walletId }, ConfigSettings.CashTransferTable);
            }
        }
        public IEnumerable<CashTransfer> GetCashTransfers(Guid walletId,CashTransferType type)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<CashTransfer>(new { WalletId = walletId,Type=(int)type }, ConfigSettings.CashTransferTable);
            }
        }

        #endregion

        #region 购物券记录

        /// <summary>
        /// 获取用户现金记录
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public IEnumerable<ShopCashTransfer> GetShopCashTransfers(Guid walletId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ShopCashTransfer>(new { WalletId = walletId }, ConfigSettings.ShopCashTransferTable);
            }
        }
        public IEnumerable<ShopCashTransfer> GetShopCashTransfers(Guid walletId, ShopCashTransferType type)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<ShopCashTransfer>(new { WalletId = walletId, Type = (int)type }, ConfigSettings.ShopCashTransferTable);
            }
        }

        #endregion

        #region 福豆记录
        /// <summary>
        /// 获取用户善心记录
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers(Guid walletId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<BenevolenceTransfer>(new { WalletId = walletId }, ConfigSettings.BenevolenceTransferTable);
            }
        }
        public IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<BenevolenceTransfer>(new {}, ConfigSettings.BenevolenceTransferTable);
            }
        }
        public IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers(Guid walletId, BenevolenceTransferType type)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<BenevolenceTransfer>(new { WalletId = walletId, Type = (int)type }, ConfigSettings.BenevolenceTransferTable);
            }
        }
        public IEnumerable<BenevolenceTransfer> GetBenevolenceTransfers(BenevolenceTransferType type)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<BenevolenceTransfer>(new {Type = (int)type }, ConfigSettings.BenevolenceTransferTable);
            }
        }
        #endregion

        public IEnumerable<Wallet> ListPage()
        {
            var sql = string.Format(@"select a.Id,
            a.UserId,
            a.AccessCode,
            a.Cash,
            a.ShopCash,
            a.LockedCash,
            a.Benevolence,
            a.YesterdayEarnings,
            a.YesterdayIndex,
            a.Earnings,
            a.BenevolenceTotal,
            a.TodayBenevolenceAdded,
            a.TodayWithdrawAmount,
            a.WeekWithdrawAmount,
            a.IsFreeze,
            b.Mobile as OwnerMobile 
            from {0} as a inner join {1} as b on a.UserId=b.Id", ConfigSettings.WalletTable, ConfigSettings.UserTable);

            using (var connection = GetConnection())
            {
                return connection.Query<Wallet>(sql);
            }
        }

        public IEnumerable<Wallet> AllWallets()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Wallet>(new { }, ConfigSettings.WalletTable);
            }
        }


        public IEnumerable<WithdrawApply> WithdrawApplyLogs(Guid walletId)
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format(@"select 
                    a.*,
                    b.Mobile,
                    b.NickName 
                    from {0} as a inner join {1} as b on a.WalletId=b.WalletId 
                    where a.WalletId='{2}' 
                    order by a.CreatedOn desc", 
                    ConfigSettings.WithdrawApplysTable, ConfigSettings.UserTable,walletId);
                return connection.Query<WithdrawApply>(sql);
            }
        }
        public IEnumerable<WithdrawApply> WithdrawApplyLogs()
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format(@"select 
                    a.*,
                    b.Mobile,
                    b.NickName 
                    from {0} as a inner join {1} as b on a.WalletId=b.WalletId 
                    order by a.CreatedOn desc",
                    ConfigSettings.WithdrawApplysTable, ConfigSettings.UserTable);
                return connection.Query<WithdrawApply>(sql);
            }
        }

        public IEnumerable<RechargeApply> RechargeApplyLogs(Guid walletId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<RechargeApply>(new { WalletId = walletId }, ConfigSettings.RechargeApplysTable);
            }
        }
        public IEnumerable<RechargeApply> RechargeApplyLogs()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<RechargeApply>(new { }, ConfigSettings.RechargeApplysTable);
            }
        }

        /// <summary>
        /// 获取银行卡
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public IEnumerable<BankCard> GetBankCards(Guid walletId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<BankCard>(new { WalletId = walletId }, ConfigSettings.BankCardTable).ToList();
            }
        }
    }
}
