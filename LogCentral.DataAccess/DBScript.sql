IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Device', N'COLUMN',N'Platform'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Device', @level2type=N'COLUMN',@level2name=N'Platform'

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] DROP CONSTRAINT [FK_Log_Users]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Device]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] DROP CONSTRAINT [FK_Log_Device]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] DROP CONSTRAINT [FK_Log_Application]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Application_RegisterationUtcDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Application] DROP CONSTRAINT [DF_Application_RegisterationUtcDate]
END

GO
/****** Object:  Table [dbo].[User]    Script Date: 2019-06-17 11:20:58 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2019-06-17 11:20:58 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log]') AND type in (N'U'))
DROP TABLE [dbo].[Log]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 2019-06-17 11:20:58 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
DROP TABLE [dbo].[Device]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 2019-06-17 11:20:58 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application]') AND type in (N'U'))
DROP TABLE [dbo].[Application]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 2019-06-17 11:20:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Application](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[AppStoreIdentifier] [nvarchar](75) COLLATE Latin1_General_CI_AS NOT NULL,
	[RegisterationUtcDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Device]    Script Date: 2019-06-17 11:20:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Device](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[OwnerName] [nvarchar](128) COLLATE Latin1_General_CI_AS NULL,
	[Platform] [tinyint] NULL,
	[RegisterationUtcDate] [datetime] NOT NULL CONSTRAINT [DF_Device_RegisterationDate]  DEFAULT (getutcdate()),
	[Descriptions] [nvarchar](max) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2019-06-17 11:20:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Log](
	[Id] [uniqueidentifier] NOT NULL,
	[UtcTime] [datetime] NOT NULL CONSTRAINT [DF_Log_UtcTime]  DEFAULT (getutcdate()),
	[LocalTime] [datetimeoffset](7) NOT NULL CONSTRAINT [DF_Log_LocalTime]  DEFAULT (getdate()),
	[Title] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Section] [nvarchar](512) COLLATE Latin1_General_CI_AS NULL,
	[LogType] [tinyint] NOT NULL CONSTRAINT [DF_Log_LogType]  DEFAULT ((0)),
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Username] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Device] [uniqueidentifier] NULL,
	[Application] [uniqueidentifier] NULL,
	[Descriptions] [nvarchar](max) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[User]    Script Date: 2019-06-17 11:20:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[Username] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Title] [nvarchar](6) COLLATE Latin1_General_CI_AS NULL,
	[FirstName] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LastName] [nvarchar](75) COLLATE Latin1_General_CI_AS NULL,
	[RegisterationUtcDate] [datetime] NOT NULL CONSTRAINT [DF_Users_RegisterationUtcDate]  DEFAULT (getutcdate()),
	[Descriptions] [nvarchar](max) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
INSERT [dbo].[Device] ([Id], [Name], [OwnerName], [Platform], [RegisterationUtcDate], [Descriptions]) VALUES (N'f8e5c6cd-5b48-49d4-b3f1-1c5dcb8a8445', N'device1', N'mydevice1', 0, CAST(N'2019-06-10 20:29:31.303' AS DateTime), NULL)
GO
INSERT [dbo].[Device] ([Id], [Name], [OwnerName], [Platform], [RegisterationUtcDate], [Descriptions]) VALUES (N'c6ee866b-c781-4bd3-badf-b7fcdadb551a', N'device2', N'mydevice2', 0, CAST(N'2019-06-10 20:29:36.903' AS DateTime), NULL)
GO
INSERT [dbo].[Device] ([Id], [Name], [OwnerName], [Platform], [RegisterationUtcDate], [Descriptions]) VALUES (N'42dc83c5-ed9f-4a8d-a53c-b83f54351ab8', N'device3', N'mydevice3', 0, CAST(N'2019-06-10 20:29:43.927' AS DateTime), NULL)
GO
INSERT [dbo].[Log] ([Id], [UtcTime], [LocalTime], [Title], [Section], [LogType], [Latitude], [Longitude], [Username], [Device], [Application], [Descriptions]) VALUES (N'b1e83450-cefc-481f-942d-1186aeb9c9cb', CAST(N'2019-06-10 20:20:55.430' AS DateTime), CAST(N'2019-06-10T16:20:55.4300000+00:00' AS DateTimeOffset), N'Log3', N'SQL Server', 0, NULL, NULL, N'user1', N'f8e5c6cd-5b48-49d4-b3f1-1c5dcb8a8445', NULL, N'Few descriptions')
GO
INSERT [dbo].[Log] ([Id], [UtcTime], [LocalTime], [Title], [Section], [LogType], [Latitude], [Longitude], [Username], [Device], [Application], [Descriptions]) VALUES (N'bff3cc4a-6a10-4eed-b288-2372a98ac11e', CAST(N'2019-06-10 20:21:01.490' AS DateTime), CAST(N'2019-06-10T16:21:01.4900000+00:00' AS DateTimeOffset), N'Log4', N'SQL Server', 0, NULL, NULL, N'user2', N'42dc83c5-ed9f-4a8d-a53c-b83f54351ab8', NULL, N'Few descriptions')
GO
INSERT [dbo].[Log] ([Id], [UtcTime], [LocalTime], [Title], [Section], [LogType], [Latitude], [Longitude], [Username], [Device], [Application], [Descriptions]) VALUES (N'ff39276e-59a2-485b-8e92-5cfa8dd69ef9', CAST(N'2019-06-10 20:21:04.710' AS DateTime), CAST(N'2019-06-10T16:21:04.7100000+00:00' AS DateTimeOffset), N'Log5', N'SQL Server', 0, NULL, NULL, N'user2', N'42dc83c5-ed9f-4a8d-a53c-b83f54351ab8', NULL, N'Few descriptions')
GO
INSERT [dbo].[Log] ([Id], [UtcTime], [LocalTime], [Title], [Section], [LogType], [Latitude], [Longitude], [Username], [Device], [Application], [Descriptions]) VALUES (N'31e5b19e-8fbc-49a7-be38-96e4ba4c7401', CAST(N'2019-06-10 20:20:31.130' AS DateTime), CAST(N'2019-06-10T16:20:31.1300000+00:00' AS DateTimeOffset), N'Log1', N'SQL Server', 0, NULL, NULL, N'user1', N'f8e5c6cd-5b48-49d4-b3f1-1c5dcb8a8445', NULL, N'Few descriptions')
GO
INSERT [dbo].[Log] ([Id], [UtcTime], [LocalTime], [Title], [Section], [LogType], [Latitude], [Longitude], [Username], [Device], [Application], [Descriptions]) VALUES (N'095dd8d8-4058-4465-8e23-b231f13c79b4', CAST(N'2019-06-10 20:20:52.357' AS DateTime), CAST(N'2019-06-10T16:20:52.3530000+00:00' AS DateTimeOffset), N'Log2', N'SQL Server', 0, NULL, NULL, N'user1', N'c6ee866b-c781-4bd3-badf-b7fcdadb551a', NULL, N'Few descriptions')
GO
INSERT [dbo].[Log] ([Id], [UtcTime], [LocalTime], [Title], [Section], [LogType], [Latitude], [Longitude], [Username], [Device], [Application], [Descriptions]) VALUES (N'19be63eb-0152-4ab1-b8d3-fe98527c1a2d', CAST(N'2019-06-10 20:56:29.447' AS DateTime), CAST(N'2019-06-10T16:56:29.4458751-04:00' AS DateTimeOffset), N'Test Put Log', N'Postman', 0, NULL, NULL, NULL, NULL, NULL, N'Few put description few put description few put description..')
GO
INSERT [dbo].[User] ([Username], [Title], [FirstName], [LastName], [RegisterationUtcDate], [Descriptions]) VALUES (N'user1', N'Mr.', N'John', N'Snow', CAST(N'2019-06-10 20:17:12.553' AS DateTime), NULL)
GO
INSERT [dbo].[User] ([Username], [Title], [FirstName], [LastName], [RegisterationUtcDate], [Descriptions]) VALUES (N'user2', N'Mrs.', N'Jamie', N'Lannister', CAST(N'2019-06-10 20:17:30.193' AS DateTime), NULL)
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Application_RegisterationUtcDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Application] ADD  CONSTRAINT [DF_Application_RegisterationUtcDate]  DEFAULT (getutcdate()) FOR [RegisterationUtcDate]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Application] FOREIGN KEY([Application])
REFERENCES [dbo].[Application] ([Id])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Application]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Device]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Device] FOREIGN KEY([Device])
REFERENCES [dbo].[Device] ([Id])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Device]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Device]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Users] FOREIGN KEY([Username])
REFERENCES [dbo].[User] ([Username])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Log_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Log]'))
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Users]
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Device', N'COLUMN',N'Platform'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=Unknown, 1=Windows, 2=Android, 3=iOS, 4=Linux, 5=Others' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Device', @level2type=N'COLUMN',@level2name=N'Platform'
GO
