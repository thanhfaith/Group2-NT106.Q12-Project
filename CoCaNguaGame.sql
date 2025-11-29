USE CoCaNguaGame;
GO

-- 1. Bảng Users: lưu tài khoản + thống kê + email
CREATE TABLE Users (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Username     NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    DisplayName  NVARCHAR(50)  NULL,

    Email        NVARCHAR(100) NOT NULL,   -- THÊM DÒNG NÀY

    TotalGames   INT NOT NULL DEFAULT 0,
    Wins         INT NOT NULL DEFAULT 0,
    Losses       INT NOT NULL DEFAULT 0,
    Score        INT NOT NULL DEFAULT 0,

    CreatedAt    DATETIME NOT NULL DEFAULT GETDATE(),
    LastLoginAt  DATETIME NULL
);
GO

-- 2. Bảng Rooms: thông tin phòng chơi
CREATE TABLE Rooms (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    RoomCode   NVARCHAR(10)  NOT NULL UNIQUE,
    Name       NVARCHAR(50)  NULL,
    HostUserId INT           NOT NULL,
    MaxPlayers INT           NOT NULL DEFAULT 4,
    Status     TINYINT       NOT NULL DEFAULT 0,
    CreatedAt  DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

ALTER TABLE Rooms
ADD CONSTRAINT FK_Rooms_Users_HostUser
FOREIGN KEY (HostUserId) REFERENCES Users(Id);
GO

-- 3. Bảng RoomPlayers: ai đang trong phòng nào
CREATE TABLE RoomPlayers (
    RoomId    INT NOT NULL,
    UserId    INT NOT NULL,
    SeatIndex INT NOT NULL,
    IsReady   BIT NOT NULL DEFAULT 0,
    IsHost    BIT NOT NULL DEFAULT 0,
    JoinedAt  DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_RoomPlayers PRIMARY KEY (RoomId, UserId)
);
GO

ALTER TABLE RoomPlayers
ADD CONSTRAINT FK_RoomPlayers_Rooms
FOREIGN KEY (RoomId) REFERENCES Rooms(Id);
GO

ALTER TABLE RoomPlayers
ADD CONSTRAINT FK_RoomPlayers_Users
FOREIGN KEY (UserId) REFERENCES Users(Id);
GO

-- 4. Bảng Matches: lưu lịch sử trận đấu
CREATE TABLE Matches (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    RoomId       INT        NOT NULL,
    WinnerUserId INT        NULL,
    StartedAt    DATETIME   NOT NULL DEFAULT GETDATE(),
    EndedAt      DATETIME   NULL,
    Status       TINYINT    NOT NULL DEFAULT 0
);
GO

ALTER TABLE Matches
ADD CONSTRAINT FK_Matches_Rooms
FOREIGN KEY (RoomId) REFERENCES Rooms(Id);
GO

ALTER TABLE Matches
ADD CONSTRAINT FK_Matches_Users_Winner
FOREIGN KEY (WinnerUserId) REFERENCES Users(Id);
GO
