
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (16, 9)
GO
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (16, 10)
GO
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (21, 9)
GO
INSERT [dbo].[AspectRatio] ([Width], [Height]) VALUES (32, 9)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (60)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (70)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (75)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (100)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (120)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (144)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (155)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (165)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (170)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (240)
GO
INSERT [dbo].[Frequency] ([Name]) VALUES (280)
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A0')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A1')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A2')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A3')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'A4')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B0')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B1')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B2')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B3')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'B4')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C0')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C1')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C2')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C3')
GO
INSERT [dbo].[PaperSize] ([Name]) VALUES (N'C4')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'LCD')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'DLP')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'LCoS')
GO
INSERT [dbo].[ProjectorTechnology] ([Name]) VALUES (N'3LCD')
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1280, 720, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1152, 720, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1360, 768, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1366, 768, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1440, 900, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1600, 900, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1680, 1050, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1920, 1080, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (1920, 1200, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2048, 1152, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2560, 1440, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2560, 1600, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3200, 1800, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3440, 1440, 3)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3840, 2400, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3840, 2160, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (4128, 2322, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (5120, 2160, 3)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (5120, 2880, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (7680, 4320, 1)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (7680, 4800, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2560, 1080, 3)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3840, 1080, 4)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (2880, 1800, 2)
GO
INSERT [dbo].[Resolution] ([Width], [Height], [AspectRatioID]) VALUES (3072, 1920, 2)
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Напольный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'На раме')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Настенно-потолочный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Настенный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Настольный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'На штативе')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Портативный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Портативно-напольный')
GO
INSERT [dbo].[ScreenInstalled] ([Name]) VALUES (N'Потолочный')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'InteractiveWhiteboard')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'Monitor')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'NetworkSwitch')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'Notebook')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'OtherEquipment')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'PrinterScanner')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'Projector')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'ProjectorScreen')
GO
INSERT [dbo].[TableName] ([Name]) VALUES (N'PC')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'PC', N'Компьютер', N'Компьютеры')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'Notebook', N'Портативный компьютер', N'Портативные компьютеры')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'PrinterScanner', N'Принтер/Сканер', N'Принтеры/Сканеры')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'InteractiveWhiteboard', N'Интерактивное оборудование', N'Интерактивное оборудование')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'NetworkSwitch', N'Сетевое оборудование', N'Сетевое оборудование')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'Monitor', N'Монитор', N'Мониторы')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'Projector', N'Проектор', N'Проекторы')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'ProjectorScreen', N'Экран для проектора', N'Экраны для проекторов')
GO
INSERT [dbo].[TypeDevice] ([Name], [RussianName], [RussianNamePlural]) VALUES (N'OtherEquipment', N'Прочее оборудование', N'Прочее оборудование')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Коммутатор')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Межсетевой экран')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Маршрутизатор')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Wi-Fi Роутер')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Точка доступа Wi-Fi')
GO
INSERT [dbo].[TypeNetworkSwitch] ([Name]) VALUES (N'Усилитель Wi-Fi')
GO
INSERT [dbo].[TypeNotebook] ([Name]) VALUES (N'Ноутбук')
GO
INSERT [dbo].[TypeNotebook] ([Name]) VALUES (N'Моноблок')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Лазерное МФУ')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Струйное МФУ')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Лазерный принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Струйный принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Матричный принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Плоттер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'3D Принтер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'Сканер')
GO
INSERT [dbo].[TypePrinter] ([Name]) VALUES (N'3D Сканер')
GO
INSERT [dbo].[TypeSoftLicense] ([Name]) VALUES (N'Лицензия')
GO
INSERT [dbo].[TypeSoftLicense] ([Name]) VALUES (N'Подписка на месяц')
GO
INSERT [dbo].[TypeSoftLicense] ([Name]) VALUES (N'Подписка на год')
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DVI', 1)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DVI-D', 2)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DVI-I', 4)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'DisplayPort', 8)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'HDMI', 16)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'Mini DisplayPort', 32)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'Thunderbolt', 64)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'USB Type С', 128)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'VGA', 256)
GO
INSERT [dbo].[VideoConnector] ([Name], [Value]) VALUES (N'microHDMI', 512)
GO
INSERT [dbo].[WiFiFrequency] ([Name]) VALUES (N'2.4 ГГц')
GO
INSERT [dbo].[WiFiFrequency] ([Name]) VALUES (N'5 ГГц')
GO
INSERT [dbo].[WiFiFrequency] ([Name]) VALUES (N'2.4 ГГц/5 ГГц')
GO