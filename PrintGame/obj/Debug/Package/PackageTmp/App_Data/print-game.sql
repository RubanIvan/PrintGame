--CREATE DATABASE PrintGameData;
--DROP TABLE Game;
--�������� ������� � �����
CREATE TABLE Game(
	GameID			int IDENTITY NOT NULL PRIMARY KEY,	--������������� ����
	TitleRu			nvarchar(1024) NOT NULL UNIQUE,		--�������� �� ������
	TitleEn			nvarchar(1024) NOT NULL UNIQUE,		--�������� �� ���������
	CatalogName		nvarchar(1024) NOT NULL UNIQUE,		--������� � �����
	YearPublishing	int NOT NULL,						--��� �������
	Rating			float(24) NOT NULL,					--������� �������
	Lang			nvarchar(10),						--���� ����
	Descript		nvarchar(MAX) NOT NULL,				--�������� ����
	NumOfPlayers	nvarchar(512),						--���������� �������
	NumOfSuggested	nvarchar(512),						--������������� ���������� �������
	SuggestedAges	nvarchar(512),						--������������� ������� �������
	Acquaintance	nvarchar(512),						--����� �������� ����
	PlayingTime		nvarchar(512),						--����� ������
	Components		nvarchar(MAX) NOT NULL,				--������ �������
	CreateTime		SmallDateTime NOT NULL				--����� ���������� 
)

--������� ��� �������� �����������
CREATE TABLE GameImage(
	GameImageID		int IDENTITY NOT NULL PRIMARY KEY,	--������������� ��������
	GameID			int,								--������������� ����
	DescriptImage	nvarchar(MAX) NOT NULL,				--�������� ��������
	SmallImagePath  nvarchar(1024),						--���� � ������ ��������
	FulllImagePath  nvarchar(1024),						--���� � ��������
)

ALTER TABLE GameImage WITH CHECK ADD  CONSTRAINT [FK_GameID_Game] FOREIGN KEY (GameID)
REFERENCES Game(GameID)


--DROP TABLE Tag

-- ������� �������� ������ �����
CREATE TABLE Tag(
		TagID			int IDENTITY NOT NULL PRIMARY KEY,	--�������������  ����
		TagName			nvarchar(512) NOT NULL UNIQUE,		--���
)

--DROP TABLE GameTag

-- ������� ��������� ������ �� ������
CREATE TABLE GameTag(
		GameTagID			int IDENTITY NOT NULL PRIMARY KEY,	--�������������  ����
		GameID				int NOT NULL,
		TagID				int NOT NULL
)

ALTER TABLE GameTag WITH CHECK ADD  CONSTRAINT [FK_GameTag_GameID] FOREIGN KEY (GameID)
REFERENCES Game(GameID)

ALTER TABLE GameTag WITH CHECK ADD  CONSTRAINT [FK_GameTag_TagID] FOREIGN KEY (TagID)
REFERENCES Tag(TagID)


--DROP TABLE FileShare
--������� ��� �������� ������� ������ �� ����
CREATE TABLE FileShare(
		FileShareID			int IDENTITY NOT NULL PRIMARY KEY,	--�������������	
		GameID				int NOT NULL,						--������������� ����	
		FileShareName		nvarchar(1024) NOT NULL,			--��� ������
		FileShareUrl1		nvarchar(1024),						--URL ����� �� 1 ��������������
		FileShareUrl2		nvarchar(1024),						--URL ����� �� 2 ��������������
		FileShareUrl3		nvarchar(1024),						--URL ����� �� 3 ��������������
		FileShareUrl4		nvarchar(1024),						--URL ����� �� 4 ��������������
		FileShareSize		nvarchar(512) NOT NULL,				--������ �����
		FileShareDesc		nvarchar(2048) NOT NULL,			--�������� �����
)

ALTER TABLE FileShare WITH CHECK ADD  CONSTRAINT [FK_FileShare_GameID] FOREIGN KEY (GameID)
REFERENCES Game(GameID)
GO

-- ������� ������ �� ���� �� �������
CREATE PROC DeleteGameByID
@ID int
AS

DELETE FROM GameTag WHERE GameID=@ID
DELETE FROM GameImage WHERE GameID=@ID
DELETE FROM Game WHERE GameID=@ID
GO

--DROP FUNCTION GamePerPageQuantity
--���������� ���-�� ��� �� �����
GO
CREATE FUNCTION GamePerPageQuantity()
RETURNS int
AS
BEGIN
DECLARE @tmp int =3;
RETURN @tmp;
END
GO

--���������� �������� � ������
CREATE PROCEDURE GetGamePage
	@PageNum int = 0	--����� ��������
AS
	--��������� ���������� ��� �� ��������
	DECLARE @PageSize int =	dbo.GamePerPageQuantity();
				
	SELECT * FROM Game
	ORDER BY GameID DESC
	OFFSET @PageNum*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO

EXEC GamePerPageQuantity;

EXEC GetGamePage 0;
EXEC GetGamePage 1;
EXEC GetGamePage 2;



--������� ��������� �������� �� ����
CREATE PROCEDURE GetGameBoxImage
	@GameID int		--Id ����
AS
	--����� ����� ������ �������� ��� � ���� ������� � �����
	SELECT TOP 1 * FROM GameImage WHERE GameID=@GameID
	ORDER By GameImageID 
GO





--EXEC DeleteGameByID 

--DELETE FROM Game;
SELECT * FROM Game
SELECT * FROM GameImage
SELECT * FROM GameTag
SELECT * FROM FileShare;

--DELETE FROM GameImage;

--DELETE FROM GameTag

--DELETE FROM Game WHERE GameID=7

