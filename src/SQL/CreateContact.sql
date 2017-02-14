USE [Customer]
GO

/****** Object: Table [dbo].[Contact] Script Date: 5/18/2014 9:01:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contact] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (25)   NOT NULL,
    [LastName]   NVARCHAR (30)   NOT NULL,
    [Address1]   NVARCHAR (40)   NULL,
    [Address2]   NVARCHAR (30)   NULL,
    [Notes]      NVARCHAR (150)  NULL,
    [ZipCode]    NVARCHAR (10)   NULL,
    [HomePhone]  NVARCHAR (10)   NULL,
    [WorkPhone]  NVARCHAR (10)   NULL,
    [CellPhone]  NVARCHAR (10)   NULL,
    [EMail]      NVARCHAR (4000) NULL,
    [CityId]     INT             NOT NULL,
    [Active]     BIT             NOT NULL,
    [ModifiedDt] DATETIME        NOT NULL,
    [CreateDt]   DATETIME        NOT NULL,
	CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED (	[Id] ASC)
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO

ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_City]
GO



