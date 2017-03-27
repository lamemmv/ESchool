USE [ESchool]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[Username] [nchar](50) NOT NULL,
	[Password] [nchar](50) NOT NULL,
	[Domain] [nchar](100) NULL,
	[Email] [nchar](50) NULL,
	[Activated] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schools]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[Id] [int] NOT NULL,
	[Question] [nvarchar](500) NOT NULL,
	[DSS] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionSets]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionSets](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_PermissionSets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modules]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modules](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_Modules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parents]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewsCategories]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examination]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examination](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[ExamDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Examination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[Id] [int] NOT NULL,
	[QuestionId] [int] NOT NULL,
	[Answer] [nchar](10) NOT NULL,
	[Body] [nvarchar](250) NULL,
 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ACL]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACL](
	[EntityId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[PermissionSetId] [int] NOT NULL,
	[Bit] [int] NOT NULL,
 CONSTRAINT [PK_ACL] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC,
	[ModuleId] ASC,
	[RoleId] ASC,
	[PermissionSetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] NOT NULL,
	[PermissionSetId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[PermissionSetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserActionLogs]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserActionLogs](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Action] [int] NOT NULL,
	[ActionDate] [datetime] NOT NULL,
	[Comment] [nvarchar](500) NULL,
 CONSTRAINT [PK_UserActionLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentTuition]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentTracking]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentSchool]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentExam]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentExam](
	[Id] [int] NOT NULL,
	[ExamId] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[Grade] [decimal](18, 2) NULL,
	[Comment] [nvarchar](500) NULL,
 CONSTRAINT [PK_StudentExam] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registration]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotifyLogs]    Script Date: 03/27/2017 10:46:14 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamResults]    Script Date: 03/27/2017 10:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamResults](
	[Id] [nchar](10) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[SudentId] [int] NOT NULL,
	[AnswerId] [int] NOT NULL,
	[ExamId] [int] NOT NULL,
 CONSTRAINT [PK_ExamResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Examination_ExamDate]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[Examination] ADD  CONSTRAINT [DF_Examination_ExamDate]  DEFAULT (getdate()) FOR [ExamDate]
GO
/****** Object:  Default [DF_NotifyLogs_SentDate]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[NotifyLogs] ADD  CONSTRAINT [DF_NotifyLogs_SentDate]  DEFAULT (getdate()) FOR [SentDate]
GO
/****** Object:  Default [DF_StudentTracking_TimeStamp]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentTracking] ADD  CONSTRAINT [DF_StudentTracking_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO
/****** Object:  Default [DF_UserActionLogs_ActionDate]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[UserActionLogs] ADD  CONSTRAINT [DF_UserActionLogs_ActionDate]  DEFAULT (getdate()) FOR [ActionDate]
GO
/****** Object:  ForeignKey [FK_ACL_Modules]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ACL]  WITH CHECK ADD  CONSTRAINT [FK_ACL_Modules] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Modules] ([Id])
GO
ALTER TABLE [dbo].[ACL] CHECK CONSTRAINT [FK_ACL_Modules]
GO
/****** Object:  ForeignKey [FK_ACL_PermissionSets]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ACL]  WITH CHECK ADD  CONSTRAINT [FK_ACL_PermissionSets] FOREIGN KEY([PermissionSetId])
REFERENCES [dbo].[PermissionSets] ([Id])
GO
ALTER TABLE [dbo].[ACL] CHECK CONSTRAINT [FK_ACL_PermissionSets]
GO
/****** Object:  ForeignKey [FK_ACL_Roles]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ACL]  WITH CHECK ADD  CONSTRAINT [FK_ACL_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[ACL] CHECK CONSTRAINT [FK_ACL_Roles]
GO
/****** Object:  ForeignKey [FK_Answers_Questions]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Answers_Questions] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Answers_Questions]
GO
/****** Object:  ForeignKey [FK_ExamResults_Answers]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD  CONSTRAINT [FK_ExamResults_Answers] FOREIGN KEY([AnswerId])
REFERENCES [dbo].[Answers] ([Id])
GO
ALTER TABLE [dbo].[ExamResults] CHECK CONSTRAINT [FK_ExamResults_Answers]
GO
/****** Object:  ForeignKey [FK_ExamResults_Examination]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD  CONSTRAINT [FK_ExamResults_Examination] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Examination] ([Id])
GO
ALTER TABLE [dbo].[ExamResults] CHECK CONSTRAINT [FK_ExamResults_Examination]
GO
/****** Object:  ForeignKey [FK_ExamResults_Questions]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD  CONSTRAINT [FK_ExamResults_Questions] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[ExamResults] CHECK CONSTRAINT [FK_ExamResults_Questions]
GO
/****** Object:  ForeignKey [FK_ExamResults_Students]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD  CONSTRAINT [FK_ExamResults_Students] FOREIGN KEY([SudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[ExamResults] CHECK CONSTRAINT [FK_ExamResults_Students]
GO
/****** Object:  ForeignKey [FK_News_NewsCategories]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_NewsCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[NewsCategories] ([Id])
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_NewsCategories]
GO
/****** Object:  ForeignKey [FK_NotifyLogs_News]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[NotifyLogs]  WITH CHECK ADD  CONSTRAINT [FK_NotifyLogs_News] FOREIGN KEY([NewsId])
REFERENCES [dbo].[News] ([Id])
GO
ALTER TABLE [dbo].[NotifyLogs] CHECK CONSTRAINT [FK_NotifyLogs_News]
GO
/****** Object:  ForeignKey [FK_Permissions_PermissionSets]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_PermissionSets] FOREIGN KEY([PermissionSetId])
REFERENCES [dbo].[PermissionSets] ([Id])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_PermissionSets]
GO
/****** Object:  ForeignKey [FK_Registration_Classes1]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Classes1] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Classes] ([Id])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Classes1]
GO
/****** Object:  ForeignKey [FK_Registration_Students]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [FK_Registration_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Students]
GO
/****** Object:  ForeignKey [FK_StudentExam_Examination]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentExam]  WITH CHECK ADD  CONSTRAINT [FK_StudentExam_Examination] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Examination] ([Id])
GO
ALTER TABLE [dbo].[StudentExam] CHECK CONSTRAINT [FK_StudentExam_Examination]
GO
/****** Object:  ForeignKey [FK_StudentExam_Students]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentExam]  WITH CHECK ADD  CONSTRAINT [FK_StudentExam_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentExam] CHECK CONSTRAINT [FK_StudentExam_Students]
GO
/****** Object:  ForeignKey [FK_Students_Parents]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Parents] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Parents] ([Id])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Parents]
GO
/****** Object:  ForeignKey [FK_StudentSchool_Schools]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentSchool]  WITH CHECK ADD  CONSTRAINT [FK_StudentSchool_Schools] FOREIGN KEY([SchoolId])
REFERENCES [dbo].[Schools] ([Id])
GO
ALTER TABLE [dbo].[StudentSchool] CHECK CONSTRAINT [FK_StudentSchool_Schools]
GO
/****** Object:  ForeignKey [FK_StudentSchool_Students]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentSchool]  WITH CHECK ADD  CONSTRAINT [FK_StudentSchool_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentSchool] CHECK CONSTRAINT [FK_StudentSchool_Students]
GO
/****** Object:  ForeignKey [FK_StudentTracking_Students]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentTracking]  WITH CHECK ADD  CONSTRAINT [FK_StudentTracking_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentTracking] CHECK CONSTRAINT [FK_StudentTracking_Students]
GO
/****** Object:  ForeignKey [FK_StudentTuition_Students]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[StudentTuition]  WITH CHECK ADD  CONSTRAINT [FK_StudentTuition_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentTuition] CHECK CONSTRAINT [FK_StudentTuition_Students]
GO
/****** Object:  ForeignKey [FK_UserActionLogs_Users]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[UserActionLogs]  WITH CHECK ADD  CONSTRAINT [FK_UserActionLogs_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserActionLogs] CHECK CONSTRAINT [FK_UserActionLogs_Users]
GO
/****** Object:  ForeignKey [FK_UserRoles_Roles]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
/****** Object:  ForeignKey [FK_UserRoles_Users]    Script Date: 03/27/2017 10:46:14 ******/
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO
