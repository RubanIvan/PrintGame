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
	ImageGameplay	int NOT NULL,						--���-�� �������� � ��������� ����
	ImageScans		int NOT NULL,						--���-�� �������� ������� ������
	CreateTime		SmallDateTime NOT NULL				--����� ���������� 
)
