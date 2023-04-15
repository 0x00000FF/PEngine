CREATE TABLE IF NOT EXISTS "Users" (
    "Id"        uuid,
    "Group"     uuid,
    "Username"  varchar(32),
    "DisplayName" varchar(32),
    "Email"     varchar(128) NOT NULL,
    "PasswordHash" varchar(256) NOT NULL,
    "PasswordSalt" varchar(256) NOT NULL,
    "ProfileImage" uuid, -- from "Files"
    "Signature" uuid,    -- from "Posts"

    PRIMARY KEY ("Id"),

    CONSTRAINT "Username_Unique" UNIQUE ("Username"),
    CONSTRAINT "Email_Unique" UNIQUE ("Email")
);

CREATE TABLE IF NOT EXISTS "Credentials" (
    "Id"        uuid,
    "Service"   varchar(32),
    "CredentialHash" varchar(256),
    "CredentialSalt" varchar(256),

    CONSTRAINT "Credentials_Service_Unique" UNIQUE ("Id", "Service")
);

CREATE TABLE IF NOT EXISTS "Entries" (
    "Id"        uuid,
    "Parent"    uuid,

    "Name"      varchar(300) not null,

    "Mode"      integer not null default 0644, -- Directory / Post / Comment / File, Permissions
    "Owner"     uuid,
    "Group"     uuid,

    "Created"   timestamp not null default now(),
    "Modified"  timestamp,
    "Deleted"   timestamp,

    "Hidden"    boolean not null default false, -- Whether hidden
    "Sealed"    boolean not null default false, -- Whether can has children

    "Hits"      integer not null default 0,
    "IPAddress" integer,

    PRIMARY KEY ("Id"),
    CONSTRAINT "Entries_Unique_Name" UNIQUE ("Parent", "Name")
);

CREATE INDEX IF NOT EXISTS "Entries_Parent_Index"
    ON "Entries" USING BTREE ("Parent");

CREATE TABLE IF NOT EXISTS "Keyrings" (
    "Id"        uuid,
    "Salt"      varchar(256), -- Used for Password to Key Derivation
    "Algorithm" varchar(32), -- Encryption Symmetric Algorithm
    "Key"       varchar(256),
    "Vector"    varchar(256), -- When IV required

    PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Posts" (
    "Entry"     uuid NOT NULL,
    "Author"    uuid,
    "Content"   TEXT,

    "Encrypted" BOOLEAN default false,
    "Keyring"   uuid,

    CONSTRAINT "Posts_Entry_Unique" UNIQUE ("Entry")
);

CREATE TABLE IF NOT EXISTS "Comments" (
    "Entry"     uuid NOT NULL,
    "Author"    uuid,

    "Name"      varchar(32), -- when anonymous comment
    "Email"     varchar(128), -- when anonymous comment, should be encrypted

    "Content"   varchar(500),

    "Encrypted" BOOLEAN default false,
    "Keyring"   uuid,

    CONSTRAINT "Comments_Entry_Unique" UNIQUE ("Entry")
);

CREATE TABLE IF NOT EXISTS "Files" (
    "Entry"     uuid NOT NULL,

    "HashName"  varchar(32),
    "Digest"    varchar(32),

    "Filetype"  varchar(64),

    "Service"   varchar(32), -- filesystem when null or ipfs, s3, or ...
    "Path"      varchar(256),

    CONSTRAINT "Files_Entry_Unique" UNIQUE ("Entry")
);

-- CONFIGURE FOREIGN KEYS -------------------------

ALTER TABLE "Users" ADD FOREIGN KEY ("ProfileImage")
            REFERENCES "Files"("Entry") ON DELETE SET NULL;

ALTER TABLE "Users" ADD FOREIGN KEY ("Signature")
            REFERENCES "Posts"("Entry") ON DELETE SET NULL;

ALTER TABLE "Credentials" ADD FOREIGN KEY ("Id")
            REFERENCES "Users"("Id") ON DELETE CASCADE;

ALTER TABLE "Entries" ADD FOREIGN KEY ("Owner")
        REFERENCES "Users"("Id") ON DELETE SET NULL;

ALTER TABLE "Entries" ADD FOREIGN KEY ("Parent")
        REFERENCES "Entries"("Id") ON DELETE CASCADE;

ALTER TABLE "Posts" ADD FOREIGN KEY ("Entry")
                   REFERENCES "Entries"("Id") ON DELETE CASCADE;

ALTER TABLE "Posts" ADD FOREIGN KEY ("Author")
                   REFERENCES "Users"("Id") ON DELETE RESTRICT;

ALTER TABLE "Posts" ADD FOREIGN KEY ("Keyring")
                   REFERENCES "Keyrings"("Id") ON DELETE RESTRICT;

ALTER TABLE "Comments" ADD FOREIGN KEY ("Entry")
                   REFERENCES "Entries"("Id") ON DELETE CASCADE;

ALTER TABLE "Comments" ADD FOREIGN KEY ("Author")
                   REFERENCES "Users"("Id") ON DELETE RESTRICT;

ALTER TABLE "Comments" ADD FOREIGN KEY ("Keyring")
                   REFERENCES "Keyrings"("Id") ON DELETE RESTRICT;

ALTER TABLE "Files" ADD FOREIGN KEY ("Entry")
                   REFERENCES "Entries"("Id") ON DELETE CASCADE;
