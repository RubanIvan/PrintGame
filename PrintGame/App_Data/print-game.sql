--CREATE DATABASE PrintGameData;
--DROP TABLE Game;
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


--DELETE FROM Game;
SELECT * FROM Game

SELECT * FROM GameImage

SELECT * FROM GameTag

--DELETE FROM GameImage;

--DELETE FROM GameTag

--DELETE FROM Game WHERE GameID=7