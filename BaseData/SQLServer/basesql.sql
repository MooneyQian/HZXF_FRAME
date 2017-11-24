USE [BaseMvc]
GO
/****** Object:  Table [dbo].[SYS_UserRole]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_UserRole](
	[ID] [varchar](36) NOT NULL,
	[UserID] [varchar](36) NULL,
	[RoleID] [varchar](36) NULL,
 CONSTRAINT [PK_SYS_USERROLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'0979d1c7-1cbc-4293-806c-205e2ee60c49', N'fa1c2d7a-d4e1-4cbb-9dea-969ad9847932', N'69f49610-5a8c-43c2-b5de-9d45a7a278f7')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'156c3ba4-134d-4e81-9886-bc1d9a4f86b5', N'ce95b290-7373-4180-b281-0d745c94cd46', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'2832d44b-d2d7-4845-bdbe-150c66545a03', N'abcb4cb3-a949-4510-81d8-85340edb54a1', N'31c25298-72d0-46ae-bf9d-611198c46fd0')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'2dd5e8ee-ce18-46b1-b92f-0af7b635749c', N'c6882cdf-74e4-4ade-a467-6ee15d7af918', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'3684c8da-dbe5-4a0a-bcba-baa4284bc419', N'64e28c5f-472d-48f3-b6ba-c399f6b96e0c', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'45e24c18-1977-4d17-bc7c-76f11ae6c3fb', N'c2003658-7b53-4b40-808e-29def41820fa', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'81d7719e-f3c7-40a5-9560-04e67f87b79f', N'3a4e53ad-2d9f-4f2d-9800-408e6a65f9d8', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'878f3e0a-5d5a-4f18-b88c-1b3944a27222', N'47f4c098-5997-43ea-87dd-84be2e6df404', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'93776c17-712c-404d-9342-0846405ea407', N'38b1a52b-7b89-4736-b02d-db75dd2fb7cb', N'31c25298-72d0-46ae-bf9d-611198c46fd0')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'a2d42a0a-50bc-4a91-8a6b-b754c89a48e1', N'df1b8420-23db-4351-ad27-e4f15af9f524', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'ea01e65e-33d5-48b3-ac6a-81bb21d71740', N'b7374f94-887c-4c10-a37d-0a3259edde34', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'ec738c49-2e83-423d-a3be-5b38260570f3', N'976a6fc3-b40c-4e6c-8c33-1ef4d2ef48c7', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'f2244815-27a3-47f9-bc82-4a84f4e39a43', N'9817ab8c-abc5-46fb-a693-d1d7174e4994', N'31c25298-72d0-46ae-bf9d-611198c46fd0')
INSERT [dbo].[SYS_UserRole] ([ID], [UserID], [RoleID]) VALUES (N'fb45360c-d07c-463e-86be-ccda51d34228', N'8357379c-77d4-434a-a6e2-7e807986f658', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d')
/****** Object:  Table [dbo].[SYS_USERORGANIZATION]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_USERORGANIZATION](
	[ID] [varchar](36) NOT NULL,
	[UserID] [varchar](36) NULL,
	[OrganizationID] [varchar](36) NULL,
	[IsDefault] [int] NULL,
 CONSTRAINT [PK_SYS_USERORGANIZATION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'04eedb94-6a99-422b-8291-4d73b458a2f4', N'64e28c5f-472d-48f3-b6ba-c399f6b96e0c', N'13c22d35-85d8-42cf-b42b-17f64302a505', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'07c846b6-57ec-4b1e-bb78-ee2f7d169c39', N'c6882cdf-74e4-4ade-a467-6ee15d7af918', N'13c22d35-85d8-42cf-b42b-17f64302a505', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'2a8b1e47-22ec-453c-85b9-34aa638771fe', N'47f4c098-5997-43ea-87dd-84be2e6df404', NULL, 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'46a0bc28-418f-46df-83d3-6576137a4756', N'38b1a52b-7b89-4736-b02d-db75dd2fb7cb', NULL, 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'4aeab700-6987-4618-b2df-53e62dd05c0b', N'9817ab8c-abc5-46fb-a693-d1d7174e4994', N'13c22d35-85d8-42cf-b42b-17f64302a505', 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'4b7b7e1a-cfac-4e3d-845f-3dab45bb3895', N'fa1c2d7a-d4e1-4cbb-9dea-969ad9847932', NULL, 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'61641d57-77e2-47be-b220-e146f6f110e1', N'c6882cdf-74e4-4ade-a467-6ee15d7af918', NULL, 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'64d19e10-eaf3-4d73-b685-d3467f470a9e', N'ce95b290-7373-4180-b281-0d745c94cd46', NULL, 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'6b49dae9-6609-406b-8bd1-2a2b71256105', N'ce95b290-7373-4180-b281-0d745c94cd46', N'13c22d35-85d8-42cf-b42b-17f64302a505', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'84dee7be-1111-4690-a94e-d37c66fa545b', N'38b1a52b-7b89-4736-b02d-db75dd2fb7cb', N'13c22d35-85d8-42cf-b42b-17f64302a505', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'8a381f3d-8b60-4eb2-af70-ccf6e48cf107', N'47f4c098-5997-43ea-87dd-84be2e6df404', N'13c22d35-85d8-42cf-b42b-17f64302a505', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'9acd0e53-2798-40c0-9b40-7c5f6603fa36', N'df1b8420-23db-4351-ad27-e4f15af9f524', N'f77737bf-583a-4480-9070-25f1001f53c0', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'a4b4a9db-02c1-48d4-b160-5bc3f600cfc8', N'abcb4cb3-a949-4510-81d8-85340edb54a1', N'13c22d35-85d8-42cf-b42b-17f64302a505', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'aae2b23f-acc6-462d-8f67-c32238cd1354', N'abcb4cb3-a949-4510-81d8-85340edb54a1', NULL, 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'ade35a9c-62a1-435b-ada5-89650fc4d96a', N'9817ab8c-abc5-46fb-a693-d1d7174e4994', NULL, 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'c7dafa4e-9aa3-4bdb-ab65-bd12ca38b7e0', N'b7374f94-887c-4c10-a37d-0a3259edde34', N'f77737bf-583a-4480-9070-25f1001f53c0', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'c848ba85-3dd2-4e02-a605-2cca5c2124a6', N'64e28c5f-472d-48f3-b6ba-c399f6b96e0c', NULL, 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'd0bebf22-58a7-44f2-9a06-b664b6e289d4', N'8357379c-77d4-434a-a6e2-7e807986f658', N'f77737bf-583a-4480-9070-25f1001f53c0', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'e4d18478-015b-419c-9f6f-0a5c9c4edaba', N'976a6fc3-b40c-4e6c-8c33-1ef4d2ef48c7', N'f77737bf-583a-4480-9070-25f1001f53c0', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'e642eb08-0980-40ed-882c-dbb9bde55112', N'abcb4cb3-a949-4510-81d8-85340edb54a1', N'f77737bf-583a-4480-9070-25f1001f53c0', 0)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'ef08c9ce-652a-420e-a128-cdfd431580a9', N'3a4e53ad-2d9f-4f2d-9800-408e6a65f9d8', N'f77737bf-583a-4480-9070-25f1001f53c0', 1)
INSERT [dbo].[SYS_USERORGANIZATION] ([ID], [UserID], [OrganizationID], [IsDefault]) VALUES (N'fbfef7e8-9db1-40cb-a35d-a03db1f7948c', N'c2003658-7b53-4b40-808e-29def41820fa', N'f77737bf-583a-4480-9070-25f1001f53c0', 1)
/****** Object:  Table [dbo].[SYS_User]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_User](
	[ID] [varchar](36) NOT NULL,
	[UserLoginName] [varchar](100) NULL,
	[UserDisplayName] [varchar](100) NULL,
	[UserPassword] [varchar](100) NULL,
	[UserType] [int] NULL,
	[UserPhone] [varchar](100) NULL,
	[RecordStatus] [int] NULL,
	[Extend1] [varchar](1000) NULL,
	[Extend2] [varchar](1000) NULL,
	[Extend3] [varchar](1000) NULL,
	[Extend4] [varchar](1000) NULL,
	[Extend5] [varchar](1000) NULL,
 CONSTRAINT [PK_SYS_USER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SYS_User] ([ID], [UserLoginName], [UserDisplayName], [UserPassword], [UserType], [UserPhone], [RecordStatus], [Extend1], [Extend2], [Extend3], [Extend4], [Extend5]) VALUES (N'1', N'administrator', N'超级管理员', N'547cb7bfe4344e24d2ed751a40676bea', 0, NULL, 1, NULL, NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[SYS_Role]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_Role](
	[ID] [varchar](36) NOT NULL,
	[RoleName] [varchar](100) NULL,
	[RoleDesc] [varchar](500) NULL,
	[RecordStatus] [int] NULL,
	[Extend1] [varchar](1000) NULL,
	[Extend2] [varchar](1000) NULL,
	[Extend3] [varchar](1000) NULL,
	[Extend4] [varchar](1000) NULL,
	[Extend5] [varchar](1000) NULL,
 CONSTRAINT [PK_SYS_ROLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SYS_Organization]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_Organization](
	[ID] [varchar](36) NOT NULL,
	[OrganName] [varchar](100) NULL,
	[OrganDesc] [varchar](500) NULL,
	[OrganParentID] [varchar](36) NULL,
	[RecordStatus] [int] NULL,
	[OrganNO] [varchar](100) NULL,
	[LevelNO] [int] NULL,
	[OrganOrder] [int] NULL,
	[Extend1] [varchar](1000) NULL,
	[Extend2] [varchar](1000) NULL,
	[Extend3] [varchar](1000) NULL,
	[Extend4] [varchar](1000) NULL,
	[Extend5] [varchar](1000) NULL,
 CONSTRAINT [PK_SYS_ORGANIZATION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SYS_MenuRole]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_MenuRole](
	[ID] [varchar](36) NOT NULL,
	[RoleID] [varchar](36) NULL,
	[MenuID] [varchar](36) NULL,
 CONSTRAINT [PK_SYS_MENUROLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'0872ffd8-ec25-43b7-b4e3-622e1406e6d2', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'e57c3549-f9d6-41b2-b3e7-fe8c7334d21c')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'09eeaad3-e1db-4f61-bfce-9dc14a6344e2', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d', N'b43cb058-ba35-41ec-be4f-bc40b6cf51f9')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'12bdbd8a-47ab-4964-9ae4-eb6d826013ef', N'69f49610-5a8c-43c2-b5de-9d45a7a278f7', N'bdd2948a-fc48-4a9d-b1f0-a5ccb687ad69')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'134a5081-7883-4eb0-bae4-d7e0a366214a', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'b573be6a-8bac-43a0-a84b-97766556c7f3')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'1ae5607b-bd4a-4414-b810-f12bb0144a49', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'6266a61c-1ed0-4e32-84ed-dfa92eb2ab90')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'43bbd963-f98a-4f5d-aafa-1333a3726fe6', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'bdd2948a-fc48-4a9d-b1f0-a5ccb687ad69')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'45273471-0929-496b-bb7b-b15402960068', N'69f49610-5a8c-43c2-b5de-9d45a7a278f7', N'c96c845e-a675-4382-a803-8a1457323c51')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'6b0a91b8-6834-4dcb-8859-c4a7cad0e1a8', N'69f49610-5a8c-43c2-b5de-9d45a7a278f7', N'add80004-e39a-4d96-82c2-ade6a5516377')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'93a7d0a1-639b-4537-895c-234fa7fcb513', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d', N'590eb1fa-1eb3-4e7a-b193-b3e7ac6c1216')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'98657a40-adb6-400c-b197-fee482977047', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d', N'd5d8a475-fa3d-4c2d-b18f-96098e87b3a6')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'9fbb053c-ad48-4a56-bf65-654ae195d4c0', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d', N'827aa4d0-00c5-490c-85e6-4cbbe0f3e270')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'a141a09c-e18d-4718-adeb-0e14b012c4a8', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'00ba1499-4d76-426e-bdd2-5df57283c290')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'aa9e9d31-0d97-4a6e-8a15-b910c3907bf2', N'69f49610-5a8c-43c2-b5de-9d45a7a278f7', N'1973076a-feaa-4cd5-9786-e7a988c28f5d')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'ac7bee8e-532d-42c1-b24e-575b86b16d4a', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'827aa4d0-00c5-490c-85e6-4cbbe0f3e270')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'c0b86769-eefb-4c5e-bb1f-829a5271bd64', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'b43cb058-ba35-41ec-be4f-bc40b6cf51f9')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'c6b70313-e679-4315-a6ff-8406b1a61265', N'297fee6a-a750-4fbd-b8f6-1ca7c9e9716d', N'bdd2948a-fc48-4a9d-b1f0-a5ccb687ad69')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'cba3559c-6207-4466-bf52-a59b6f133c51', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'b3e5e4ac-ff12-4158-bb35-cee960939a8c')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'eaeca976-d85c-460c-bdc8-9639069d5c87', N'efb91d5d-d6f9-465e-aae7-10353e22c041', N'b3e5e4ac-ff12-4158-bb35-cee960939a8c')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'fb090449-e304-49fd-ab7f-fe9656395794', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'3b3e1760-57ee-4ec0-8f5e-4db36edb0a18')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'fbf2b51a-5734-43b5-8b1b-dab0de2277e3', N'69f49610-5a8c-43c2-b5de-9d45a7a278f7', N'e3de2ceb-e768-4ff1-b99e-dd35fe0a0bd2')
INSERT [dbo].[SYS_MenuRole] ([ID], [RoleID], [MenuID]) VALUES (N'fea4762e-bc45-4144-8c02-fde7374cca78', N'31c25298-72d0-46ae-bf9d-611198c46fd0', N'b65906ea-10e1-4c4c-a80e-a63eef49479a')
/****** Object:  Table [dbo].[SYS_Menu]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_Menu](
	[ID] [varchar](36) NOT NULL,
	[MenuName] [varchar](200) NULL,
	[MenuPath] [varchar](200) NULL,
	[MenuType] [int] NULL,
	[PerMenuID] [varchar](36) NULL,
	[RecordStatus] [int] NULL,
	[MenuCode] [varchar](200) NULL,
	[MenuLevel] [int] NULL,
	[MenuOrder] [int] NULL,
	[Controller] [varchar](50) NULL,
	[Action] [varchar](50) NULL,
	[MenuDesc] [varchar](500) NULL,
	[Extend2] [varchar](1000) NULL,
	[Extend1] [varchar](1000) NULL,
	[Extend3] [varchar](1000) NULL,
	[Extend4] [varchar](1000) NULL,
	[Extend5] [varchar](1000) NULL,
 CONSTRAINT [PK_SYS_MENU] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'0F30E16D-FB61-7012-7AEB-FEB0B2F45350', N'系统设置', N'/AppSetting/Setting', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001001', 2, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'10482024-63B6-4075-A94B-3D03ECCE1B3A', N'部门用户', N'/Organization/OrganizationUserList', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001008', 2, 8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'1566A6E5-A521-9C94-49D5-9F26A7B329C2', N'菜单管理', N'/Menu/List', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001004', 2, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', N'管理工具', N'', -1, N'0', 1, N'001', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'22582A5F-8E7F-4404-ADBE-C2234EE47CEA', N'数据字典', N'/Dictionary/List', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001007', 2, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'29DC17E0-DE55-7744-6A2A-C75AF4BAF1EE', N'角色管理', N'/Role/List', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001003', 2, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'5D834020-0633-5AAE-AD38-110E2F60A9FC', N'用户管理', N'/User/List', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001002', 2, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'E014E691-5FF9-3719-EA3B-7E90B0594DE2', N'缓存管理', N'/CacheManager/List', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001006', 2, 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SYS_Menu] ([ID], [MenuName], [MenuPath], [MenuType], [PerMenuID], [RecordStatus], [MenuCode], [MenuLevel], [MenuOrder], [Controller], [Action], [MenuDesc], [Extend2], [Extend1], [Extend3], [Extend4], [Extend5]) VALUES (N'F921B475-2DB9-E644-C7EC-CCA35A328717', N'部门管理', N'/Organization/List', -1, N'1C2CA296-BCB3-FC07-31D1-D6152D0236DB', 1, N'001005', 2, 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[SYS_Log]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_Log](
	[ID] [varchar](36) NULL,
	[LogType] [varchar](50) NULL,
	[LogTime] [datetime] NULL,
	[Module] [varchar](256) NULL,
	[ClassName] [varchar](256) NULL,
	[MethodName] [varchar](256) NULL,
	[OperaterId] [varchar](36) NULL,
	[OperaterName] [varchar](100) NULL,
	[Exception] [varchar](4000) NULL,
	[DataString] [varchar](4000) NULL,
	[Message] [varchar](500) NULL,
	[IPAddress] [varchar](128) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SYS_Dictionary]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_Dictionary](
	[ID] [varchar](36) NULL,
	[ParDictID] [varchar](36) NULL,
	[DictType] [varchar](500) NULL,
	[DictName] [varchar](500) NULL,
	[DictCode] [varchar](500) NULL,
	[DictDesc] [varchar](500) NULL,
	[DictOrder] [int] NULL,
	[LevelNO] [int] NULL,
	[IsCache] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SYS_AppRole]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_AppRole](
	[ID] [varchar](36) NOT NULL,
	[RoleID] [varchar](36) NULL,
	[AppID] [varchar](36) NULL,
 CONSTRAINT [PK_SYS_APPROLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SYS_AppRegister]    Script Date: 05/10/2016 09:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYS_AppRegister](
	[ID] [varchar](36) NOT NULL,
	[AppRegisterID] [varchar](50) NULL,
	[AppName] [varchar](50) NULL,
	[LoginVerifiedUrl] [varchar](500) NULL,
	[HomePageUrl] [varchar](500) NULL,
	[OrderNum] [int] NULL,
	[RecordStatus] [int] NULL,
 CONSTRAINT [PK_SYS_APPREGISTER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GUID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册系统号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'AppRegisterID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'AppName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统登录校验地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'LoginVerifiedUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首页地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'HomePageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'OrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态:1启用 0停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_AppRegister', @level2type=N'COLUMN',@level2name=N'RecordStatus'
GO
