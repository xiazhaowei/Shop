CREATE TABLE [dbo].[Users] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [ParentId]            UNIQUEIDENTIFIER NOT NULL,
    [WalletId]            UNIQUEIDENTIFIER NOT NULL,
    [CartId]            UNIQUEIDENTIFIER NOT NULL,
    [Mobile]    NVARCHAR (15)     NOT NULL,
    [NickName]     NVARCHAR (20)   NOT NULL,
    [Portrait]    NVARCHAR (300)   NOT NULL,
    [Password]          NVARCHAR (200)   NOT NULL,
	[Gender]			NVARCHAR(50) NOT NULL,
	[Role]	INT		NOT NULL,
    [Region]          NVARCHAR (100)   NOT NULL,
    [IsLocked]   BIT              NOT NULL,
	[IsFreeze]		BIT			NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[WeixinId]			NVARCHAR(200)  NULL,
	[UnionId]			NVARCHAR(200)  NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[UserMobiles] (
    [IndexId]      NVARCHAR (32)    NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Mobile]         NVARCHAR (15)   NOT NULL,
    PRIMARY KEY CLUSTERED ([IndexId] ASC)
)
GO
CREATE TABLE [dbo].[UserExpressAddresses] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (70)    NOT NULL,
	[Mobile]             NVARCHAR (15)   NOT NULL,   
    [Region]       NVARCHAR (250)   NOT NULL,
	[Address]       NVARCHAR (250)   NOT NULL,
	[Zip]       NVARCHAR (10)    NULL,   
    [CreatedOn]       DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[UserGifts] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [GiftName]              NVARCHAR (100)    NOT NULL,
    [GiftSize]              NVARCHAR (100)    NOT NULL,
    [Name]              NVARCHAR (70)    NOT NULL,
	[Mobile]             NVARCHAR (15)   NOT NULL,   
    [Region]       NVARCHAR (250)   NOT NULL,
	[Address]       NVARCHAR (250)   NOT NULL,
	[Zip]       NVARCHAR (10)   NOT NULL,   
	[Remark]       NVARCHAR (100)   NOT NULL,   
    [CreatedOn]       DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[Carts] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [GoodsCount]      INT NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[CartGoodses] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [CartId]      UNIQUEIDENTIFIER NOT NULL,
    [StoreId]      UNIQUEIDENTIFIER NOT NULL,
    [GoodsId]      UNIQUEIDENTIFIER NOT NULL,
    [SpecificationId]      UNIQUEIDENTIFIER NOT NULL,
    [GoodsName]              NVARCHAR (200)    NOT NULL,
    [GoodsPic]              NVARCHAR (200)     NULL,
    [SpecificationName]              NVARCHAR (200)    NOT NULL,
    [Price]               DECIMAL (18, 2)  NOT NULL,
    [OriginalPrice]               DECIMAL (18, 2)  NOT NULL,
    [Stock]               INT  NOT NULL,
    [Quantity]               INT  NOT NULL,
	[Benevolence]	DECIMAL (18, 4)  NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[CartId] ASC,[StoreId] ASC,[GoodsId] ASC,[SpecificationId] ASC)
)

GO
CREATE TABLE [dbo].[Categorys] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [ParentId]      UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (100)    NOT NULL,
    [Thumb]              NVARCHAR (200)     NULL,
    [Url]              NVARCHAR (200)     NULL,
    [Type]              INT   NOT NULL,
	[IsShow]   BIT              NOT NULL,
    [Sort]      INT              NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[PubCategorys] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [ParentId]      UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (100)    NOT NULL,
    [Thumb]              NVARCHAR (200)     NULL,
	[IsShow]   BIT              NOT NULL,
    [Sort]      INT              NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[Announcements] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Title]              NVARCHAR (100)    NOT NULL,
    [Body]              NVARCHAR (MAX)    NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[Goodses] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[StoreId]		UNIQUEIDENTIFIER NOT NULL,
	[Pics]		NVARCHAR(MAX)	NOT NULL,
    [Name]     NVARCHAR (200)   NOT NULL,
    [Description]    NVARCHAR (MAX)   NOT NULL,
    [Price]               DECIMAL (18, 2)  NOT NULL,
    [OriginalPrice]               DECIMAL (18, 2)  NOT NULL,
    [Benevolence]               DECIMAL (18, 2)  NOT NULL,
    [Stock]      INT              NOT NULL,
    [SellOut]      INT              NOT NULL,
    [IsPayOnDelivery]   BIT              NOT NULL,
    [IsInvoice]   BIT              NOT NULL,
    [Is7SalesReturn]   BIT              NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Sort]      INT              NOT NULL,
	[Rate]               DECIMAL (18, 2)  NOT NULL,
	[RateCount]               INT  NOT NULL,
	[QualityRate]               DECIMAL (18, 2)  NOT NULL,
	[PriceRate]               DECIMAL (18, 2)  NOT NULL,
	[ExpressRate]               DECIMAL (18, 2)  NOT NULL,
	[DescribeRate]               DECIMAL (18, 2)  NOT NULL,
    [IsPublished]   BIT              NOT NULL,
    [Status]   INT              NOT NULL,
	[RefusedReason]		NVARCHAR (200)   NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[GoodsPubCategorys] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [GoodsId]      UNIQUEIDENTIFIER NOT NULL,
    [CategoryId]      UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[GoodsId] ASC,[CategoryId] ASC)
)
GO
CREATE TABLE [dbo].[GoodsComments] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[GoodsId]		UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
    [Body]    NVARCHAR (400)   NOT NULL,
	[CreatedOn]       DATETIME         NOT NULL,
    [Rate]               FLOAT  NOT NULL,
    [PriceRate]               FLOAT  NOT NULL,
    [DescribeRate]               FLOAT  NOT NULL,
    [QualityRate]               FLOAT  NOT NULL,
    [ExpressRate]               FLOAT  NOT NULL,
    [Thumbs]      NVARCHAR(MAX)              NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[GoodsParams] (
	[Id]                UNIQUEIDENTIFIER NOT NULL,
	[GoodsId]		UNIQUEIDENTIFIER NOT NULL,
    [Name]    NVARCHAR (200)   NOT NULL,
    [Value]    NVARCHAR (200)   NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
)



GO
CREATE TABLE [dbo].[Grantees] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[Publisher]		UNIQUEIDENTIFIER NOT NULL,
	[Title]    NVARCHAR (400)   NOT NULL,
    [Descriptions]    NVARCHAR (MAX)   NOT NULL,
	[Pics]	NVARCHAR(MAX)	NOT NULL,
	[Max] DECIMAL(18,2)	NOT NULL,
	[Days]	INT NOT NULL,
	[ExpiredOn]       DATETIME         NOT NULL,
	[HelpCount]		INT		NOT NULL,
    [Goods]               DECIMAL (18, 2)	 NOT NULL,
    [Total]               DECIMAL (18, 2)		 NOT NULL,
	[CreatedOn]		DATETIME		NOT NULL,
    [Status]      INT              NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[Publisher] ASC)
)

GO
CREATE TABLE [dbo].[GranteeMoneyHelps] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[GranteeId]		UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[Amount] DECIMAL(18,2)	NOT NULL,
	[CreatedOn]		DATETIME		NOT NULL,
    [Says]      NVARCHAR(400)               NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[GranteeTestifys] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[GranteeId]		UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(200)		NOT NULL,
	[Relationship] NVARCHAR(200)		NOT NULL,
	[Mobile] NVARCHAR(100)		NOT NULL,
	[Remark] NVARCHAR(400)		NOT NULL,
	[CreatedOn]		DATETIME		NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)


GO
CREATE TABLE [dbo].[Partners] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[WalletId]		UNIQUEIDENTIFIER NOT NULL,
	[Mobile]	NVARCHAR(200)	 NULL,
	[Region]	NVARCHAR(200)	NOT NULL,
	[Level]		INT	NOT NULL,
	[Persent]    DECIMAL (18,4)   NOT NULL,
	[CashPersent]    DECIMAL (18,2)   NOT NULL,
	[BalanceInterval]		INT	NOT NULL,
	[LastCashBalancedAmount]    DECIMAL (18,2)   NOT NULL,
	[LastBenevolenceBalancedAmount]    DECIMAL (18,4)   NOT NULL,
	[LastBalancedAmount]    DECIMAL (18,2)   NOT NULL,
	[TotalCashBalancedAmount]    DECIMAL (18,2)   NOT NULL,
	[TotalBenevolenceBalancedAmount]    DECIMAL (18,4)   NOT NULL,
	[TotalBalancedAmount]    DECIMAL (18,2)   NOT NULL,
    [BalancedDate]    DATETIME   NOT NULL,
	[CreatedOn]    DATETIME   NOT NULL,
	[Remark]	NVARCHAR(400)	 NULL,
	[IsLocked]		INT	NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[UserId] ASC,[WalletId] ASC)
)
GO
CREATE TABLE [dbo].[PartnerBalanceLogs] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [PartnerId]            UNIQUEIDENTIFIER NOT NULL,
	[WalletId]		UNIQUEIDENTIFIER NOT NULL,
	[Region]    NVARCHAR (200)   NOT NULL,
	[Amount]		DECIMAL(18,2)	NOT NULL,
	[BalanceAmount]		DECIMAL(18,2)	NOT NULL,
	[CashAmount]		DECIMAL(18,2)	NOT NULL,
	[BenevolenceAmount]		DECIMAL(18,4)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[PartnerId] ASC,[WalletId] ASC)
)
GO
CREATE TABLE [dbo].[Stores] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[AccessCode]    NVARCHAR (200)   NOT NULL,
	[Name]    NVARCHAR (200)   NOT NULL,
	[Description]    NVARCHAR (400)   NOT NULL,
	[Region]    NVARCHAR (200)   NOT NULL,
	[Address]    NVARCHAR (400)   NOT NULL,
	[SubjectName]    NVARCHAR (400)    NULL,
	[SubjectNumber]    NVARCHAR (400)    NULL,
	[SubjectPic]    NVARCHAR (400)    NULL,
	[TodaySale]		DECIMAL(18,2)	NOT NULL,
	[TotalSale]		DECIMAL(18,2)	NOT NULL,
	[TodayOrder]		INT	NOT NULL,
	[TotalOrder]		INT	NOT NULL,
	[OnSaleGoodsCount]		INT	NOT NULL,
	[UpdatedOn]    DATETIME   NOT NULL,
	[CreatedOn]    DATETIME   NOT NULL,
	[ReturnAddressName]    NVARCHAR (200)    NULL,
	[ReturnAddress]    NVARCHAR (200)    NULL,
	[ReturnAddressMobile]    NVARCHAR (200)    NULL,
	[Type]		INT	NOT NULL,
	[Status]		INT	NOT NULL,
	[IsLocked]		INT	NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[UserId] ASC)
)
GO
CREATE TABLE [dbo].[OfflineStores] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[Name]    NVARCHAR (200)   NOT NULL,
	[Thumb]    NVARCHAR (400)   NOT NULL,
	[Phone]    NVARCHAR (100)    NULL,
	[Description]    NVARCHAR (400)   NOT NULL,
	[Labels]    NVARCHAR (400)   NOT NULL,
	[Region]    NVARCHAR (200)   NOT NULL,
	[Address]    NVARCHAR (400)   NOT NULL,
	[Persent]		DECIMAL(18,2)	NOT NULL,
	[Longitude]		DECIMAL(10,7)	NOT NULL,
	[Latitude]		DECIMAL(10,7)	NOT NULL,
	[TodaySale]		DECIMAL(18,2)	NOT NULL,
	[TotalSale]		DECIMAL(18,2)	NOT NULL,
	[UpdatedOn]    DATETIME   NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
	[IsLocked]		INT	NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[UserId] ASC)
)
GO
CREATE TABLE [dbo].[OfflineStoreSaleLogs] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [OfflineStoreId]            UNIQUEIDENTIFIER NOT NULL,
	[UserWalletId]		UNIQUEIDENTIFIER NOT NULL,
	[StoreOwnerWalletId]		UNIQUEIDENTIFIER NOT NULL,
	[StoreName]    NVARCHAR (200)   NOT NULL,
	[Region]    NVARCHAR (200)   NOT NULL,
	[Address]    NVARCHAR (400)   NOT NULL,
	[Amount]		DECIMAL(18,2)	NOT NULL,
	[StoreAmount]		DECIMAL(18,2)	NOT NULL,
	[UserBenevolence]		DECIMAL(18,4)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[OfflineStoreId] ASC,[UserWalletId] ASC,[StoreOwnerWalletId] ASC)
)
GO
CREATE TABLE [dbo].[StoreOrders] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[OrderId]		UNIQUEIDENTIFIER NOT NULL,
	[StoreId]		UNIQUEIDENTIFIER NOT NULL,
	[WalletId]		UNIQUEIDENTIFIER NOT NULL,
	[StoreOwnerWalletId]		UNIQUEIDENTIFIER NOT NULL,
	[Region]    NVARCHAR (200)  NOT	NULL,
	[Number]    NVARCHAR (200)  NOT  NULL,
	[Remark]    NVARCHAR (200)    NULL,
	[ExpressRegion]    NVARCHAR (200)  NOT  NULL,
	[ExpressAddress]    NVARCHAR (200) NOT   NULL,
	[ExpressName]    NVARCHAR (200)  NOT  NULL,
	[ExpressMobile]    NVARCHAR (200)  NOT  NULL,
	[ExpressZip]    NVARCHAR (200)  NOT  NULL,
	[DeliverExpressName]    NVARCHAR (200)    NULL,
	[DeliverExpressCode]    NVARCHAR (200)    NULL,
	[DeliverExpressNumber]    NVARCHAR (200)    NULL,
	[DeliverTime]    DATETIME     NULL,
	[ReturnDeliverExpressName]    NVARCHAR (200)    NULL,
	[ReturnDeliverExpressCode]    NVARCHAR (200)    NULL,
	[ReturnDeliverExpressNumber]    NVARCHAR (200)    NULL,
	[ReturnDeliverTime]    DATETIME     NULL,
    [CreatedOn]    DATETIME   NOT NULL,
	[Total]		DECIMAL(18,2)	NOT NULL,
	[ShopCash]		DECIMAL(18,2)	NOT NULL,
	[StoreTotal]		DECIMAL(18,2)	NOT NULL,
	[Reason] NVARCHAR (200)    NULL,
	[RefundAmount]		DECIMAL(18,2)	 NULL,
	[ApplyRefundTime]    DATETIME     NULL,
	[AgreeReturnTime]    DATETIME     NULL,
    [Status] INT              NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[OrderGoodses] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[OrderId]		UNIQUEIDENTIFIER NOT NULL,
	[GoodsId]		UNIQUEIDENTIFIER NOT NULL,
	[SpecificationId]		UNIQUEIDENTIFIER NOT NULL,
	[WalletId]		UNIQUEIDENTIFIER NOT NULL,
	[StoreOwnerWalletId]		UNIQUEIDENTIFIER NOT NULL,
	[GoodsName]	NVARCHAR(400)	NOT NULL,
	[GoodsPic]	NVARCHAR(400)	NOT NULL,
	[SpecificationName]	NVARCHAR(400)	NOT NULL,
	[Quantity]		INT	NOT NULL,
	[Price]	DECIMAL(18,2)		NOT NULL,
	[OriginalPrice]	DECIMAL(18,2)		NOT NULL,
	[Total]	DECIMAL(18,2)		NOT NULL,
	[ShopCash]	DECIMAL(18,2)		NOT NULL,
	[StoreTotal]	DECIMAL(18,2)		NOT NULL,
	[Benevolence]	DECIMAL(18,4)		NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    [ServiceExpirationDate]    DATETIME   NOT NULL,
	[Status]		INT	NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[ApplyServices] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[OrderGoodsId]		UNIQUEIDENTIFIER NOT NULL,
	[ServiceNumber]	NVARCHAR(200)	NOT NULL,
	[Reason]	NVARCHAR(400)	NOT NULL,
	[Remark]	NVARCHAR(400)	NOT NULL,
	[Quantity]		INT	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[ServiceExpresses] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[OrderGoodsId]		UNIQUEIDENTIFIER NOT NULL,
	[ServiceNumber]	NVARCHAR(200)	NOT NULL,
	[ExpressName]	NVARCHAR(200)	NOT NULL,
	[ExpressNumber]	NVARCHAR(200)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)


GO
CREATE TABLE [dbo].[StoreSections] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[Name]	NVARCHAR(200)	NOT NULL,
	[Description]	NVARCHAR(400)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[Wallets] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
	[AccessCode]	NVARCHAR(200)	NOT NULL,
	[Cash]	DECIMAL(18,2)	NOT NULL,
	[LockedCash]	DECIMAL(18,2)	NOT NULL,
	[Benevolence]	DECIMAL(18,4)	NOT NULL,
	[ShopCash]	DECIMAL(18,2)	NOT NULL,
	[YesterdayEarnings]	DECIMAL(18,2)	NOT NULL,
	[Earnings]	DECIMAL(18,2)	NOT NULL,
	[YesterdayIndex]	DECIMAL(18,4)	NOT NULL,
	[BenevolenceTotal]	DECIMAL(18,4)	NOT NULL,
	[TodayBenevolenceAdded]	DECIMAL(18,4)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    [UpdatedOn]    DATETIME   NOT NULL,
	[TodayWithdrawAmount]	DECIMAL(18,2)	NOT NULL,
	[WeekWithdrawAmount]	DECIMAL(18,2)	NOT NULL,
	[WithdrawTotalAmount]	DECIMAL(18,2)	NOT NULL,
	[LastWithdrawTime]    DATETIME   NOT NULL,
	[IsFreeze]		BIT			NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[UserId] ASC)
)

GO
CREATE TABLE [dbo].[BenevolenceIndexIncentives] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[BIndex]	DECIMAL(18,5)	NOT NULL,
	[BenevolenceAmount]	DECIMAL(18,4)	NOT NULL,
	[IncentivedAmount]	DECIMAL(18,4)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[BankCards] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[BankName]	NVARCHAR(200)	NOT NULL,
	[OwnerName]	NVARCHAR(200)	NOT NULL,
	[Number]	NVARCHAR(200)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[WithdrawApplys] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[Amount]	DECIMAL(18,2)	NOT NULL,
	[BankName]	NVARCHAR(200)	NOT NULL,
	[BankOwner]	NVARCHAR(200)	NOT NULL,
	[BankNumber]	NVARCHAR(200)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
	[Remark]	NVARCHAR(200)	NOT NULL,
    [Status]    INT   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[RechargeApplys] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[Amount]	DECIMAL(18,2)	NOT NULL,
	[Pic]	NVARCHAR(200)	 NULL,
	[BankName]	NVARCHAR(200)	NOT NULL,
	[BankOwner]	NVARCHAR(200)	NOT NULL,
	[BankNumber]	NVARCHAR(200)	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
	[Remark]	NVARCHAR(200)	NOT NULL,
    [Status]    INT   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[CashTransfers] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[Number]	NVARCHAR(200)	NOT NULL,
	[Amount]	DECIMAL(18,2)	NOT NULL,
	[Fee]	DECIMAL(18,2)	NOT NULL,
	[Direction]	INT	NOT NULL,
	[Remark]	NVARCHAR(200)	 NULL,
	[Type]	INT	NOT NULL,
	[FinallyValue]	DECIMAL(18,2)	NOT NULL,
	[Status]		INT	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[ShopCashTransfers] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[Number]	NVARCHAR(200)	NOT NULL,
	[Amount]	DECIMAL(18,2)	NOT NULL,
	[Fee]	DECIMAL(18,2)	NOT NULL,
	[Direction]	INT	NOT NULL,
	[Remark]	NVARCHAR(200)	 NULL,
	[Type]	INT	NOT NULL,
	[FinallyValue]	DECIMAL(18,2)	NOT NULL,
	[Status]		INT	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[BenevolenceTransfers] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[Number]	NVARCHAR(200)	NOT NULL,
	[Amount]	DECIMAL(18,4)	NOT NULL,
	[Fee]	DECIMAL(18,4)	NOT NULL,
	[Direction]	INT	NOT NULL,
	[Remark]	NVARCHAR(200)	 NULL,
	[Type]	INT	NOT NULL,
	[FinallyValue]	DECIMAL(18,4)	NOT NULL,
	[Status]		INT	NOT NULL,
    [CreatedOn]    DATETIME   NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE TABLE [dbo].[Specifications] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [GoodsId]      UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (200)    NOT NULL,
    [Value]              NVARCHAR (200)    NOT NULL,
    [Thumb]       NVARCHAR (250)    NULL,
    [Price]               DECIMAL (18, 2)  NOT NULL,
    [OriginalPrice]               DECIMAL (18, 2)  NOT NULL,
    [Benevolence]               DECIMAL (18, 4)  NOT NULL,
    [Stock]      INT              NOT NULL,
	[Number]              NVARCHAR (70)     NULL,
	[BarCode]              NVARCHAR (70)     NULL,
    [AvailableQuantity] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[ReservationItems] (
    [GoodsId]  UNIQUEIDENTIFIER NOT NULL,
    [ReservationId] UNIQUEIDENTIFIER NOT NULL,
    [SpecificationId]    UNIQUEIDENTIFIER NOT NULL,
    [Quantity]      INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([GoodsId] ASC, [ReservationId] ASC, [SpecificationId] ASC)
)
GO
CREATE TABLE [dbo].[Orders] (
    [OrderId]                   UNIQUEIDENTIFIER NOT NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
	[ExpressRegion]    NVARCHAR (200)  NOT  NULL,
	[ExpressAddress]    NVARCHAR (200) NOT   NULL,
	[ExpressName]    NVARCHAR (200)  NOT  NULL,
	[ExpressMobile]    NVARCHAR (200)  NOT  NULL,
	[ExpressZip]    NVARCHAR (200)  NOT  NULL,
    [Status]                    INT              NOT NULL,
    [Total]               DECIMAL (18, 2)  NOT NULL,
    [ShopCash]               DECIMAL (18, 2)  NOT NULL,
    [StoreTotal]               DECIMAL (18, 2)  NOT NULL,
    [ReservationExpirationDate] DATETIME         NULL,
    [Version]                   BIGINT           NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
)
GO
CREATE TABLE [dbo].[OrderLines] (
    [OrderId]      UNIQUEIDENTIFIER NOT NULL,
    [GoodsId]   UNIQUEIDENTIFIER NOT NULL,
    [StoreId]   UNIQUEIDENTIFIER NOT NULL,
    [SpecificationId]   UNIQUEIDENTIFIER NOT NULL,
    [GoodsName] NVARCHAR (400)   NULL,
    [GoodsPic] NVARCHAR (400)   NULL,
    [SpecificationName] NVARCHAR (400)   NULL,
    [Price]    DECIMAL (18, 2)  NOT NULL,
    [OriginalPrice]    DECIMAL (18, 2)  NOT NULL,
	[Benevolence]    DECIMAL (18, 4)  NOT NULL,
    [Quantity]     INT              NOT NULL,
    [LineTotal]    DECIMAL (18, 2)  NOT NULL,
    [StoreLineTotal]    DECIMAL (18, 2)  NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC,[GoodsId] ASC,[StoreId] ASC, [SpecificationId] ASC)
)
GO
CREATE TABLE [dbo].[Payments] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [State]       INT              NOT NULL,
    [OrderId]     UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [TotalAmount] DECIMAL (18, 2)  NOT NULL,
    [ShopCashAmount] DECIMAL (18, 2)  NOT NULL,
    [Version]     BIGINT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[PaymentItems] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [Amount]      DECIMAL (18, 2)  NOT NULL,
    [PaymentId]   UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[GoodsBlocks] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (200)   NOT NULL,
    [Thumb]      NVARCHAR (400)  NOT NULL,
    [Banner]      NVARCHAR (400)  NULL,
    [Layout]      INT              NOT NULL,
    [IsShow]   BIT              NOT NULL,
    [Sort]      INT              NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[GoodsBlockGoodses] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [GoodsBlockId]          UNIQUEIDENTIFIER NOT NULL,
    [GoodsId]          UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[GoodsBlockWarps] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (200)    NULL,
    [Style]      INT              NOT NULL,
    [IsShow]   BIT              NOT NULL,
    [Sort]      INT              NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[GoodsBlockWarpGoodsBlocks] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [GoodsBlockWarpId]          UNIQUEIDENTIFIER NOT NULL,
    [GoodsBlockId]          UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[Admins] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (200)  NOT  NULL,
    [LoginName] NVARCHAR (200)  NOT  NULL,
    [Portrait] NVARCHAR (200)    NULL,
    [Password] NVARCHAR (200)  NOT  NULL,
    [Role]      INT              NOT NULL,
    [IsLocked]   BIT              NOT NULL,
    [CreatedOn]       DATETIME         NOT NULL,
	[Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[AdminOperatRecords] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [AdminId]          UNIQUEIDENTIFIER NOT NULL,
    [AboutId]          UNIQUEIDENTIFIER NOT NULL,
    [AdminName] NVARCHAR (200)  NOT  NULL,
    [Operat] NVARCHAR (200)  NOT  NULL,
    [Remark] NVARCHAR (400)    NULL,
    [CreatedOn]       DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[AdminId] ASC)
)
GO
CREATE TABLE [dbo].[Notifications] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[UserId]		UNIQUEIDENTIFIER NOT NULL,
	[Mobile]	NVARCHAR(200)	NOT NULL,
	[WeixinId]	NVARCHAR(200)	 NULL,
	[Title]	NVARCHAR(200)	NOT NULL,
	[Body]	NVARCHAR(400)	NOT NULL,
	[Type]		INT	NOT NULL,
	[AboutId]		UNIQUEIDENTIFIER NOT NULL,
	[Remark]	NVARCHAR(400)	 NULL,
	[CreatedOn]       DATETIME         NOT NULL,
	[IsSmsed]		INT	NOT NULL,
	[IsMessaged]		INT	NOT NULL,
	[AboutObjectStream] NVARCHAR (MAX)   NULL,
	[IsRead]		INT	NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[UserId] ASC)
)
GO
CREATE TABLE [dbo].[ThirdCurrencys] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
	[Name]	NVARCHAR(200)	NOT NULL,
	[Icon]	NVARCHAR(200)	 NULL,
	[CompanyName]	NVARCHAR(200)	 NULL,
	[Conversion]    DECIMAL (18,2)   NOT NULL,
	[ImportedAmount]    DECIMAL (18,2)   NOT NULL,
	[MaxImportAmount]    DECIMAL (18,2)   NOT NULL,
	[CreatedOn]    DATETIME   NOT NULL,
	[Remark]	NVARCHAR(400)	 NULL,
	[IsLocked]		INT	NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventSequence] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE [dbo].[ThirdCurrencyImportLogs] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [ThirdCurrencyId]            UNIQUEIDENTIFIER NOT NULL,
    [WalletId]            UNIQUEIDENTIFIER NOT NULL,
	[Mobile]	NVARCHAR(200)	NOT NULL,
	[Account]	NVARCHAR(200)	NOT NULL,
	[Amount]    DECIMAL (18,2)   NOT NULL,
	[ShopCashAmount]    DECIMAL (18,2)   NOT NULL,
	[Conversion]    DECIMAL (18,2)   NOT NULL,
	[CreatedOn]    DATETIME   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC,[ThirdCurrencyId] ASC,[WalletId] ASC)
)
GO

