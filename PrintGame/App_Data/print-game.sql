--CREATE DATABASE PrintGameData;
--DROP TABLE Game;
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


--DELETE FROM Game;
SELECT * FROM Game

SELECT * FROM GameImage

SELECT * FROM GameTag

--DELETE FROM GameImage;

--DELETE FROM GameTag

--DELETE FROM Game WHERE GameID=7