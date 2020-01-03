CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "UserName" character varying(256) NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" serial NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" serial NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" character varying(128) NOT NULL,
    "ProviderKey" character varying(128) NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" character varying(128) NOT NULL,
    "Name" character varying(128) NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181213062107_idenmigration', '2.1.11-servicing-32099');

ALTER TABLE "AspNetUsers" ADD "Name" text NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181213064503_CustomUserData', '2.1.11-servicing-32099');

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "BillSeries" (
    "SeriesName" text NOT NULL,
    CONSTRAINT "PK_BillSeries" PRIMARY KEY ("SeriesName")
);

CREATE TABLE "Company" (
    "CompanyID" serial NOT NULL,
    "CompanyName" character varying(100) NOT NULL,
    "CreationDate" timestamp without time zone NOT NULL,
    "CompanyLogo" text NOT NULL,
    "BillFormat" integer NULL,
    "GSTIN" text NOT NULL,
    "BankName" character varying(100) NOT NULL,
    "AccountType" integer NOT NULL,
    "AccountNumber" text NOT NULL,
    "IFSCcode" text NOT NULL,
    "PAN" text NOT NULL,
    CONSTRAINT "PK_Company" PRIMARY KEY ("CompanyID")
);

CREATE TABLE "Particulars" (
    "ParticularsID" serial NOT NULL,
    "ParticularsName" character varying(150) NOT NULL,
    "SACCode" integer NOT NULL,
    "Amount" double precision NOT NULL,
    CONSTRAINT "PK_Particulars" PRIMARY KEY ("ParticularsID")
);

CREATE TABLE "Branch" (
    "BranchID" serial NOT NULL,
    "BranchName" character varying(100) NOT NULL,
    "BranchAddress" character varying(200) NOT NULL,
    "CreationDate" timestamp without time zone NOT NULL,
    "CompanyID" integer NOT NULL,
    CONSTRAINT "PK_Branch" PRIMARY KEY ("BranchID"),
    CONSTRAINT "FK_Branch_Company_CompanyID" FOREIGN KEY ("CompanyID") REFERENCES "Company" ("CompanyID") ON DELETE CASCADE
);

CREATE TABLE "DebtorGroup" (
    "DebtorGroupID" serial NOT NULL,
    "DebtorGroupName" character varying(150) NOT NULL,
    "DebtorGroupAddress" character varying(350) NOT NULL,
    "DebtorGroupMail" text NOT NULL,
    "DebtorGroupPhoneNumber" text NOT NULL,
    "DebtorGroupCity" text NOT NULL,
    "DebtorGSTIN" text NULL,
    "DebtorOutstanding" double precision NOT NULL,
    "BranchID" integer NOT NULL,
    CONSTRAINT "PK_DebtorGroup" PRIMARY KEY ("DebtorGroupID"),
    CONSTRAINT "FK_DebtorGroup_Branch_BranchID" FOREIGN KEY ("BranchID") REFERENCES "Branch" ("BranchID") ON DELETE CASCADE
);

CREATE TABLE "Bill" (
    "BillNumber" text NOT NULL,
    "BilledTo" text NOT NULL,
    "BillAmount" double precision NOT NULL,
    "Note" character varying(500) NULL,
    "InvoiceDate" timestamp without time zone NOT NULL,
    "PlaceOfSupply" integer NULL,
    "DebtorGroupID" integer NOT NULL,
    "CompanyID" integer NOT NULL,
    "SeriesName" text NULL,
    CONSTRAINT "PK_Bill" PRIMARY KEY ("BillNumber"),
    CONSTRAINT "FK_Bill_Company_CompanyID" FOREIGN KEY ("CompanyID") REFERENCES "Company" ("CompanyID") ON DELETE CASCADE,
    CONSTRAINT "FK_Bill_DebtorGroup_DebtorGroupID" FOREIGN KEY ("DebtorGroupID") REFERENCES "DebtorGroup" ("DebtorGroupID") ON DELETE CASCADE,
    CONSTRAINT "FK_Bill_BillSeries_SeriesName" FOREIGN KEY ("SeriesName") REFERENCES "BillSeries" ("SeriesName") ON DELETE RESTRICT
);

CREATE TABLE "Debtor" (
    "DebtorID" serial NOT NULL,
    "DebtorName" character varying(150) NOT NULL,
    "DebtorOutstanding" double precision NOT NULL,
    "DebtorGroupID" integer NOT NULL,
    CONSTRAINT "PK_Debtor" PRIMARY KEY ("DebtorID"),
    CONSTRAINT "FK_Debtor_DebtorGroup_DebtorGroupID" FOREIGN KEY ("DebtorGroupID") REFERENCES "DebtorGroup" ("DebtorGroupID") ON DELETE CASCADE
);

CREATE TABLE "BillDetails" (
    "BillDetailsID" serial NOT NULL,
    "ParticularsName" text NOT NULL,
    "Period" text NOT NULL,
    "YearInfo" integer NOT NULL,
    "Amount" double precision NOT NULL,
    "CGSTAmount" double precision NOT NULL,
    "SGSTAmount" double precision NOT NULL,
    "TaxableValue" double precision NOT NULL,
    "BillAmountOutstanding" double precision NOT NULL,
    "CompanyID" integer NOT NULL,
    "ParticularsID" integer NOT NULL,
    "BillNumber" text NULL,
    "DebtorID" integer NOT NULL,
    "DebtorGroupID" integer NOT NULL,
    CONSTRAINT "PK_BillDetails" PRIMARY KEY ("BillDetailsID"),
    CONSTRAINT "FK_BillDetails_Bill_BillNumber" FOREIGN KEY ("BillNumber") REFERENCES "Bill" ("BillNumber") ON DELETE RESTRICT,
    CONSTRAINT "FK_BillDetails_Company_CompanyID" FOREIGN KEY ("CompanyID") REFERENCES "Company" ("CompanyID") ON DELETE CASCADE,
    CONSTRAINT "FK_BillDetails_DebtorGroup_DebtorGroupID" FOREIGN KEY ("DebtorGroupID") REFERENCES "DebtorGroup" ("DebtorGroupID") ON DELETE CASCADE,
    CONSTRAINT "FK_BillDetails_Debtor_DebtorID" FOREIGN KEY ("DebtorID") REFERENCES "Debtor" ("DebtorID") ON DELETE CASCADE,
    CONSTRAINT "FK_BillDetails_Particulars_ParticularsID" FOREIGN KEY ("ParticularsID") REFERENCES "Particulars" ("ParticularsID") ON DELETE CASCADE
);

CREATE TABLE "Received" (
    "ReceivedID" serial NOT NULL,
    "ReceivedAmount" double precision NOT NULL,
    "ReceivedDate" timestamp without time zone NOT NULL,
    "ChequePaymet" boolean NOT NULL,
    "ChequeNumber" text NULL,
    "CompanyID" integer NOT NULL,
    "BillDetailsID" integer NOT NULL,
    "DebtorGroupID" integer NULL,
    CONSTRAINT "PK_Received" PRIMARY KEY ("ReceivedID"),
    CONSTRAINT "FK_Received_BillDetails_BillDetailsID" FOREIGN KEY ("BillDetailsID") REFERENCES "BillDetails" ("BillDetailsID") ON DELETE CASCADE,
    CONSTRAINT "FK_Received_Company_CompanyID" FOREIGN KEY ("CompanyID") REFERENCES "Company" ("CompanyID") ON DELETE CASCADE,
    CONSTRAINT "FK_Received_DebtorGroup_DebtorGroupID" FOREIGN KEY ("DebtorGroupID") REFERENCES "DebtorGroup" ("DebtorGroupID") ON DELETE RESTRICT
);

CREATE INDEX "IX_Bill_CompanyID" ON "Bill" ("CompanyID");

CREATE INDEX "IX_Bill_DebtorGroupID" ON "Bill" ("DebtorGroupID");

CREATE INDEX "IX_Bill_SeriesName" ON "Bill" ("SeriesName");

CREATE INDEX "IX_BillDetails_BillNumber" ON "BillDetails" ("BillNumber");

CREATE INDEX "IX_BillDetails_CompanyID" ON "BillDetails" ("CompanyID");

CREATE INDEX "IX_BillDetails_DebtorGroupID" ON "BillDetails" ("DebtorGroupID");

CREATE INDEX "IX_BillDetails_DebtorID" ON "BillDetails" ("DebtorID");

CREATE INDEX "IX_BillDetails_ParticularsID" ON "BillDetails" ("ParticularsID");

CREATE INDEX "IX_Branch_CompanyID" ON "Branch" ("CompanyID");

CREATE INDEX "IX_Debtor_DebtorGroupID" ON "Debtor" ("DebtorGroupID");

CREATE INDEX "IX_DebtorGroup_BranchID" ON "DebtorGroup" ("BranchID");

CREATE INDEX "IX_Received_BillDetailsID" ON "Received" ("BillDetailsID");

CREATE INDEX "IX_Received_CompanyID" ON "Received" ("CompanyID");

CREATE INDEX "IX_Received_DebtorGroupID" ON "Received" ("DebtorGroupID");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181218060410_ModelMigrate', '2.1.11-servicing-32099');

ALTER TABLE "Company" DROP COLUMN "CompanyLogo";

ALTER TABLE "Company" ADD "CompanyLogoImg" bytea NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181218075000_CompanyImageadd', '2.1.11-servicing-32099');

ALTER TABLE "Branch" ADD "BranchEmail" character varying(200) NOT NULL DEFAULT '';

ALTER TABLE "Branch" ADD "BranchPhone" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190108061125_mailandphoneadd', '2.1.11-servicing-32099');

ALTER TABLE "Company" ADD "CompanyOwner" character varying(100) NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190108075357_ownername', '2.1.11-servicing-32099');

ALTER TABLE "Bill" ADD "BillDate" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190108181050_billdate', '2.1.11-servicing-32099');

ALTER TABLE "BillDetails" ALTER COLUMN "YearInfo" TYPE integer;
ALTER TABLE "BillDetails" ALTER COLUMN "YearInfo" DROP NOT NULL;
ALTER TABLE "BillDetails" ALTER COLUMN "YearInfo" DROP DEFAULT;

ALTER TABLE "BillDetails" ALTER COLUMN "Period" TYPE text;
ALTER TABLE "BillDetails" ALTER COLUMN "Period" DROP NOT NULL;
ALTER TABLE "BillDetails" ALTER COLUMN "Period" DROP DEFAULT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190110054917_rmrequired', '2.1.11-servicing-32099');

ALTER TABLE "BillDetails" DROP CONSTRAINT "FK_BillDetails_Debtor_DebtorID";

ALTER TABLE "BillDetails" ALTER COLUMN "DebtorID" TYPE integer;
ALTER TABLE "BillDetails" ALTER COLUMN "DebtorID" DROP NOT NULL;
ALTER TABLE "BillDetails" ALTER COLUMN "DebtorID" DROP DEFAULT;

ALTER TABLE "BillDetails" ADD CONSTRAINT "FK_BillDetails_Debtor_DebtorID" FOREIGN KEY ("DebtorID") REFERENCES "Debtor" ("DebtorID") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190110060142_rmfk', '2.1.11-servicing-32099');

ALTER TABLE "Bill" ADD "BillActNum" integer NOT NULL DEFAULT 0;

CREATE UNIQUE INDEX "IX_Bill_BillActNum" ON "Bill" ("BillActNum");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190110121116_billnumunique', '2.1.11-servicing-32099');

ALTER TABLE "Bill" ALTER COLUMN "BillActNum" TYPE integer;
ALTER TABLE "Bill" ALTER COLUMN "BillActNum" DROP NOT NULL;
ALTER TABLE "Bill" ALTER COLUMN "BillActNum" DROP DEFAULT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190110140151_rmnotnull', '2.1.11-servicing-32099');

ALTER TABLE "BillDetails" DROP CONSTRAINT "FK_BillDetails_Bill_BillNumber";

ALTER TABLE "BillDetails" ADD CONSTRAINT "FK_BillDetails_Bill_BillNumber" FOREIGN KEY ("BillNumber") REFERENCES "Bill" ("BillNumber") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190111175420_cascadedel', '2.1.11-servicing-32099');

ALTER TABLE "Bill" ADD "BillDelivered" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Bill" ADD "BranchID" integer NOT NULL DEFAULT 0;

ALTER TABLE "Bill" ADD "SecretUnlockCode" integer NOT NULL DEFAULT 0;

CREATE INDEX "IX_Bill_BranchID" ON "Bill" ("BranchID");

ALTER TABLE "Bill" ADD CONSTRAINT "FK_Bill_Branch_BranchID" FOREIGN KEY ("BranchID") REFERENCES "Branch" ("BranchID") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190206191351_Billchanges', '2.1.11-servicing-32099');

ALTER TABLE "Bill" ADD "MessageSent" boolean NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190206194820_Bill1change', '2.1.11-servicing-32099');

ALTER TABLE "Company" ALTER COLUMN "CompanyOwner" TYPE text;
ALTER TABLE "Company" ALTER COLUMN "CompanyOwner" DROP NOT NULL;
ALTER TABLE "Company" ALTER COLUMN "CompanyOwner" DROP DEFAULT;

ALTER TABLE "Branch" ADD "BranchManagerName" character varying(100) NOT NULL DEFAULT '';

ALTER TABLE "Branch" ADD "BranchManagerSign" bytea NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190207130417_BranchChange', '2.1.11-servicing-32099');

INSERT INTO "Company"("CompanyName","CreationDate","BillFormat","GSTIN","BankName","AccountType","AccountNumber","IFSCcode","PAN","CompanyLogoImg","CompanyOwner") 
VALUES ('Test Company',CURRENT_DATE,1,'24CAGPP9521D1ZS','Test Bank',1,'50444444444321','HDFC0003789','AAPPP5612O','wdfwqdasdasd','Mr. Test');

INSERT INTO "Company"("CompanyName","CreationDate","BillFormat","GSTIN","BankName","AccountType","AccountNumber","IFSCcode","PAN","CompanyLogoImg","CompanyOwner") 
VALUES ('Test Company1',CURRENT_DATE,0,'24CAGPP9521D1ZS','Test Bank',1,'50444444444321','HDFC0003789','AAPPP5612O','wdfwqdasdasd','Mr. Test');

INSERT INTO "Branch" ("BranchName","BranchAddress","CreationDate","CompanyID","BranchEmail","BranchPhone","BranchManagerName", "BranchManagerSign")
VALUES ('Test Branch','Test Address',CURRENT_DATE,1,'testbranch@test.com','+911111111111','Manager Test', 'wc3wwscwscwcwsdc');

INSERT INTO "Branch" ("BranchName","BranchAddress","CreationDate","CompanyID","BranchEmail","BranchPhone","BranchManagerName", "BranchManagerSign")
VALUES ('Test Branch1','Test Address',CURRENT_DATE,2,'testbranch@test.com','+911111111111','Manager Test', 'wc3wwscwscwcwsdc');

INSERT INTO "BillSeries"("SeriesName") VALUES ('Test');

INSERT INTO "Particulars"("ParticularsName","SACCode","Amount") VALUES ('Test particular',112211,1200.5);

INSERT INTO "DebtorGroup"("DebtorGroupName","DebtorGroupAddress","DebtorGroupMail","DebtorGroupPhoneNumber","DebtorGroupCity","DebtorGSTIN","DebtorOutstanding","BranchID") 
VALUES ('Test Debtor','Test Address','test@test.com','+918732992181','Test','24CAGPP9521D1ZS',11,1);

INSERT INTO "Debtor"("DebtorName","DebtorOutstanding","DebtorGroupID")
VALUES ('Test',11,1);

INSERT INTO "Bill"("BillNumber","BilledTo","BillAmount","Note","InvoiceDate","PlaceOfSupply","DebtorGroupID","CompanyID","SeriesName","BillDate","BillActNum","BillDelivered","BranchID","SecretUnlockCode","MessageSent")
VALUES ('Test/19-20/1','Sangarsh Laminates',1200,'First Bill',CURRENT_DATE,1,1,1,'Test',CURRENT_DATE,1,true,1,111111,true);

INSERT INTO "BillDetails"("ParticularsName","Period","YearInfo","Amount","CGSTAmount","SGSTAmount","TaxableValue","BillAmountOutstanding","CompanyID","ParticularsID","BillNumber","DebtorID","DebtorGroupID") 
VALUES('Test particular','19-20',1,1200,108,108,216,1200,1,1,'Test/19-20/1',1,1);

INSERT INTO "Bill"("BillNumber","BilledTo","BillAmount","Note","InvoiceDate","PlaceOfSupply","DebtorGroupID","CompanyID","SeriesName","BillDate","BillActNum","BillDelivered","BranchID","SecretUnlockCode","MessageSent")
VALUES ('1','Shyam Timber',1200,'First Bill',CURRENT_DATE,NULL,1,1,NULL,CURRENT_DATE,NULL,true,1,111111,true);

INSERT INTO "BillDetails"("ParticularsName","Period","YearInfo","Amount","CGSTAmount","SGSTAmount","TaxableValue","BillAmountOutstanding","CompanyID","ParticularsID","BillNumber","DebtorID","DebtorGroupID") 
VALUES('Test particular','19-20',1,1200,108,108,216,1200,1,1,'1',1,1);