using ENode.Domain;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Users;
using Shop.Domain.Events.Users.ExpressAddresses;
using Shop.Domain.Events.Users.UserGifts;
using Shop.Domain.Models.Users.ExpressAddresses;
using System;
using System.Collections.Generic;
using System.Linq;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Users
{
    /// <summary>
    /// 用户聚合跟
    /// </summary>
    public class User:AggregateRoot<Guid>
    {
        private Guid _parentId;//推荐人
        private UserInfo _info;//用户信息
        private IList<ExpressAddress> _expressAddresses;
        private Guid _walletId;//用户钱包ID
        private Guid _cartId;//购物车ID
        private bool _isLocked;//是否锁定账号 只用于限制登陆
        private bool _isFreeze;//是否冻结账号 怀疑账号被盗可以冻结账号
        private UserRole _role;//用户角色
        private IDictionary<Guid,RecommandUserInfo> _myRecommends;//保存我推荐的直推用户
        private decimal _mySpending;//我的消费额
        private decimal _myHighProfitSpending;

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="info"></param>
        public User(Guid id, User parent, UserInfo info) : base(id)
        {
            if (parent != null && id == parent.Id)
            {
                throw new ArgumentException(string.Format("用户的推荐人不能是自己，Id:{0}", id));
            }
            ApplyEvent(new UserCreatedEvent(info, 
                parent == null ? Guid.Empty : parent.Id,
                Guid.NewGuid(),Guid.NewGuid()));
        }

        /// <summary>
        /// 清空用户的推荐人
        /// </summary>
        public void ClearParent()
        {
            ApplyEvent(new UserParentClearedEvent());
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="weixinId"></param>
        /// <param name="unionId"></param>
        public void BindWeixin(string weixinId,string unionId)
        {
            ApplyEvent(new UserBindedWeixinEvent(weixinId, unionId));
        }

        #region 基本信息修改
        public void Edit(string nickName,string gender,UserRole role)
        {
            nickName.CheckNotNullOrEmpty(nameof(nickName));
            if (nickName.Length > 20)
            {
                throw new Exception("昵称不得超过20字符");
            }
            if (!"男,女,保密".IsIncludeItem(gender))
            {
                throw new Exception("只接受参数值：男/女/保密");
            }
            ApplyEvent(new UserEditedEvent(nickName,gender,role));
            //if(role==UserRole.Ambassador)
            //{
            //    PayToAmbassador();
            //}
        }
        /// <summary>
        /// 更新昵称
        /// </summary>
        /// <param name="nickName"></param>
        public void UpdateNickName(string nickName )
        {
            nickName.CheckNotNullOrEmpty(nameof(nickName));
            if(nickName.Length>20)
            {
                throw new Exception("昵称不得超过20字符");
            }
            ApplyEvent(new UserNickNameUpdatedEvent(nickName));
        }

        public void UpdateInfo(string nickName,string region,string portrait)
        {
            ApplyEvent(new UserInfoUpdatedEvent(nickName,region,portrait));
        }


        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="password">HASHE</param>
        public void UpdatePassword(string password)
        {
            password.CheckNotNullOrEmpty(nameof(password));
            ApplyEvent(new UserPasswordUpdatedEvent(password));
        }

        /// <summary>
        /// 更新性别
        /// </summary>
        /// <param name="gender"></param>
        public void UpdateGender(string gender)
        {
            gender.CheckNotNullOrEmpty(nameof(gender));
            if (!"男,女,保密".IsIncludeItem(gender))
            {
                throw new Exception("只接受参数值：男/女/保密");
            }
            ApplyEvent(new UserGenderUpdatedEvent(gender));
        }
        /// <summary>
        /// 更新地区
        /// </summary>
        /// <param name="region"></param>
        public void UpdateRegion(string region)
        {
            region.CheckNotNullOrEmpty(nameof(region));  
            ApplyEvent(new UserRegionUpdatedEvent(region));
        }
        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="portrait"></param>
        public void UpdatePortrait(string portrait)
        {
            portrait.CheckNotNullOrEmpty(nameof(portrait));        
            ApplyEvent(new UserPortraitUpdatedEvent(portrait));
        }

        /// <summary>
        /// 锁定用户 限制登陆
        /// </summary>
        public void Lock()
        {
            if (_isLocked)
            {
                throw new Exception("用户早已锁定.");
            }            
            ApplyEvent(new UserLockedEvent());
        }
        /// <summary>
        /// 解锁用户
        /// </summary>
        public void UnLock()
        {
            if (!_isLocked)
            {
                throw new Exception("用户早已解算");
            }
            ApplyEvent(new UserUnLockedEvent());
        }

        /// <summary>
        /// 冻结用户 限制交易
        /// </summary>
        public void Freeze()
        {
            if (_isFreeze)
            {
                throw new Exception("用户早已冻结.");
            }
            ApplyEvent(new UserFreezeEvent(_walletId));
        }
        /// <summary>
        /// 解冻用户
        /// </summary>
        public void UnFreeze()
        {
            if (!_isFreeze)
            {
                throw new Exception("用户早已解冻");
            }
            ApplyEvent(new UserUnFreezeEvent(_walletId));
        }

        #endregion

        public void SetMyParent(User parent)
        {
            if (parent != null && _id == parent.Id)
            {
                throw new ArgumentException("用户的推荐人不能是自己");
            }
            ApplyEvent(new MyParentSetedEvent(parent.Id));
        }

        #region 获取信息
        public Guid GetWalletId()
        {
            return _walletId;
        }
        public UserInfo GetInfo()
        {
            return _info;
        }
        #endregion

        /// <summary>
        /// 接受新的升级订单--我报单
        /// </summary>
        public void AcceptNewUpdateOrder(int goodsCount,UpdateOrderType updateOrderType)
        {
            if(goodsCount == 0)
            {
                throw new Exception("没有升级订单");
            }
            if (_role == UserRole.Consumer)
            {
                //如果是普通会员则升级为Vip会员
                ApplyEvent(new UserRoleToPasserEvent());
            }
            if(_role == UserRole.Passer && updateOrderType == UpdateOrderType.VipPasserOrder)
            {
                //如果是经理升级单，升级到经理
                ApplyEvent(new UserRoleToVipPasserEvent());
            }
            if(_parentId!=Guid.Empty)
            {
                //待分配的奖金-岗位补贴
                var leftAwardAmount = 100M * goodsCount;
                if (updateOrderType == UpdateOrderType.VipPasserOrder)
                {
                    leftAwardAmount = 1000M * goodsCount;
                }
                ApplyEvent(new MyParentRecommandAPasserEvent(_parentId,_id,_role,
                    _id, _role,goodsCount, leftAwardAmount,1,updateOrderType));
            }
        }
        

        /// <summary>
        /// 接受被推荐人的升级订单 分发角色奖金
        /// </summary>
        /// <param name="goodsCount"></param>
        public void AcceptChildUpdateOrder(Guid newVipId,UserRole newVipRole,int goodsCount,decimal leftAwardAmount, int level,UpdateOrderType updateOrderType=UpdateOrderType.VipOrder)
        {
            if (goodsCount == 0)
            {
                throw new Exception("升级订单商品数量错误");
            }

            #region 用户推荐奖
            var leftAmount = leftAwardAmount;
            if (ConfigSettings.IsRecommandAward && leftAmount>0)//是否开启推荐奖
            {
                var gratefulAward = 0M;
                if (_role!=UserRole.Consumer && level == 1)
                {
                    //用户只要不是普通会员就拿直推奖
                    var directAward = Math.Round(ConfigSettings.DirectRecommandAward * goodsCount,2);
                    if (updateOrderType == UpdateOrderType.VipPasserOrder)
                    {
                        directAward = Math.Round(ConfigSettings.DirectRecommandVipPasserAward * goodsCount, 2);
                    }
                    #region 感恩奖
                    if (_parentId != Guid.Empty)
                    {
                        //推荐人获取感恩奖金
                        var directGratefulAward = Math.Round(directAward * ConfigSettings.GratefulAwardPersent, 2);
                        gratefulAward += directGratefulAward;
                        //用户部分直推奖
                        var getAward = Math.Round((directAward - directGratefulAward),2);
                        ApplyEvent(new UserDirectGetRecommandVipAwardEvent(_walletId, getAward));
                    }
                    else
                    {
                        //用户全部直推奖
                        ApplyEvent(new UserDirectGetRecommandVipAwardEvent(_walletId, directAward));
                    }
                    #endregion
                }
                if (_role==UserRole.VipPasser)//岗位补贴
                {
                    var perGoodsLeft = Math.Round(leftAwardAmount / goodsCount, 0);
                    if (perGoodsLeft == 100M || perGoodsLeft==1000M)//每个报单商品剩余100元或者1000元说明，没有经理拿过该奖金，我就拿我的奖金
                    {
                        //我拿的岗位补贴金额
                        var award = ConfigSettings.VipPasserAward * goodsCount;
                        if (updateOrderType == UpdateOrderType.VipPasserOrder)
                        {
                            award = ConfigSettings.VipPasserAward2 * goodsCount;
                        }
                        #region 感恩奖
                        if (_parentId != Guid.Empty)
                        {
                            //推荐人获取感恩奖金
                            var vipPasserGratefulAward = Math.Round((award * ConfigSettings.GratefulAwardPersent), 2);
                            gratefulAward += vipPasserGratefulAward;
                            var getAward = Math.Round((award - vipPasserGratefulAward), 2);
                            ApplyEvent(new UserGetRecommandVipAwardEvent(_walletId, getAward));
                        }
                        else
                        {
                            //我拿50 用户间接推荐奖励，没有层数限制
                            ApplyEvent(new UserGetRecommandVipAwardEvent(_walletId, award));
                        }
                        #endregion
                        leftAmount -= award;
                    }
                }
                if(_role==UserRole.Director)//岗位补贴
                {
                    #region 感恩奖
                    if (_parentId != Guid.Empty)
                    {
                        //推荐人获取感恩奖金
                        var directorDratefulAward = Math.Round((leftAmount * ConfigSettings.GratefulAwardPersent), 2);
                        gratefulAward += directorDratefulAward;
                        var getAward = Math.Round((leftAmount - directorDratefulAward), 2);
                        ApplyEvent(new UserGetRecommandVipAwardEvent(_walletId, getAward));
                    }
                    else
                    {
                        //总监拿剩下的所有金额
                        ApplyEvent(new UserGetRecommandVipAwardEvent(_walletId, leftAmount));
                    }
                    #endregion
                    leftAmount = 0;
                }
                //感恩奖分配
                if (gratefulAward>0)
                {
                    ApplyEvent(new MyParentCanGetGratefulAwardEvent(_parentId, gratefulAward, _info.Mobile + "的感恩奖"));
                }
                
            }
            #endregion

            #region 树的角色升级
            int myDirectRecommandPasserCount = 0;
            int myRecommandPasserCount = 0;
            var myDirectRecommandVipPasserCount = 0;
            var myRecommandVipPasserCount = 0;

            foreach (var kvp in _myRecommends )
            {
                if (kvp.Value.Role == UserRole.Passer)
                {
                    myRecommandPasserCount++;
                    if (kvp.Value.Level == 1)
                    {
                        myDirectRecommandPasserCount++;
                    }
                }
                if (kvp.Value.Role == UserRole.VipPasser)
                {
                    myRecommandVipPasserCount++;
                    if (kvp.Value.Level == 1)
                    {
                        myDirectRecommandVipPasserCount++;
                    }
                }
            }
            //如果我是普通消费者并且有超过10个直推VIP  或者报单为经理升级单
            if (myDirectRecommandPasserCount>=10 && myRecommandPasserCount>=60 && _role==UserRole.Passer)
            {
                //我是VIP 直推10个VIP 团队60VIP 升级为经理
                ApplyEvent(new UserRoleToVipPasserEvent());
            }

            //我是经理 直推10个经理 团队10个经理 升级为总监
            if(myDirectRecommandVipPasserCount>=10 && myRecommandVipPasserCount>=60 && _role==UserRole.VipPasser)
            {
                ApplyEvent(new UserRoleToDirectorEvent());
            }

            #endregion
            
            //继续递归
            ApplyEvent(new MyParentRecommandAPasserEvent(_parentId, _id,_role, 
                newVipId,newVipRole,goodsCount, leftAmount, level+1,updateOrderType));
            
        }
        


        #region 推荐用户
        /// <summary>
        /// 接受新的推荐者
        /// </summary>
        /// <param name="userId"></param>
        public void AcceptNewRecommend(Guid userId)
        {
            userId.CheckNotEmpty(nameof(userId));
            ApplyEvent(new InvotedNewUserEvent(userId, _walletId));
        }
        #endregion

        #region 用户消费结算逻辑
        /// <summary>
        /// 自己的新消费
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="storeAmount"></param>
        /// <param name="benevolence"></param>
        /// <param name="highProfitAmount">高倍利润商品金额</param>
        public void AcceptMyNewSpending(decimal amount,decimal storeAmount,decimal benevolence,decimal highProfitAmount)
        {
            if (amount <= 0) return;
            ApplyEvent(new UserNewSpendingEvent(amount));
            
            //用户消费转换为善心
            if (ConfigSettings.BenevolenceValue <= 0)
            {
                throw new Exception("善心价值配置异常");
            }
            //消费者获得的福豆量
            var benevolenceAmount = Math.Round(benevolence,4);
            if (_role==UserRole.Consumer)//如果是普通消费者获得的福豆打折
            {
                benevolenceAmount = Math.Round(benevolenceAmount * 0.8M, 4);
            }

            ApplyEvent(new UserSpendingTransformToBenevolenceEvent(_walletId, benevolenceAmount));

            if (highProfitAmount > 0)
            {
                //我消费了高利润商品
                ApplyEvent(new UserSpendingHighProfitGoodsEvent(highProfitAmount));
            }

            //计算我的推荐者的 间接福豆
            if(_parentId!=Guid.Empty)
            {
                //利润的福豆量
                var profitBenevolenceAmount = Math.Round((amount - storeAmount) / ConfigSettings.BenevolenceValue, 4);
                //如果我有推荐者，将我消费的信息广播给我的推荐者推荐者自己计算自己的一度或二度激励
                ApplyEvent(new MyParentCanGetBenevolenceEvent(_parentId, benevolenceAmount, profitBenevolenceAmount,highProfitAmount, 1));
            }
        }

        /// <summary>
        /// 我的推荐人可以获取一度二度福豆奖励
        /// </summary>
        /// <param name="benevolenceAmount"></param>
        public void MyParentCanGetBenevolence(decimal benevolenceAmount)
        {
            if(_parentId!=Guid.Empty)
            {
                //如果我有推荐者，将我消费的信息广播给我的推荐者推荐者自己计算自己的一度或二度激励
                ApplyEvent(new MyParentCanGetBenevolenceEvent(_parentId, benevolenceAmount, 0, 0, 1));
            }
        }
        /// <summary>
        /// 接受被推荐人的善心分成
        /// </summary>
        /// <param name="amount">购物者获得的善心量</param>
        /// <param name="level">层级</param>
        public void AcceptChildBenevolence(decimal amount,decimal profitBenevolenceAmount,decimal highProfitAmount,int level)
        {
            if (amount <= 0) return;
            if (level > 0 && level <= 2)//奖励16层
            {
                if (level == 1 && _role != UserRole.Consumer)//一度
                {
                    //用户消费达到高利润奖励
                    if (highProfitAmount>0 && _myHighProfitSpending >= ConfigSettings.HighProfitAwardThreshold)
                    {
                        var myHighProfitAmount = Math.Round(highProfitAmount * 0.1M, 2);
                        ApplyEvent(new UserGetChildCashEvent(_walletId, myHighProfitAmount, level));
                    }
                    else
                    {
                        //达不到高倍奖励走原来的奖励
                        var myamount = Math.Round(amount * 0.05M, 4);//我能获取的善心
                        if (myamount > 0.0001M)
                        {
                            ApplyEvent(new UserGetChildBenevolenceEvent(_walletId, myamount, level));
                        }
                    }
                }
                if (level == 2 && _role != UserRole.Consumer)//二度
                {
                    //用户消费达到高利润奖励
                    if (highProfitAmount > 0 && _myHighProfitSpending >= ConfigSettings.HighProfitAwardThreshold)
                    {
                        var myHighProfitAmount = Math.Round(highProfitAmount * 0.05M, 2);
                        ApplyEvent(new UserGetChildCashEvent(_walletId, myHighProfitAmount, level));
                    }
                    else
                    {
                        var myamount = Math.Round(amount * 0.025M, 4);//我能获取的善心
                        if (myamount > 0.0001M)
                        {
                            ApplyEvent(new UserGetChildBenevolenceEvent(_walletId, myamount, level));
                        }
                    }
                }
                //领导奖
                if (ConfigSettings.IsLeadershipAward && _role != UserRole.Consumer)
                {
                    decimal profitAmount = 0M;
                    if (level<=8)
                    {
                        profitAmount = Math.Round(profitBenevolenceAmount * ConfigSettings.Leadership1_8, 4);
                    }
                    else
                    {
                        profitAmount = Math.Round(profitBenevolenceAmount * ConfigSettings.Leadership8_16, 4);
                    }
                    //分配奖金
                    if (profitAmount > 0.0001M)
                    {
                        ApplyEvent(new UserGetChildProfitBenevolenceEvent(_walletId, profitAmount, level));
                    }
                }
                if (level < 2 && _parentId != Guid.Empty)
                {
                    //继续递归
                    ApplyEvent(new MyParentCanGetBenevolenceEvent(_parentId, amount, profitBenevolenceAmount,highProfitAmount, level + 1));
                }
            }

        }

        /// <summary>
        /// 接受被推荐人的感恩奖
        /// </summary>
        /// <param name="amount"></param>
        public void AcceptChildGratefulAward(decimal amount,string remark)
        {
            ApplyEvent(new UserGetChildGratefulAwardEvent(_walletId, amount,remark));
        }
        /// <summary>
        /// 我的店铺新销售，结算我的推荐者的收益
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="surrenderPersent"></param>
        public void AcceptMyStoreNewSale(decimal sale)
        {
            if (ConfigSettings.BenevolenceValue <= 0)
            {
                throw new Exception("善心价值参数设置异常");
            }

            //计算我的推荐者的收益 商家销售额
            if (_parentId!=Guid.Empty)
            {
                //现在拿现金奖励
                var parentCashGetAmount = Math.Round((sale * ConfigSettings.RecommandStoreGetPercent ), 4);
                ApplyEvent(new UserGetChildStoreSaleCashEvent(_parentId, parentCashGetAmount));
            }
        }

        public void AcceptChildStoreSaleBenevolence(decimal amount)
        {
            ApplyEvent(new AcceptedChildStoreSaleBenevolenceEvent(_walletId, amount));
        }
        public void AcceptChildStoreSaleCash(decimal amount)
        {
            ApplyEvent(new AcceptedChildStoreSaleCashEvent(_walletId, amount));
        }
        #endregion
        

        #region 快递地址
        /// <summary>
        /// 添加快递地址
        /// </summary>
        /// <param name="expressAddress"></param>
        public void AddExpressAddress(ExpressAddressInfo expressAddressInfo)
        {
            ApplyEvent(new ExpressAddressAddedEvent(Guid.NewGuid(),expressAddressInfo));
        }

        /// <summary>
        /// 更新 快递地址
        /// </summary>
        /// <param name="expressAddressId"></param>
        /// <param name="expressAddressInfo"></param>
        public void UpdateExpressAddress(Guid expressAddressId, ExpressAddressInfo expressAddressInfo)
        {
            var expressaddress = _expressAddresses.SingleOrDefault(x => x.Id == expressAddressId);
            if (expressaddress == null)
            {
                throw new Exception("不存在该收货地址.");
            }
            ApplyEvent(new ExpressAddressUpdatedEvent(expressAddressId, expressAddressInfo));
        }

        /// <summary>
        /// 删除快递地址
        /// </summary>
        /// <param name="expressAddressId"></param>
        public void RemoveExpressAddress(Guid expressAddressId)
        {
            var expressAddress= _expressAddresses.SingleOrDefault(x => x.Id == expressAddressId);
            if (expressAddress == null)
            {
                throw new Exception("不存在该收货地址.");
            }
            ApplyEvent(new ExpressAddressRemovedEvent(expressAddressId));
        }

        #endregion

        #region Event Handle Methods 通过事件更改聚合跟状态

        private void Handle(UserCreatedEvent evnt)
        {
            _info = evnt.Info;
            _parentId = evnt.ParentId;
            _expressAddresses = new List<ExpressAddress>();
            _isLocked = false;
            _isFreeze = false;
            _myRecommends = new Dictionary<Guid,RecommandUserInfo>();
            _role = UserRole.Consumer;
            _mySpending = 0;
            _myHighProfitSpending = 0;
            _walletId = evnt.WalletId;
            _cartId = evnt.CartId;
        }
        private void Handle(MyParentSetedEvent evnt)
        {
            _parentId = evnt.ParentId;
        }
        private void Handle(MyParentRecommandAPasserEvent evnt)
        {
            //修改直推角色，直推用户可能已经升级
            if (_myRecommends.ContainsKey(evnt.UserId))
            {
                _myRecommends[evnt.UserId].Role = evnt.UserRole;
            }
            //添加报单用户 针对非直推的情况,用户可能重复报单
            if (_myRecommends.ContainsKey(evnt.NewVipId))
            {
                _myRecommends[evnt.NewVipId].Role = evnt.NewVipRole;
            }
            else
            {
                _myRecommends.Add(evnt.NewVipId,new RecommandUserInfo(evnt.NewVipId, evnt.NewVipRole, evnt.Level));
            }
        }
        private void Handle(UserNickNameUpdatedEvent evnt)
        {
            _info.NickName = evnt.NickName;
        }
        private void Handle(UserEditedEvent evnt)
        {
            _info.NickName = evnt.NickName;
            _info.Gender = evnt.Gender;
            _role = evnt.Role;
        }
        private void Handle(UserBindedWeixinEvent evnt)
        {
            _info.WeixinId = evnt.WeixinId;
            _info.UnionId = evnt.UnionId;
        }
        private void Handle(UserGenderUpdatedEvent evnt)
        {
            _info.Gender = evnt.Gender;
        }
        private void Handle(UserRegionUpdatedEvent evnt)
        {
            _info.Region = evnt.Region;
        }
        private void Handle(UserPortraitUpdatedEvent evnt)
        {
            _info.Portrait = evnt.Portrait;
        }
        private void Handle(UserInfoUpdatedEvent evnt)
        {
            _info.NickName = evnt.NickName;
            _info.Portrait = evnt.Portrait;
            _info.Region = evnt.Region;
        }
        private void Handle(UserPasswordUpdatedEvent evnt)
        {
            _info.Password = evnt.Password;
        }
        private void Handle(UserLockedEvent evnt)
        {
            _isLocked = true;
        }
        private void Handle(UserParentClearedEvent evnt)
        {
            _parentId = Guid.Empty;
        }
        private void Handle(UserUnLockedEvent evnt)
        {
            _isLocked = false;
        }
        private void Handle(UserFreezeEvent evnt)
        {
            _isFreeze = true;
        }
        private void Handle(UserUnFreezeEvent evnt)
        {
            _isFreeze = false;
        }
        private void Handle(ExpressAddressAddedEvent evnt)
        {
            _expressAddresses.Add(new ExpressAddress(evnt.ExpressAddressId,evnt.Info));
        }
        private void Handle(ExpressAddressUpdatedEvent evnt)
        {
            _expressAddresses.Single(x => x.Id == evnt.ExpressAddressId).Info = evnt.Info;
        }
        private void Handle(ExpressAddressRemovedEvent evnt)
        {

            _expressAddresses.Remove(_expressAddresses.Single(x => x.Id == evnt.ExpressAddressId));
        }
        private void Handle(UserRoleToPasserEvent evnt)
        {
            _role = UserRole.Passer;
        }
        private void Handle(UserRoleToVipPasserEvent evnt)
        {
            _role = UserRole.VipPasser;
        }
        private void Handle(UserRoleToDirectorEvent evnt)
        {
            _role = UserRole.Director;
        }
        private void Handle(UserNewSpendingEvent evnt)
        {
            _mySpending += evnt.Amount;
        }
        private void Handle(InvotedNewUserEvent evnt)
        {
            //添加直推用户
            if (!_myRecommends.ContainsKey(evnt.UserId))
            {
                _myRecommends.Add(evnt.UserId,new RecommandUserInfo(evnt.UserId,UserRole.Consumer,1));
            }
        }

        private void Handle(UserGetChildBenevolenceEvent evnt) { }
        private void Handle(UserGetChildCashEvent evnt) { }
        private void Handle(UserGetChildProfitBenevolenceEvent evnt) { }
        private void Handle(MyParentCanGetBenevolenceEvent evnt) { }
        private void Handle(MyParentCanGetGratefulAwardEvent evnt) { }
        private void Handle(UserGetSaleBenevolenceEvent evnt) { }
        private void Handle(UserGetChildStoreSaleBenevolenceEvent evnt) { }
        private void Handle(UserGetChildStoreSaleCashEvent evnt) { }
        private void Handle(AcceptedChildStoreSaleBenevolenceEvent evnt) { }
        private void Handle(AcceptedChildStoreSaleCashEvent evnt) { }
        private void Handle(UserSpendingTransformToBenevolenceEvent evnt) { }
        private void Handle(UserSpendingHighProfitGoodsEvent evnt)
        {
            _myHighProfitSpending += evnt.Amount;
        }
        private void Handle(UserDirectGetRecommandVipAwardEvent evnt) { }
        private void Handle(UserGetRecommandVipAwardEvent evnt) { }
        private void Handle(UserRoleToAmbassadorEvent evnt) {}
        private void Handle(UserGiftAddedEvent evnt) { }
        private void Handle(UserGiftPayedEvent evnt) { }
        private void Handle(UserGiftRemarkChangedEvent evnt) { }
        private void Handle(UserGetChildGratefulAwardEvent evnt) { }
        #endregion
    }

    
}
