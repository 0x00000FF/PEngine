CREATE TABLE IF NOT EXISTS "Users" (
    "Id"        uuid,
    "Group"     uuid,
    "Username"  varchar(32) NOT NULL,
    "DisplayName" varchar(32) NOT NULL,
    "Email"     varchar(128) NOT NULL,
    "ProfileImage" uuid, -- from "Files"
    "Signature" uuid,    -- from "Posts"
    
    "Keyring"   uuid NOT NULL,

    PRIMARY KEY ("Id"),

    CONSTRAINT "Username_Unique" UNIQUE ("Username"),
    CONSTRAINT "Email_Unique" UNIQUE ("Email")
);

CREATE TABLE IF NOT EXISTS "Credentials" (
    "Id"        uuid,
    "Service"   varchar(32) NOT NULL,
    "CredentialHash" varchar(256) NOT NULL,
    "CredentialSalt" varchar(256),

    CONSTRAINT "Credentials_Service_Unique" UNIQUE ("Id", "Service")
);

CREATE TABLE IF NOT EXISTS "Entries" (
    "Id"        uuid,
    "Parent"    uuid,

    "Name"      varchar(300) not null,

    "Mode"      integer not null default 0644, -- Directory / Post / Comment / File, Permissions
    "Creator"   uuid,
    "Owner"     uuid,
    "Group"     uuid,

    "Created"   timestamp not null default now(),
    "Modified"  timestamp,
    "Deleted"   timestamp,

    "Hidden"    boolean not null default false, -- Whether hidden
    "Sealed"    boolean not null default false, -- Whether can has children

    "Hits"      integer not null default 0,
    "IpAddress" varchar(32), -- Base64, IPv4/v6
    
    "Entity"    uuid,

    PRIMARY KEY ("Id"),
    CONSTRAINT "Entries_Unique_Name" UNIQUE ("Parent", "Name")
);

CREATE INDEX IF NOT EXISTS "Entries_Parent_Index"
    ON "Entries" USING BTREE ("Parent");

CREATE TABLE IF NOT EXISTS "Keyrings" (
    "Id"        uuid,
    "Salt"      varchar(256), -- Used for Password to Key Derivation
    "Algorithm" varchar(32) NOT NULL, -- Crypto Algorithm
    "Key"       varchar(256) NOT NULL, -- Digest or Key
    "Vector"    varchar(256), -- When IV required

    PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Posts" (
    "Id"        uuid,
    "Content"   TEXT,

    "Encrypted" BOOLEAN default false,
    "Keyring"   uuid,

    PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Tags" (
    "Id"        uuid,
    "Name"      varchar(100) NOT NULL,

    PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Comments" (
    "Id"        uuid,

    "Name"      varchar(32), -- when anonymous comment
    "Email"     varchar(128), -- when anonymous comment, should be encrypted

    "Content"   varchar(500),

    "Encrypted" BOOLEAN default false,
    "Keyring"   uuid,

    PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Files" (
    "Id"        uuid,

    "HashName"  varchar(32),
    "Digest"    varchar(32),

    "Filetype"  varchar(64),

    "Service"   varchar(32), -- filesystem when null or ipfs, s3, or ...
    "Path"      varchar(256),

    PRIMARY KEY ("Id")
);

-- CONFIGURE FOREIGN KEYS -------------------------

ALTER TABLE "Users" ADD FOREIGN KEY ("ProfileImage")
            REFERENCES "Files"("Id") ON DELETE SET NULL;

ALTER TABLE "Users" ADD FOREIGN KEY ("Signature")
            REFERENCES "Posts"("Id") ON DELETE SET NULL;

ALTER TABLE "Users" ADD FOREIGN KEY ("Keyring")
    REFERENCES "Keyrings"("Id") ON DELETE RESTRICT;

ALTER TABLE "Credentials" ADD FOREIGN KEY ("Id")
            REFERENCES "Users"("Id") ON DELETE CASCADE;

ALTER TABLE "Entries" ADD FOREIGN KEY ("Creator")
    REFERENCES "Users"("Id") ON DELETE SET NULL;

ALTER TABLE "Entries" ADD FOREIGN KEY ("Owner")
        REFERENCES "Users"("Id") ON DELETE SET NULL;

ALTER TABLE "Entries" ADD FOREIGN KEY ("Parent")
        REFERENCES "Entries"("Id") ON DELETE CASCADE;

ALTER TABLE "Posts" ADD FOREIGN KEY ("Keyring")
                   REFERENCES "Keyrings"("Id") ON DELETE RESTRICT;

ALTER TABLE "PostTags" ADD FOREIGN KEY ("TagId")
                    REFERENCES "Tags"("Id") ON DELETE CASCADE;

ALTER TABLE "Comments" ADD FOREIGN KEY ("Keyring")
                   REFERENCES "Keyrings"("Id") ON DELETE RESTRICT;