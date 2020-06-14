USE [Accounting2]
;
SET IDENTITY_INSERT [dbo].[AspectRatio] ON 

INSERT [dbo].[AspectRatio] ([ID], [Width], [Height]) VALUES (1, 16, 9)
INSERT [dbo].[AspectRatio] ([ID], [Width], [Height]) VALUES (2, 16, 10)
INSERT [dbo].[AspectRatio] ([ID], [Width], [Height]) VALUES (3, 21, 9)
INSERT [dbo].[AspectRatio] ([ID], [Width], [Height]) VALUES (4, 32, 9)
SET IDENTITY_INSERT [dbo].[AspectRatio] OFF
;
SET IDENTITY_INSERT [dbo].[Frequency] ON 

INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (1, 60)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (2, 70)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (3, 75)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (4, 100)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (5, 120)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (6, 144)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (7, 155)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (8, 165)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (9, 170)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (10, 240)
INSERT [dbo].[Frequency] ([ID], [Name]) VALUES (11, 280)
SET IDENTITY_INSERT [dbo].[Frequency] OFF
;
SET IDENTITY_INSERT [dbo].[MatrixTechnology] ON 

INSERT [dbo].[MatrixTechnology] ([ID], [Name]) VALUES (1, N'IPS')
INSERT [dbo].[MatrixTechnology] ([ID], [Name]) VALUES (2, N'PLS')
INSERT [dbo].[MatrixTechnology] ([ID], [Name]) VALUES (3, N'TN')
INSERT [dbo].[MatrixTechnology] ([ID], [Name]) VALUES (4, N'VA')
SET IDENTITY_INSERT [dbo].[MatrixTechnology] OFF
;
SET IDENTITY_INSERT [dbo].[PaperSize] ON 

INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (5, N'A0')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (4, N'A1')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (3, N'A2')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (2, N'A3')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (1, N'A4')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (10, N'B0')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (9, N'B1')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (8, N'B2')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (7, N'B3')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (6, N'B4')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (15, N'C0')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (14, N'C1')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (13, N'C2')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (12, N'C3')
INSERT [dbo].[PaperSize] ([ID], [Name]) VALUES (11, N'C4')
SET IDENTITY_INSERT [dbo].[PaperSize] OFF
;
SET IDENTITY_INSERT [dbo].[ProjectorTechnology] ON 

INSERT [dbo].[ProjectorTechnology] ([ID], [Name]) VALUES (1, N'LCD')
INSERT [dbo].[ProjectorTechnology] ([ID], [Name]) VALUES (2, N'DLP')
INSERT [dbo].[ProjectorTechnology] ([ID], [Name]) VALUES (3, N'LCoS')
INSERT [dbo].[ProjectorTechnology] ([ID], [Name]) VALUES (4, N'3LCD')
SET IDENTITY_INSERT [dbo].[ProjectorTechnology] OFF
;
SET IDENTITY_INSERT [dbo].[Resolution] ON 

INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (1, 1280, 720, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (2, 1152, 720, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (3, 1360, 768, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (4, 1366, 768, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (6, 1440, 900, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (7, 1600, 900, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (8, 1680, 1050, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (9, 1920, 1080, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (10, 1920, 1200, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (11, 2048, 1152, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (12, 2560, 1440, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (13, 2560, 1600, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (14, 3200, 1800, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (15, 3440, 1440, 3)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (16, 3840, 2400, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (17, 3840, 2160, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (18, 4128, 2322, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (19, 5120, 2160, 3)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (20, 5120, 2880, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (21, 7680, 4320, 1)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (22, 7680, 4800, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (23, 2560, 1080, 3)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (24, 3840, 1080, 4)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (1023, 2880, 1800, 2)
INSERT [dbo].[Resolution] ([ID], [Width], [Height], [AspectRatioID]) VALUES (1024, 3072, 1920, 2)
SET IDENTITY_INSERT [dbo].[Resolution] OFF
;
SET IDENTITY_INSERT [dbo].[ScreenInstalled] ON 

INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (1, N'Напольный')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (2, N'На раме')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (3, N'Настенно-потолочный')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (4, N'Настенный')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (5, N'Настольный')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (6, N'На штативе')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (7, N'Портативный')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (8, N'Портативно-напольный')
INSERT [dbo].[ScreenInstalled] ([ID], [Name]) VALUES (9, N'Потолочный')
SET IDENTITY_INSERT [dbo].[ScreenInstalled] OFF
;
SET IDENTITY_INSERT [dbo].[TableName] ON 

INSERT [dbo].[TableName] ([ID], [Name]) VALUES (1, N'InteractiveWhiteboard')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (2, N'Monitor')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (3, N'NetworkSwitch')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (4, N'Notebook')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (5, N'OtherEquipment')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (6, N'PrinterScanner')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (7, N'Projector')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (8, N'ProjectorScreen')
INSERT [dbo].[TableName] ([ID], [Name]) VALUES (9, N'PC')
SET IDENTITY_INSERT [dbo].[TableName] OFF
;
SET IDENTITY_INSERT [dbo].[TypeDevice] ON 

INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (1, N'PC', N'Компьютер', N'Компьютеры')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (2, N'Notebook', N'Портативный компьютер', N'Портативные компьютеры')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (3, N'PrinterScanner', N'Принтер/Сканер', N'Принтеры/Сканеры')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (4, N'InteractiveWhiteboard', N'Интерактивная доска', N'Интерактивные доски')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (5, N'NetworkSwitch', N'Сетевое оборудование', N'Сетевое оборудование')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (6, N'Monitor', N'Монитор', N'Мониторы')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (7, N'Projector', N'Проектор', N'Проекторы')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (8, N'ProjectorScreen', N'Экран для проектора', N'Экраны для проекторов')
INSERT [dbo].[TypeDevice] ([ID], [Name], [RussianName], [RussianNamePlural]) VALUES (9, N'OtherEquipment', N'Прочее оборудование', N'Прочее оборудование')
SET IDENTITY_INSERT [dbo].[TypeDevice] OFF
;
SET IDENTITY_INSERT [dbo].[TypeNetworkSwitch] ON 

INSERT [dbo].[TypeNetworkSwitch] ([ID], [Name]) VALUES (1, N'Коммутатор')
INSERT [dbo].[TypeNetworkSwitch] ([ID], [Name]) VALUES (2, N'Межсетевой экран')
INSERT [dbo].[TypeNetworkSwitch] ([ID], [Name]) VALUES (3, N'Маршрутизатор')
INSERT [dbo].[TypeNetworkSwitch] ([ID], [Name]) VALUES (4, N'Wi-Fi Роутер')
INSERT [dbo].[TypeNetworkSwitch] ([ID], [Name]) VALUES (5, N'Точка доступа Wi-Fi')
INSERT [dbo].[TypeNetworkSwitch] ([ID], [Name]) VALUES (6, N'Усилитель Wi-Fi')
SET IDENTITY_INSERT [dbo].[TypeNetworkSwitch] OFF
;
SET IDENTITY_INSERT [dbo].[TypeNotebook] ON 

INSERT [dbo].[TypeNotebook] ([ID], [Name]) VALUES (1, N'Ноутбук')
INSERT [dbo].[TypeNotebook] ([ID], [Name]) VALUES (2, N'Моноблок')
SET IDENTITY_INSERT [dbo].[TypeNotebook] OFF
;
SET IDENTITY_INSERT [dbo].[TypePrinter] ON 

INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (7, N'3D Принтер')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (9, N'3D Сканер')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (1, N'Лазерное МФУ')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (3, N'Лазерный принтер')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (5, N'Матричный принтер')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (6, N'Плоттер')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (8, N'Сканер')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (2, N'Струйное МФУ')
INSERT [dbo].[TypePrinter] ([ID], [Name]) VALUES (4, N'Струйный принтер')
SET IDENTITY_INSERT [dbo].[TypePrinter] OFF
;
SET IDENTITY_INSERT [dbo].[TypeSoftLicense] ON 

INSERT [dbo].[TypeSoftLicense] ([ID], [Name]) VALUES (1, N'Лицензия')
INSERT [dbo].[TypeSoftLicense] ([ID], [Name]) VALUES (2, N'Подписка на месяц')
INSERT [dbo].[TypeSoftLicense] ([ID], [Name]) VALUES (3, N'Подписка на год')
SET IDENTITY_INSERT [dbo].[TypeSoftLicense] OFF
;
SET IDENTITY_INSERT [dbo].[VideoConnector] ON 

INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (1, N'DVI', 1)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (2, N'DVI-D', 2)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (3, N'DVI-I', 4)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (4, N'DisplayPort', 8)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (5, N'HDMI', 16)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (6, N'Mini DisplayPort', 32)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (7, N'Thunderbolt', 64)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (8, N'USB Type С', 128)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (9, N'VGA', 256)
INSERT [dbo].[VideoConnector] ([ID], [Name], [Value]) VALUES (10, N'microHDMI', 512)
SET IDENTITY_INSERT [dbo].[VideoConnector] OFF
;
SET IDENTITY_INSERT [dbo].[WiFiFrequency] ON 

INSERT [dbo].[WiFiFrequency] ([ID], [Name]) VALUES (1, N'2.4 ГГц')
INSERT [dbo].[WiFiFrequency] ([ID], [Name]) VALUES (2, N'5 ГГц')
INSERT [dbo].[WiFiFrequency] ([ID], [Name]) VALUES (3, N'2.4 ГГц/5 ГГц')
SET IDENTITY_INSERT [dbo].[WiFiFrequency] OFF
;
