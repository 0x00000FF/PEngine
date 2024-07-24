CREATE TABLE AuthFactors (
    Id           UUID         PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL UNIQUE,
    Description  TEXT,
    Enabled      BOOLEAN,
    Endpoint     TEXT NOT NULL,
    Key          TEXT,
    Secret       TEXT,
    ExtraConfig  TEXT,
    AddedAt      TIMESTAMP DEFAULT NOW()
);

CREATE TABLE Roles (
    Id           UUID         PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL UNIQUE,
    Description  TEXT
);

CREATE TABLE KeyChains (
    Id           UUID         PRIMARY KEY,
    Name         VARCHAR(64)  NOT NULL,
    Keys         TEXT         NOT NULL
);

CREATE TABLE Members (
    Id           UUID         PRIMARY KEY,

    Username     VARCHAR(128) NOT NULL UNIQUE,
    Password     VARCHAR(64)  NOT NULL,
    PasswordSalt VARCHAR(64)  NOT NULL,

    Name         VARCHAR(32)  NOT NULL,
    Email        VARCHAR(64)  NOT NULL,

    Disabled     BOOLEAN      NOT NULL,

    CreatedAt    TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE MemberAuditLogs (
    Id          UUID         PRIMARY KEY,
    Member      UUID         REFERENCES Members(Id) ON DELETE SET NULL,
    Action      VARCHAR(32)  NOT NULL,
    Description VARCHAR(256) NOT NULL,
    Timestamp   TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE MemberAuthFactors (
    Id          UUID         PRIMARY KEY,
    Member      UUID         REFERENCES Members(Id) ON DELETE CASCADE,  
    Factor      UUID         NOT NULL REFERENCES AuthFactors(Id) ON DELETE CASCADE,
    EncToken    TEXT,
    AddedAt     TIMESTAMP    DEFAULT NOW(),
    ModifiedAt  TIMESTAMP    DEFAULT NOW(),
    ExpiredAt   TIMESTAMP,
    IPAddress   VARCHAR(32)  NOT NULL
);

CREATE TABLE MemberRoles (
    Id          UUID         PRIMARY KEY,
    Role        UUID         NOT NULL REFERENCES Roles(Id) ON DELETE CASCADE,
    AddedAt     TIMESTAMP    DEFAULT NOW(),
    Ord         INT          NOT NULL
);

CREATE TABLE FileStorages (
    Id          UUID         PRIMARY KEY,
    Name        VARCHAR(32)  NOT NULL UNIQUE,
    Path        TEXT         NOT NULL,
    Provider    VARCHAR(32)  CHECK (Provider IN ('Local', 'Remote', 'ObjectStorage'))
);

CREATE TABLE Files (
    Id          UUID         PRIMARY KEY,
    Uploader    UUID         NOT NULL REFERENCES Members(Id) ON DELETE RESTRICT,

    Name        VARCHAR(256),
    Size        BIGINT,
    Type        VARCHAR(256),

    Storage     UUID         NOT NULL REFERENCES FileStorages(Id) ON DELETE CASCADE,

    AddedAt     TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE FileHits (
    File        UUID         NOT NULL REFERENCES Files(Id) ON DELETE CASCADE,

    LoggedMember  UUID         REFERENCES Members(Id) ON DELETE SET NULL,
    IPAddress   VARCHAR(32)  NOT NULL,
    UserAgent   VARCHAR(256) NOT NULL,

    Timestamp   TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE Categories (
    Name    VARCHAR(32) PRIMARY KEY,
    Count   INT DEFAULT 0,
    Ord     INT NOT NULL
);

CREATE TABLE DocumentTypes (
    Id          UUID         PRIMARY KEY,
    Name        VARCHAR(32)  NOT NULL,
    Description VARCHAR(256)
);

CREATE TABLE Documents (
    Id          uuid         PRIMARY KEY,
    Type        uuid         NOT NULL REFERENCES DocumentTypes(Id) ON DELETE RESTRICT,

    Author      uuid         REFERENCES Members(Id) ON DELETE SET NULL,
    Owner       uuid         REFERENCES Members(Id) ON DELETE SET NULL,
    Role        uuid         REFERENCES Roles(Id) ON DELETE SET NULL,
    Permission  INT          NOT NULL CHECK ( Permission >= 0x000 AND Permission <= 0xFFF ),

    IsProtected BOOLEAN      NOT NULL,
    KeyChain    UUID         REFERENCES KeyChains(Id) ON DELETE SET NULL,

    Title       VARCHAR(256) NOT NULL,
    Content     TEXT,

    AddedAt     TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE ProfileVersions (
    Id          uuid         PRIMARY KEY,
    Document    UUID         NOT NULL REFERENCES Documents(Id) ON DELETE CASCADE
);

CREATE TABLE Profiles (
    Id          uuid         PRIMARY KEY,
    Member        UUID         NOT NULL UNIQUE REFERENCES Members(Id) ON DELETE CASCADE,
    Version     UUID         NOT NULL UNIQUE REFERENCES ProfileVersions(Id) ON DELETE RESTRICT,
    AddedAt     TIMESTAMP    DEFAULT NOW(),
    ModifiedAt  TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE PostVersions (
    Id          uuid         PRIMARY KEY,
    Document    UUID         NOT NULL REFERENCES Documents(Id) ON DELETE CASCADE
);

CREATE TABLE Posts (
    Id          SERIAL      PRIMARY KEY,
    Category    UUID        REFERENCES Categories(Id) ON DELETE SET NULL,
    Version     UUID        NOT NULL REFERENCES PostVersions(Id) ON DELETE RESTRICT,

    Visibility  VARCHAR(32)  NOT NULL CHECK ( Visibility IN ('public', 'permallink', 'private') ),
    Permalink   VARCHAR(256),
    Password    VARCHAR(64),

    AddedAt     TIMESTAMP    DEFAULT NOW(),
    ModifiedAt  TIMESTAMP,
    DeletedAt   TIMESTAMP
);

CREATE TABLE PostAttachments (
    Post        INT          NOT NULL REFERENCES Posts(Id) ON DELETE CASCADE,
    File        UUID         NOT NULL REFERENCES Files(Id) ON DELETE CASCADE
);

CREATE TABLE PostHits (
    Post        INT         NOT NULL REFERENCES Posts(Id) ON DELETE CASCADE,

    LoggedMember  UUID         REFERENCES Members(Id) ON DELETE SET NULL,
    IPAddress   VARCHAR(32)  NOT NULL,
    UserAgent   VARCHAR(256) NOT NULL,

    Timestamp   TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE PostLikes (
    Post        INT         NOT NULL REFERENCES Posts(Id) ON DELETE CASCADE,

    LoggedMember  UUID         REFERENCES Members(Id) ON DELETE SET NULL,
    IPAddress   VARCHAR(32)  NOT NULL,
    UserAgent   VARCHAR(256) NOT NULL,

    Timestamp   TIMESTAMP    DEFAULT NOW()
);

CREATE TABLE PostTags (
    Post        INT         NOT NULL REFERENCES Posts(Id) ON DELETE CASCADE,
    Tag         VARCHAR(32)  NOT NULL,

    UNIQUE (Post, Tag)
);

CREATE TABLE GuestbookVersions (
    Id          uuid         PRIMARY KEY,
    Document    UUID         NOT NULL REFERENCES Documents(Id) ON DELETE CASCADE
);

CREATE TABLE Guestbook (
    Id          SERIAL       PRIMARY KEY,
    Version     UUID         NOT NULL REFERENCES GuestbookVersions(Id) ON DELETE RESTRICT,

    Writer      VARCHAR(32),
    Password    VARCHAR(64),
    Contact     VARCHAR(64),

    AddedAt     TIMESTAMP    DEFAULT NOW(),
    ModifiedAt  TIMESTAMP,

    IsDeleted   BOOLEAN NOT NULL DEFAULT FALSE,

    IsBlocked   BOOLEAN NOT NULL DEFAULT FALSE,
    BlockReason VARCHAR(384),
    BlockedAt   TIMESTAMP
);

CREATE TABLE GuestbookCommentVersions (
    Id          uuid         PRIMARY KEY,
    Document    UUID         NOT NULL REFERENCES Documents(Id) ON DELETE CASCADE
);

CREATE TABLE GuestbookComments (
    Id        SERIAL PRIMARY KEY,
    Guestbook INT    NOT NULL REFERENCES Guestbook (Id) ON DELETE CASCADE,
    Version   UUID   NOT NULL REFERENCES GuestbookCommentVersions(Id) ON DELETE RESTRICT,

    Writer      VARCHAR(32),
    Password    VARCHAR(64),
    Contact     VARCHAR(64),

    AddedAt     TIMESTAMP    DEFAULT NOW(),
    ModifiedAt  TIMESTAMP,

    IsDeleted   BOOLEAN NOT NULL DEFAULT FALSE,

    IsBlocked   BOOLEAN NOT NULL DEFAULT FALSE,
    BlockReason VARCHAR(384),
    BlockedAt   TIMESTAMP
);

CREATE TABLE CommentVersions (
    Id          uuid         PRIMARY KEY,
    Document    UUID         NOT NULL REFERENCES Documents(Id) ON DELETE CASCADE
);

CREATE TABLE Comments (
    Id          SERIAL       PRIMARY KEY,
    Post        INT    NOT NULL REFERENCES Posts(Id) ON DELETE CASCADE,
    Version     UUID   NOT NULL REFERENCES CommentVersions(Id) ON DELETE RESTRICT,

    Writer      VARCHAR(32),
    Password    VARCHAR(64),
    Contact     VARCHAR(64),

    AddedAt     TIMESTAMP    DEFAULT NOW(),
    ModifiedAt  TIMESTAMP,

    IsDeleted   BOOLEAN NOT NULL DEFAULT FALSE,

    IsBlocked   BOOLEAN NOT NULL DEFAULT FALSE,
    BlockReason VARCHAR(384),
    BlockedAt   TIMESTAMP
);