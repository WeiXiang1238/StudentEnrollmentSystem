USE [StudentEnrollmentSystemDB]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Admins] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactUs]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactUs](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[StudentEmail] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SubmittedAt] [datetime2](7) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ContactUs] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[CourseID] [int] IDENTITY(1,1) NOT NULL,
	[CourseCode] [nvarchar](max) NOT NULL,
	[CourseName] [nvarchar](150) NOT NULL,
	[Day] [nvarchar](max) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[Venue] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Credits] [int] NOT NULL,
	[Lecturer] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollments]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollments](
	[EnrollmentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[CourseID] [int] NOT NULL,
	[SemesterID] [int] NOT NULL,
	[EnrollmentDate] [datetime2](7) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[DropDate] [datetime2](7) NULL,
	[PaymentDate] [datetime2](7) NULL,
	[Action] [nvarchar](10) NOT NULL,
	[PaymentID] [int] NULL,
 CONSTRAINT [PK_Enrollments] PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[PaymentMethod] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Semesters]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Semesters](
	[SemesterID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Semesters] PRIMARY KEY CLUSTERED 
(
	[SemesterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[ContactNumber] [nvarchar](15) NOT NULL,
	[BankAccNum] [nvarchar](max) NOT NULL,
	[BankAccName] [nvarchar](max) NOT NULL,
	[Bank] [nvarchar](max) NOT NULL,
	[Address1] [nvarchar](200) NULL,
	[Address2] [nvarchar](200) NULL,
	[City] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[EmergencyContactName] [nvarchar](200) NULL,
	[EmergencyContactPhoneNumber] [nvarchar](20) NULL,
	[EmergencyContactRelationship] [nvarchar](100) NULL,
	[ICNumber] [nvarchar](20) NOT NULL,
	[MatricNo] [nvarchar](20) NOT NULL,
	[PostCode] [nvarchar](8) NULL,
	[Program] [nvarchar](200) NOT NULL,
	[State] [nvarchar](100) NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemSettings]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSettings](
	[SystemSettingID] [int] IDENTITY(1,1) NOT NULL,
	[EnabledEnrollment] [int] NOT NULL,
	[CurrentSemester] [int] NOT NULL,
 CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED 
(
	[SystemSettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeachingEvaluations]    Script Date: 22/2/2025 12:14:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeachingEvaluations](
	[EvaluationID] [int] IDENTITY(1,1) NOT NULL,
	[EnrollmentID] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Comments] [nvarchar](1000) NOT NULL,
	[SubmittedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TeachingEvaluations] PRIMARY KEY CLUSTERED 
(
	[EvaluationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Admins] ON 

INSERT [dbo].[Admins] ([AdminID], [Name], [Email], [Password]) VALUES (1, N'Admin', N'admin@gmail.com', N'$2a$11$xZmEWH9Rgu1jnIQIKKwD3O8.2dHHcj1rCB6K3s/Afk5NCNC7.zy2W')
SET IDENTITY_INSERT [dbo].[Admins] OFF
GO
SET IDENTITY_INSERT [dbo].[Courses] ON 

INSERT [dbo].[Courses] ([CourseID], [CourseCode], [CourseName], [Day], [StartTime], [EndTime], [Venue], [Amount], [Credits], [Lecturer]) VALUES (1, N'MPU123', N'Community Service', N'Monday', CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), N'Online', CAST(350.00 AS Decimal(18, 2)), 3, N'Dr. Alex Tan')
INSERT [dbo].[Courses] ([CourseID], [CourseCode], [CourseName], [Day], [StartTime], [EndTime], [Venue], [Amount], [Credits], [Lecturer]) VALUES (2, N'PRG124', N'Web Development', N'Wednesday', CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), N'Online', CAST(500.00 AS Decimal(18, 2)), 4, N'Prof. Linda Chan')
INSERT [dbo].[Courses] ([CourseID], [CourseCode], [CourseName], [Day], [StartTime], [EndTime], [Venue], [Amount], [Credits], [Lecturer]) VALUES (3, N'PRG125', N'ERP Programming', N'Friday', CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), N'Online', CAST(500.00 AS Decimal(18, 2)), 4, N'Dr. Michael Lim')
INSERT [dbo].[Courses] ([CourseID], [CourseCode], [CourseName], [Day], [StartTime], [EndTime], [Venue], [Amount], [Credits], [Lecturer]) VALUES (4, N'ITM126', N'Quantitative Methods', N'Thursday', CAST(N'20:30:30' AS Time), CAST(N'21:30:30' AS Time), N'Online', CAST(500.00 AS Decimal(18, 2)), 4, N'Dr. Karen Lee')
INSERT [dbo].[Courses] ([CourseID], [CourseCode], [CourseName], [Day], [StartTime], [EndTime], [Venue], [Amount], [Credits], [Lecturer]) VALUES (5, N'ITM127', N'IT Management', N'Tuesday', CAST(N'20:30:30' AS Time), CAST(N'21:30:30' AS Time), N'Online', CAST(500.00 AS Decimal(18, 2)), 4, N'Prof. David Ng')
SET IDENTITY_INSERT [dbo].[Courses] OFF
GO
SET IDENTITY_INSERT [dbo].[Semesters] ON 

INSERT [dbo].[Semesters] ([SemesterID], [Name], [StartDate], [EndDate]) VALUES (1, N'JAN 2025', CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-02-28T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Semesters] ([SemesterID], [Name], [StartDate], [EndDate]) VALUES (2, N'MAR 2025', CAST(N'2025-03-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-30T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Semesters] ([SemesterID], [Name], [StartDate], [EndDate]) VALUES (3, N'MAY 2025', CAST(N'2025-05-01T00:00:00.0000000' AS DateTime2), CAST(N'2026-07-31T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Semesters] OFF
GO
SET IDENTITY_INSERT [dbo].[Students] ON 

INSERT [dbo].[Students] ([StudentID], [FullName], [Email], [Password], [ContactNumber], [BankAccNum], [BankAccName], [Bank], [Address1], [Address2], [City], [Country], [EmergencyContactName], [EmergencyContactPhoneNumber], [EmergencyContactRelationship], [ICNumber], [MatricNo], [PostCode], [Program], [State]) VALUES (1, N'John Doe', N'john.doe@gmail.com', N'$2a$11$YvPDHIl0PY/6FxGUO5qmYOSqmwEyCJooafusMYyKlgvk3iXQCvjY2', N'0123456789', N'123456789', N'John Doe', N'Maybank', N'123, Main Street', NULL, N'Kuala Lumpur', N'Malaysia', NULL, NULL, NULL, N'900101-10-1234', N'S12345', N'55520', N'Computer Science', N'WP Kuala Lumpur')
INSERT [dbo].[Students] ([StudentID], [FullName], [Email], [Password], [ContactNumber], [BankAccNum], [BankAccName], [Bank], [Address1], [Address2], [City], [Country], [EmergencyContactName], [EmergencyContactPhoneNumber], [EmergencyContactRelationship], [ICNumber], [MatricNo], [PostCode], [Program], [State]) VALUES (2, N'Jane Smith', N'jane.smith@gmail.com', N'$2a$11$DUILbZej75u0k/wiin68i.eHuBEUqGvCC4NYM3Nj601vIXlv5KiuW', N'0176543210', N'987654321', N'Jane Smith', N'CIMB', N'456, City Avenue', NULL, N'Selangor', N'Malaysia', NULL, NULL, NULL, N'880202-05-5678', N'S67890', N'56520', N'Information Technology', N'Selangor')
SET IDENTITY_INSERT [dbo].[Students] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemSettings] ON 

INSERT [dbo].[SystemSettings] ([SystemSettingID], [EnabledEnrollment], [CurrentSemester]) VALUES (1, 1, 1)
SET IDENTITY_INSERT [dbo].[SystemSettings] OFF
GO
ALTER TABLE [dbo].[ContactUs] ADD  DEFAULT (N'') FOR [Category]
GO
ALTER TABLE [dbo].[Courses] ADD  DEFAULT (N'') FOR [Lecturer]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (N'') FOR [ICNumber]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (N'') FOR [MatricNo]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (N'') FOR [Program]
GO
ALTER TABLE [dbo].[TeachingEvaluations] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [SubmittedAt]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Courses_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Courses] ([CourseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Courses_CourseID]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Payments_PaymentID] FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payments] ([PaymentID])
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Payments_PaymentID]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Semesters_SemesterID] FOREIGN KEY([SemesterID])
REFERENCES [dbo].[Semesters] ([SemesterID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Semesters_SemesterID]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Students_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Students_StudentID]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Students_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Students_StudentID]
GO
ALTER TABLE [dbo].[TeachingEvaluations]  WITH CHECK ADD  CONSTRAINT [FK_TeachingEvaluations_Enrollments_EnrollmentID] FOREIGN KEY([EnrollmentID])
REFERENCES [dbo].[Enrollments] ([EnrollmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeachingEvaluations] CHECK CONSTRAINT [FK_TeachingEvaluations_Enrollments_EnrollmentID]
GO
