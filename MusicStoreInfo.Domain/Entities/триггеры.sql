
--Before Insert Trigger
CREATE TRIGGER trgBeforeInsertAlbums
ON dbo.Albums
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @Name NVARCHAR(150), @ListenerTypeId INT, @CompanyId INT, @GroupId INT, @Duration INT, @ReleaseDate DATETIME2, @SongsCount INT, @ImagePath NVARCHAR(MAX);

    SELECT 
        @Name = Name, 
        @ListenerTypeId = ListenerTypeId, 
        @CompanyId = CompanyId, 
        @GroupId = GroupId, 
        @Duration = Duration, 
        @ReleaseDate = ReleaseDate, 
        @SongsCount = SongsCount, 
        @ImagePath = ImagePath
    FROM inserted;

    -- Пример проверки: Проверка, что SongsCount не меньше 0
    IF @SongsCount < 0
    BEGIN
        RAISERROR ('SongsCount cannot be negative', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Вставка данных
    INSERT INTO dbo.Albums (Name, ListenerTypeId, CompanyId, GroupId, Duration, ReleaseDate, SongsCount, ImagePath)
    VALUES (@Name, @ListenerTypeId, @CompanyId, @GroupId, @Duration, @ReleaseDate, @SongsCount, @ImagePath);
END




--After Insert Trigger

CREATE TRIGGER trgAfterInsertAlbums
ON dbo.Albums
AFTER INSERT
AS
BEGIN
    -- Пример: Логирование вставки новой записи
    INSERT INTO dbo.AuditLog (Action, TableName, RecordId, ActionDate)
    SELECT 'INSERT', 'Albums', Id, GETDATE()
    FROM inserted;
END


--After Update Trigger

CREATE TRIGGER trgAfterUpdateAlbums
ON dbo.Albums
AFTER UPDATE
AS
BEGIN
    -- Пример: Логирование обновления записи
    INSERT INTO dbo.AuditLog (Action, TableName, RecordId, ActionDate, OldValue, NewValue)
    SELECT 'UPDATE', 'Albums', i.Id, GETDATE(), d.Name, i.Name
    FROM inserted i
    INNER JOIN deleted d ON i.Id = d.Id;
END


--Before Delete Trigger
CREATE TRIGGER trgBeforeDeleteAlbums
ON dbo.Albums
INSTEAD OF DELETE
AS
BEGIN
    DECLARE @Id INT;

    SELECT @Id = Id FROM deleted;

    -- Пример проверки: Предотвращение удаления записи с определенным Id
    IF @Id = 1
    BEGIN
        RAISERROR ('Cannot delete this record', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Удаление данных
    DELETE FROM dbo.Albums WHERE Id = @Id;
END

--Before Update Trigger
CREATE TRIGGER trgBeforeUpdateAlbums
ON dbo.Albums
INSTEAD OF UPDATE
AS
BEGIN
    DECLARE @Id INT, @Name NVARCHAR(150), @ListenerTypeId INT, @CompanyId INT, @GroupId INT, @Duration INT, @ReleaseDate DATETIME2, @SongsCount INT, @ImagePath NVARCHAR(MAX);

    SELECT 
        @Id = Id, 
        @Name = Name, 
        @ListenerTypeId = ListenerTypeId, 
        @CompanyId = CompanyId, 
        @GroupId = GroupId, 
        @Duration = Duration, 
        @ReleaseDate = ReleaseDate, 
        @SongsCount = SongsCount, 
        @ImagePath = ImagePath
    FROM inserted;

    -- Пример проверки: Проверка, что SongsCount не меньше 0
    IF @SongsCount < 0
    BEGIN
        RAISERROR ('SongsCount cannot be negative', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Обновление данных
    UPDATE dbo.Albums
    SET Name = @Name, ListenerTypeId = @ListenerTypeId, CompanyId = @CompanyId, GroupId = @GroupId, Duration = @Duration, ReleaseDate = @ReleaseDate, SongsCount = @SongsCount, ImagePath = @ImagePath
    WHERE Id = @Id;
END

--After Delete Trigger
CREATE TRIGGER trgAfterDeleteAlbums
ON dbo.Albums
AFTER DELETE
AS
BEGIN
    -- Пример: Логирование удаления записи
    INSERT INTO dbo.AuditLog (Action, TableName, RecordId, ActionDate)
    SELECT 'DELETE', 'Albums', Id, GETDATE()
    FROM deleted;
END