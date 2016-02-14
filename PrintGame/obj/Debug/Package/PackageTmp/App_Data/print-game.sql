--CREATE DATABASE PrintGameData;
--DROP TABLE Game;
--основная таблица с игрой
CREATE TABLE Game(
	GameID			int IDENTITY NOT NULL PRIMARY KEY,	--идентификатор игры
	TitleRu			nvarchar(1024) NOT NULL UNIQUE,		--Название по русски
	TitleEn			nvarchar(1024) NOT NULL UNIQUE,		--Название по английски
	CatalogName		nvarchar(1024) NOT NULL UNIQUE,		--Каталог с игрой
	YearPublishing	int NOT NULL,						--Год издания
	Rating			float(24) NOT NULL,					--Средний рейтинг
	Lang			nvarchar(10),						--Язык игры
	Descript		nvarchar(MAX) NOT NULL,				--Описание игры
	NumOfPlayers	nvarchar(512),						--Количество игроков
	NumOfSuggested	nvarchar(512),						--Рекомендуемое количество игроков
	SuggestedAges	nvarchar(512),						--Рекомендуемый возраст игроков
	Acquaintance	nvarchar(512),						--Время освоения игры
	PlayingTime		nvarchar(512),						--Время партии
	Components		nvarchar(MAX) NOT NULL,				--Состав коробки
	CreateTime		SmallDateTime NOT NULL				--Время добавления 
)

--таблица для хранения изображений
CREATE TABLE GameImage(
	GameImageID		int IDENTITY NOT NULL PRIMARY KEY,	--идентификатор картинки
	GameID			int,								--идентификатор игры
	DescriptImage	nvarchar(MAX) NOT NULL,				--Описание картинки
	SmallImagePath  nvarchar(1024),						--путь к превью картинки
	FulllImagePath  nvarchar(1024),						--путь к картинки
)

ALTER TABLE GameImage WITH CHECK ADD  CONSTRAINT [FK_GameID_Game] FOREIGN KEY (GameID)
REFERENCES Game(GameID)


--DROP TABLE Tag

-- таблица содержит список кэгов
CREATE TABLE Tag(
		TagID			int IDENTITY NOT NULL PRIMARY KEY,	--идентификатор  Тэга
		TagName			nvarchar(512) NOT NULL UNIQUE,		--Тэг
)

--DROP TABLE GameTag

-- таблица отношения многих ко многим
CREATE TABLE GameTag(
		GameTagID			int IDENTITY NOT NULL PRIMARY KEY,	--идентификатор  Тэга
		GameID				int NOT NULL,
		TagID				int NOT NULL
)

ALTER TABLE GameTag WITH CHECK ADD  CONSTRAINT [FK_GameTag_GameID] FOREIGN KEY (GameID)
REFERENCES Game(GameID)

ALTER TABLE GameTag WITH CHECK ADD  CONSTRAINT [FK_GameTag_TagID] FOREIGN KEY (TagID)
REFERENCES Tag(TagID)


--DROP TABLE FileShare
--таблица для хранения внешних ссылок на файл
CREATE TABLE FileShare(
		FileShareID			int IDENTITY NOT NULL PRIMARY KEY,	--идентификатор	
		GameID				int NOT NULL,						--идентификатор игры	
		FileShareName		nvarchar(1024) NOT NULL,			--имя архива
		FileShareUrl1		nvarchar(1024),						--URL файла на 1 файлообменнике
		FileShareUrl2		nvarchar(1024),						--URL файла на 2 файлообменнике
		FileShareUrl3		nvarchar(1024),						--URL файла на 3 файлообменнике
		FileShareUrl4		nvarchar(1024),						--URL файла на 4 файлообменнике
		FileShareSize		nvarchar(512) NOT NULL,				--размер файла
		FileShareDesc		nvarchar(2048) NOT NULL,			--описание файла
)

ALTER TABLE FileShare WITH CHECK ADD  CONSTRAINT [FK_FileShare_GameID] FOREIGN KEY (GameID)
REFERENCES Game(GameID)
GO

-- удалить данные об игре по индексу
CREATE PROC DeleteGameByID
@ID int
AS

DELETE FROM GameTag WHERE GameID=@ID
DELETE FROM GameImage WHERE GameID=@ID
DELETE FROM Game WHERE GameID=@ID
GO

--DROP FUNCTION GamePerPageQuantity
--возвращает кол-во игр на листе
GO
CREATE FUNCTION GamePerPageQuantity()
RETURNS int
AS
BEGIN
DECLARE @tmp int =3;
RETURN @tmp;
END
GO

--возвращает страницу с играми
CREATE PROCEDURE GetGamePage
	@PageNum int = 0	--номер страницы
AS
	--извлекаем количество игр на странице
	DECLARE @PageSize int =	dbo.GamePerPageQuantity();
				
	SELECT * FROM Game
	ORDER BY GameID DESC
	OFFSET @PageNum*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO

EXEC GamePerPageQuantity;

EXEC GetGamePage 0;
EXEC GetGamePage 1;
EXEC GetGamePage 2;



--вернуть титульную картинку от игры
CREATE PROCEDURE GetGameBoxImage
	@GameID int		--Id игры
AS
	--берем самую первую картинку это и есть коробка с игрой
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

