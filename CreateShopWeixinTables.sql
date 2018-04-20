CREATE TABLE [dbo].[UserOAuthAccessTokens] (
    [OpenId]    NVARCHAR (200)     NOT NULL,
    [StartTime]       DATETIME         NOT NULL,
	[AccessToken]			NVARCHAR(400) NOT NULL,
    PRIMARY KEY CLUSTERED ([OpenId] ASC)
)
GO
CREATE TABLE [dbo].[Users] (
    [OpenId]    NVARCHAR (200)     NOT NULL,
    [UnionId]    NVARCHAR (200)      NULL,
    [NickName]       NVARCHAR (200)           NULL,
	[Province]			NVARCHAR(200)  NULL,
	[City]			NVARCHAR(200)  NULL,
	[County]			NVARCHAR(200)  NULL,
	[Gender]			NVARCHAR(200)  NULL,
	[Portrait]			NVARCHAR(200)  NULL,
	[ParentOpenId]			NVARCHAR(200)  NULL,
    PRIMARY KEY CLUSTERED ([OpenId] ASC)
)
GO


