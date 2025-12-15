USE [Tips]
GO

/****** Object:  Table [dbo].[WeatherForecasts]    Script Date: 2025-12-15 18:25:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WeatherForecasts](
	[Id] [int] NOT NULL,
	[Date] [date] NULL,
	[TemperatureC] [int] NULL,
	[Summary] [varchar](50) NULL
) ON [PRIMARY]
GO


