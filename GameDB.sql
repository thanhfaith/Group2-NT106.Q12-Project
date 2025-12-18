----------------------------------------------------------
-- TẠO DATABASE (nếu chưa có)
----------------------------------------------------------
IF DB_ID('GameDB') IS NULL
BEGIN
    CREATE DATABASE GameDB;
END
GO

USE GameDB;
GO


----------------------------------------------------------
-- 1. BẢNG USERS (ĐĂNG KÝ – ĐĂNG NHẬP)
----------------------------------------------------------
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users
    (
        UserId      INT IDENTITY(1,1) PRIMARY KEY,
        Username    NVARCHAR(50) NOT NULL UNIQUE,
        Password    NVARCHAR(64) NOT NULL,     -- hash SHA256
        Email       NVARCHAR(100) NULL UNIQUE,
        CreatedAt   DATETIME2 DEFAULT SYSUTCDATETIME()
    );
END
GO


----------------------------------------------------------
-- 2. BẢNG ROOMS (TẠO PHÒNG / JOIN PHÒNG)
----------------------------------------------------------
IF OBJECT_ID('dbo.Rooms', 'U') IS NULL
BEGIN
    CREATE TABLE Rooms
    (
        RoomId      INT IDENTITY(1,1) PRIMARY KEY,
        RoomCode    NVARCHAR(10) NOT NULL UNIQUE,   -- mã phòng
        HostUserId  INT NOT NULL,                   -- chủ phòng
        Status      TINYINT NOT NULL DEFAULT 0,     -- 0=Waiting, 1=Playing, 2=Finished
        CreatedAt   DATETIME2 DEFAULT SYSUTCDATETIME()
    );

    ALTER TABLE Rooms
    ADD CONSTRAINT FK_Rooms_Users
        FOREIGN KEY (HostUserId) REFERENCES Users(UserId);
END
GO


----------------------------------------------------------
-- 3. BẢNG ROOMPLAYERS (DANH SÁCH NGƯỜI TRONG PHÒNG)
----------------------------------------------------------
IF OBJECT_ID('dbo.RoomPlayers', 'U') IS NULL
BEGIN
    CREATE TABLE RoomPlayers
    (
        RoomId      INT NOT NULL,
        UserId      INT NOT NULL,
        SeatIndex   TINYINT NULL,                   -- vị trí/màu (0–3)
        IsReady     BIT DEFAULT 0,
        JoinedAt    DATETIME2 DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_RoomPlayers PRIMARY KEY(RoomId, UserId)
    );

    ALTER TABLE RoomPlayers
    ADD CONSTRAINT FK_RoomPlayers_Rooms
        FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId);

    ALTER TABLE RoomPlayers
    ADD CONSTRAINT FK_RoomPlayers_Users
        FOREIGN KEY (UserId) REFERENCES Users(UserId);
END
GO


----------------------------------------------------------
-- 4. BẢNG MATCHES (KẾT QUẢ VÁN CỜ – DÙNG CHO BẢNG XẾP HẠNG)
----------------------------------------------------------
IF OBJECT_ID('dbo.Matches', 'U') IS NULL
BEGIN
    CREATE TABLE Matches
    (
        MatchId       INT IDENTITY(1,1) PRIMARY KEY,
        RoomId        INT NOT NULL,
        WinnerUserId  INT NULL,
        PlayedAt      DATETIME2 DEFAULT SYSUTCDATETIME()
    );

    ALTER TABLE Matches
    ADD CONSTRAINT FK_Matches_Rooms
        FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId);

    ALTER TABLE Matches
    ADD CONSTRAINT FK_Matches_Users
        FOREIGN KEY (WinnerUserId) REFERENCES Users(UserId);
END
GO


----------------------------------------------------------
-- VIEW BẢNG XẾP HẠNG (KHÔNG DÙNG ORDER BY)
----------------------------------------------------------
IF OBJECT_ID('dbo.Leaderboard', 'V') IS NOT NULL
BEGIN
    DROP VIEW Leaderboard;
END
GO

CREATE VIEW Leaderboard AS
SELECT 
    u.UserId,
    u.Username,
    COUNT(m.MatchId) AS Wins
FROM Users u
LEFT JOIN Matches m ON u.UserId = m.WinnerUserId
GROUP BY u.UserId, u.Username;
GO
-------------------------------------------------------------------
---LƯU MÃ RESET VÀ THỜI GIAN HẾT HẠN 
-------------------------------------------------------------------
ALTER TABLE Users 
ADD ResetToken NVARCHAR(10) NULL,
    TokenExpiry DATETIME2 NULL;   
GO