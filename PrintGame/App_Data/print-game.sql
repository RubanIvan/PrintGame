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
	ImageGameplay	int NOT NULL,						--Кол-во картинок с процессом игры
	ImageScans		int NOT NULL,						--Кол-во картинок примеры сканов
	CreateTime		SmallDateTime NOT NULL				--Время добавления 
)
