USE [Customer]
GO

/****** Object: Table [dbo].[State] Script Date: 5/18/2014 9:10:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[State] (
    [Id]         CHAR (2)      NOT NULL,
    [Name]       NVARCHAR (30) NOT NULL,
    [Active]     BIT           NOT NULL,
    [ModifiedDt] DATETIME      NOT NULL,
    [CreateDt]   DATETIME      NOT NULL
	CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([Id] ASC )
)
	ON [PRIMARY]


GO