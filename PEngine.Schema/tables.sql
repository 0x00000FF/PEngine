CREATE TABLE "Users" (
    "Id"        uuid,
    "Group"     uuid default "Id",
    "Username"  varchar(32),
    "DisplayName" varchar(32),
    "Email"     varchar(128),
    "PasswordHash" varchar(256),
    "PasswordSalt" varchar(256),
    "ProfileImage" uuid,
    "Signature" uuid,

    PRIMARY KEY ("Id")
);

CREATE TABLE "Credentials" (
    "Id"        uuid,
    "Service"   varchar(32),
    "CredentialHash" varchar(256),
    "CredentialSalt" varchar(256),

    CONSTRAINT "Credentials_Service_Unique" UNIQUE ("Id", "Service"),
    CONSTRAINT "Credentials_Id_Users" FOREIGN KEY ("Id")
            REFERENCES "Users"("Id") ON DELETE CASCADE
);

CREATE TABLE Entries (
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
    CONSTRAINT "Entries_Unique_Name" UNIQUE ("Parent", "Name"),

    CONSTRAINT "Entries_Owner_User" FOREIGN KEY ("Owner")
        REFERENCES "Users"("Id") ON DELETE SET NULL,
    CONSTRAINT "Entries_Parent" FOREIGN KEY ("Parent")
        REFERENCES "Entries"("Id") ON DELETE CASCADE
);

CREATE INDEX "Entries_Parent_Index" ON "Entries" USING BTREE ("Parent");

CREATE TABLE Keyrings (
    "Id"        uuid,
    "Salt"      varchar(256), -- Used for Password to Key Derivation
    "Algorithm" varchar(32), -- Encryption Symmetric Algorithm
    "Key"       varchar(256),
    "Vector"    varchar(256), -- When IV required

    PRIMARY KEY ("Id")
);

CREATE TABLE Posts (
    "Entry"     uuid NOT NULL,
    "Author"    uuid,
    "Content"   TEXT,

    "Encrypted" BOOLEAN default false,
    "Keyring"   uuid,

    CONSTRAINT "Posts_Entry_Id" FOREIGN KEY ("Entry")
                   REFERENCES "Entries"("Id") ON DELETE CASCADE,
    CONSTRAINT "Posts_Author_Id" FOREIGN KEY ("Author")
                   REFERENCES "Users"("Id") ON DELETE RESTRICT,
    CONSTRAINT "Posts_Keyring" FOREIGN KEY ("Keyring")
                   REFERENCES "Keyrings"("Id") ON DELETE RESTRICT
);

CREATE TABLE Comments (
    "Entry"     uuid NOT NULL,
    "Author"    uuid,

    "Name"      varchar(32), -- when anonymous comment
    "Email"     varchar(128), -- when anonymous comment, should be encrypted

    "Content"   varchar(500),

    CONSTRAINT "Comments_Entry_Id" FOREIGN KEY ("Entry")
                   REFERENCES "Entries"("Id") ON DELETE CASCADE,
    CONSTRAINT "Comments_Author_Id" FOREIGN KEY ("Author")
                   REFERENCES "Users"("Id") ON DELETE RESTRICT,
    CONSTRAINT "Posts_Keyring" FOREIGN KEY ("Keyring")
                   REFERENCES "Keyrings"("Id") ON DELETE RESTRICT
);

CREATE TABLE Files (
    "Entry"     uuid NOT NULL,

    "HashName"  varchar(32),
    "Digest"    varchar(32),

    "Filetype"  varchar(64),

    "Service"   varchar(32), -- filesystem or ipfs, s3, or ...
    "Path"      varchar(256),

    CONSTRAINT "Files_Entry_Id" FOREIGN KEY ("Entry")
                   REFERENCES "Entries"("Id") ON DELETE CASCADE,
);