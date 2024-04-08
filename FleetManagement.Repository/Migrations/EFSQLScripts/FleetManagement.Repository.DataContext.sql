IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE TABLE [DeliveryPoints] (
        [Id] int NOT NULL IDENTITY,
        [Type] int NOT NULL,
        [Value] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_DeliveryPoints] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE TABLE [Vehicles] (
        [Id] int NOT NULL IDENTITY,
        [Plate] nvarchar(11) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Vehicles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE TABLE [Bags] (
        [Id] int NOT NULL IDENTITY,
        [Barcode] nvarchar(11) NOT NULL,
        [DeliveryPointId] int NOT NULL,
        [State] int NOT NULL DEFAULT 1,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Bags] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Bags_DeliveryPoints_DeliveryPointId] FOREIGN KEY ([DeliveryPointId]) REFERENCES [DeliveryPoints] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE TABLE [FleetTransactions] (
        [Id] int NOT NULL IDENTITY,
        [TransactionId] uniqueidentifier NOT NULL,
        [VehicleId] int NOT NULL,
        [DeliveryPointId] int NULL,
        [Barcode] nvarchar(11) NOT NULL,
        [State] int NOT NULL,
        [Message] nvarchar(255) NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_FleetTransactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FleetTransactions_DeliveryPoints_DeliveryPointId] FOREIGN KEY ([DeliveryPointId]) REFERENCES [DeliveryPoints] ([Id]),
        CONSTRAINT [FK_FleetTransactions_Vehicles_VehicleId] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE TABLE [Packages] (
        [Id] int NOT NULL IDENTITY,
        [Barcode] nvarchar(11) NOT NULL,
        [DeliveryPointId] int NOT NULL,
        [VolumetricWeight] int NOT NULL,
        [State] int NOT NULL DEFAULT 1,
        [BagId] int NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        CONSTRAINT [PK_Packages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Packages_Bags_BagId] FOREIGN KEY ([BagId]) REFERENCES [Bags] ([Id]),
        CONSTRAINT [FK_Packages_DeliveryPoints_DeliveryPointId] FOREIGN KEY ([DeliveryPointId]) REFERENCES [DeliveryPoints] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Bags_Barcode] ON [Bags] ([Barcode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE INDEX [IX_Bags_DeliveryPointId] ON [Bags] ([DeliveryPointId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_DeliveryPoints_Value] ON [DeliveryPoints] ([Value]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE INDEX [IX_FleetTransactions_DeliveryPointId] ON [FleetTransactions] ([DeliveryPointId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE INDEX [IX_FleetTransactions_VehicleId] ON [FleetTransactions] ([VehicleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE INDEX [IX_Packages_BagId] ON [Packages] ([BagId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Packages_Barcode] ON [Packages] ([Barcode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE INDEX [IX_Packages_DeliveryPointId] ON [Packages] ([DeliveryPointId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Vehicles_Plate] ON [Vehicles] ([Plate]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220406173624_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220406173624_initial', N'6.0.3');
END;
GO

COMMIT;
GO

