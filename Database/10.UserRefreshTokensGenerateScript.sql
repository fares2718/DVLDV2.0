CREATE TABLE dbo.UserRefreshTokens
(
    RefreshTokenID UNIQUEIDENTIFIER NOT NULL
        DEFAULT NEWID(),

    UserID UNIQUEIDENTIFIER NOT NULL,

    TokenHash NVARCHAR(500) NOT NULL,

    ExpiresAt DATETIME2(3) NOT NULL,

    CreatedAt DATETIME2(3) NOT NULL
        DEFAULT SYSUTCDATETIME(),

    RevokedAt DATETIME2(3) NULL,

    ReplacedByTokenID UNIQUEIDENTIFIER NULL,

    CreatedByIp NVARCHAR(45) NULL,

    RevokedByIp NVARCHAR(45) NULL,

    UserAgent NVARCHAR(500) NULL,

    CONSTRAINT PK_UserRefreshTokens
        PRIMARY KEY (RefreshTokenID),

    CONSTRAINT FK_UserRefreshTokens_User
        FOREIGN KEY (UserID)
        REFERENCES dbo.Users(UserID),

    CONSTRAINT FK_UserRefreshTokens_ReplacedBy
        FOREIGN KEY (ReplacedByTokenID)
        REFERENCES dbo.UserRefreshTokens(RefreshTokenID)
);
GO

CREATE INDEX IX_UserRefreshTokens_UserID
ON dbo.UserRefreshTokens(UserID);
GO
