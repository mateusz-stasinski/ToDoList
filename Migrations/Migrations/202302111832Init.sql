CREATE TABLE ToDoTask(
	Id uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
	Day datetimeoffset NOT NULL,
	Description nvarchar(1000) NULL,
  State [int] NOT NULL,
  Priority [int] NOT NULL,
);
GO