CREATE TABLE [dbo].[Posts]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(100) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [FK_Posts_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);
