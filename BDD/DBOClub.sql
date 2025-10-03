-- Crear base de datos (opcional)
CREATE DATABASE ClubesDB;
GO
USE ClubesDB;
GO

-- Tabla Club
CREATE TABLE Club (
    ClubId              INT IDENTITY(1,1) PRIMARY KEY,
    Nombre              NVARCHAR(100) NOT NULL,
    CantidadSocios      INT NOT NULL CONSTRAINT DF_Club_CantSocios DEFAULT (0),
    CantidadTitulos     INT NOT NULL CONSTRAINT DF_Club_CantTit DEFAULT (0),
    FechaFundacion      DATE NOT NULL,
    UbicacionEstadio    NVARCHAR(150) NULL,
    NombreEstadio       NVARCHAR(120) NULL,
	Activo				BIT NOT NULL DEFAULT (1),
    CONSTRAINT CK_Club_Cantidades_NoNeg CHECK (CantidadSocios >= 0 AND CantidadTitulos >= 0),
    CONSTRAINT CK_Club_FechaFundacion CHECK (FechaFundacion <= CAST(GETDATE() AS DATE))
);
GO

CREATE UNIQUE INDEX UX_Club_Nombre ON Club (Nombre);
GO

-- Tabla Dirigente
CREATE TABLE Dirigente (
    DirigenteId         INT IDENTITY(1,1) PRIMARY KEY,
    ClubId              INT NOT NULL,
    Nombre              NVARCHAR(80) NOT NULL,
    Apellido            NVARCHAR(80) NOT NULL,
    FechaNacimiento     DATE NOT NULL,
    Rol                 NVARCHAR(80) NOT NULL,
    Dni                 INT NOT NULL,
	Activo				BIT NOT NULL DEFAULT (1),
    CONSTRAINT FK_Dirigente_Club FOREIGN KEY (ClubId) REFERENCES Club (ClubId),
    CONSTRAINT UX_Dirigente_Dni UNIQUE (Dni)
);
GO

CREATE INDEX IX_Dirigente_ClubId ON Dirigente (ClubId);
GO

-- Tabla Socio
CREATE TABLE Socio (
    SocioId             INT IDENTITY(1,1) PRIMARY KEY,
    ClubId              INT NOT NULL,
    Nombre              NVARCHAR(80) NOT NULL,
    Apellido            NVARCHAR(80) NOT NULL,
    FechaNacimiento     DATE NOT NULL,
    FechaAsociado       DATE NOT NULL,
    Dni                 INT NOT NULL,
    CantidadAsistencias INT NOT NULL CONSTRAINT DF_Socio_Asist DEFAULT (0),
	Activo				BIT NOT NULL DEFAULT (1),
    CONSTRAINT FK_Socio_Club FOREIGN KEY (ClubId) REFERENCES Club (ClubId),
    CONSTRAINT UX_Socio_Dni UNIQUE (Dni),
    CONSTRAINT CK_Socio_Asist_NoNeg CHECK (CantidadAsistencias >= 0),
    CONSTRAINT CK_Socio_Fechas CHECK (FechaAsociado >= FechaNacimiento)
);
GO

CREATE INDEX IX_Socio_ClubId ON Socio (ClubId);
GO

-- Tabla Usuario
CREATE TABLE Usuario (
    UsuarioId           INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioNombre       NVARCHAR(100) NOT NULL,
    ContraseñaHash      VARBINARY(64) NOT NULL,
    Salt                VARBINARY(16) NOT NULL,
    Rol				    NVARCHAR(25) NOT NULL DEFAULT ('Usuario'),
	Activo				BIT NOT NULL DEFAULT (1),
);
GO

CREATE UNIQUE INDEX UX_Usuario_UsuarioNombre ON Usuario (UsuarioNombre);
CREATE INDEX IX_Usuario_UsuarioId ON Usuario (UsuarioId);
GO

-- Datos de prueba (opcionales)
INSERT INTO Club (Nombre, CantidadSocios, CantidadTitulos, FechaFundacion, UbicacionEstadio, NombreEstadio, Activo)
VALUES
('Club Atlético Ejemplo', 1200, 5, '1980-03-15', 'Av. Siempre Viva 123, CABA', 'Estadio Central', 1);

INSERT INTO Dirigente (ClubId, Nombre, Apellido, FechaNacimiento, Rol, Dni, Activo)
VALUES
(1, 'Carla', 'Pérez', '1985-07-01', 'Presidenta', 30123456, 1);

INSERT INTO Socio (ClubId, Nombre, Apellido, FechaNacimiento, FechaAsociado, Dni, CantidadAsistencias, Activo)
VALUES
(1, 'Juan', 'González', '2000-11-20', '2021-03-10', 40999888, 12, 1);

-- Contraseña: !Admin123 (Para el algoritmo utilizado, chequear BDLocal -> Clubes -> UsuarioLocal.cs["Ayuda Precarga"])
INSERT INTO Usuario (UsuarioNombre, ContraseñaHash, Salt, Rol, Activo) 
VALUES 
('admin', 0xF6FD180C54971DA3B963BAB78C40D3707109C5A7, 0x9AD6430BD033E8B0785D250C68EE184F, 'Admin', 1)
