USE [ESchool]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Classes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[News]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[Id] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[PublishedDate] [datetime] NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NewsCategories]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsCategories](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_NewsCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NotifyLogs]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotifyLogs](
	[Id] [int] NOT NULL,
	[NewsId] [int] NOT NULL,
	[SentEmail] [bit] NULL,
	[SentSMS] [bit] NULL,
	[SentDate] [datetime] NOT NULL,
 CONSTRAINT [PK_NotifyLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Parents]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parents](
	[Id] [int] NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[Address] [nvarchar](250) NULL,
	[PhoneNumber] [nchar](15) NULL,
	[Email] [nchar](50) NULL,
 CONSTRAINT [PK_Parents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Registration]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registration](
	[Id] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[ClassId] [int] NOT NULL,
	[RegisteredDate] [datetime] NULL,
	[InputScore] [decimal](18, 2) NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Schools]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schools](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Address] [nvarchar](500) NULL,
 CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Students]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[Id] [int] NOT NULL,
	[FirstName] [nvarchar](150) NULL,
	[LastName] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[PhoneNumber] [nchar](15) NULL,
	[ParentId] [int] NOT NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentSchool]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentSchool](
	[Id] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[SchoolId] [int] NOT NULL,
	[InputScore] [decimal](18, 2) NULL,
 CONSTRAINT [PK_StudentSchool] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentTracking]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentTracking](
	[Id] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[Action] [int] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Comment] [nvarchar](500) NULL,
 CONSTRAINT [PK_StudentTracking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentTuition]    Script Date: 3/22/2017 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentTuition](
	[Id] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[Tuition] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
	[Comment] [nvarchar](500) NULL,
 CONSTRAINT [PK_StudentTuition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[NotifyLogs] ADD  CONSTRAINT [DF_NotifyLogs_SentDate]  DEFAULT (getdate()) FOR [SentDate]
GO
ALTER TABLE [dbo].[StudentTracking] ADD  CONSTRAINT [DF_StudentTracking_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_NewsCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[NewsCategories] ([Id])
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_NewsCategories]
GO
ALTER TABLE [dbo].[NotifyLogs]  WITH CHECK ADD  CONSTRAINT [FK_NotifyLogs_News] FOREIGN KEY([NewsId])
REFERENCES [dbo].[News] ([Id])
GO
ALTER TABLE [dbo].[NotifyLogs] CHECK CONSTRAINT [FK_NotifyLogs_News]
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Classes1] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Classes] ([Id])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Classes1]
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Students]
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Parents] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Parents] ([Id])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Parents]
GO
ALTER TABLE [dbo].[StudentSchool]  WITH CHECK ADD  CONSTRAINT [FK_StudentSchool_Schools] FOREIGN KEY([SchoolId])
REFERENCES [dbo].[Schools] ([Id])
GO
ALTER TABLE [dbo].[StudentSchool] CHECK CONSTRAINT [FK_StudentSchool_Schools]
GO
ALTER TABLE [dbo].[StudentSchool]  WITH CHECK ADD  CONSTRAINT [FK_StudentSchool_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentSchool] CHECK CONSTRAINT [FK_StudentSchool_Students]
GO
ALTER TABLE [dbo].[StudentTracking]  WITH CHECK ADD  CONSTRAINT [FK_StudentTracking_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentTracking] CHECK CONSTRAINT [FK_StudentTracking_Students]
GO
ALTER TABLE [dbo].[StudentTuition]  WITH CHECK ADD  CONSTRAINT [FK_StudentTuition_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentTuition] CHECK CONSTRAINT [FK_StudentTuition_Students]
GO
