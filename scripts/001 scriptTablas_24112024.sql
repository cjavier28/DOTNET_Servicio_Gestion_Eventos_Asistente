USE master;
GO
DROP DATABASE IF EXISTS GESTION_EVENTOS;
DROP TABLE IF EXISTS USUARIOS_USU;
DROP TABLE IF EXISTS GESTION_EVENTOS_EVE;
DROP TABLE IF EXISTS INSCRIPCIONES_EVT;
DROP TABLE IF EXISTS HISTORIAL_MODIFICACION_EVE;
GO

CREATE DATABASE GESTION_EVENTOS;
GO
GO
USE GESTION_EVENTOS;
GO

-- Crear tabla de USUARIOS
CREATE TABLE USUARIOS_USU (
    Id_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Usuario VARCHAR(255) NOT NULL,
    Correo_Usuario VARCHAR(255) NOT NULL,
	CnameUsuario VARCHAR(255) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,  -- Nuevo campo Estado (1 = activo, 0 = inactivo)
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),  -- Fecha de creación
	ClaveUsuario varchar(max),
    FechaActualizacion DATETIME NULL,  -- Fecha de última actualización
    UsuarioCreacion INT NOT NULL,  -- Usuario que crea el registro
    UsuarioActualizacion INT NULL,  -- Usuario que actualiza el registro
    CONSTRAINT UQ_Correo_Usuario UNIQUE (Correo_Usuario)
);
GO

-- Crear tabla de GESTION_EVENTOS
CREATE TABLE GESTION_EVENTOS_EVE (
    Id_Evento INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion TEXT,
    Fecha_Hora DATETIME NOT NULL,
    Ubicacion VARCHAR(255) NOT NULL,
    Capacidad_Maxima INT NOT NULL,
    Id_Usuario INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,  -- Nuevo campo Estado (1 = activo, 0 = inactivo)
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),  -- Fecha de creación
    FechaActualizacion DATETIME NULL,  -- Fecha de última actualización
    UsuarioCreacion INT NOT NULL,  -- Usuario que crea el registro
    UsuarioActualizacion INT NULL,  -- Usuario que actualiza el registro
    CONSTRAINT FK_Usuario_Evento FOREIGN KEY (Id_Usuario) REFERENCES USUARIOS_USU(Id_Usuario)
);
GO

-- Crear tabla de INSCRIPCIONES
CREATE TABLE INSCRIPCIONES_EVT (
    Id_Inscripcion INT IDENTITY(1,1) PRIMARY KEY,
    Id_Evento INT NOT NULL,
    Id_Usuario INT NOT NULL,
    Fecha_Inscripcion DATETIME NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,  -- Nuevo campo Estado (1 = activo, 0 = inactivo)
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),  -- Fecha de creación
    FechaActualizacion DATETIME NULL,  -- Fecha de última actualización
    UsuarioCreacion INT NOT NULL,  -- Usuario que crea el registro
    UsuarioActualizacion INT NULL,  -- Usuario que actualiza el registro
    CONSTRAINT FK_Evento_Inscripcion FOREIGN KEY (Id_Evento) REFERENCES GESTION_EVENTOS_EVE(Id_Evento),
    CONSTRAINT FK_Usuario_Inscripcion FOREIGN KEY (Id_Usuario) REFERENCES USUARIOS_USU(Id_Usuario),
    CONSTRAINT UQ_Usuario_Evento UNIQUE (Id_Evento, Id_Usuario)
);
GO

-- Crear tabla de HISTORIAL_MODIFICACION
CREATE TABLE HISTORIAL_MODIFICACION_EVE (
    Id_Modificacion INT IDENTITY(1,1) PRIMARY KEY,
    Id_Evento INT NOT NULL,
    Id_Usuario INT NOT NULL,
    Fecha_Modificacion DATETIME NOT NULL,
    Descripcion_Modificacion TEXT,
    Estado BIT NOT NULL DEFAULT 1,  -- Nuevo campo Estado (1 = activo, 0 = inactivo)
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),  -- Fecha de creación
    FechaActualizacion DATETIME NULL,  -- Fecha de última actualización
    UsuarioCreacion INT NOT NULL,  -- Usuario que crea el registro
    UsuarioActualizacion INT NULL,  -- Usuario que actualiza el registro
    CONSTRAINT FK_Evento_Modificacion FOREIGN KEY (Id_Evento) REFERENCES GESTION_EVENTOS_EVE(Id_Evento),
    CONSTRAINT FK_Usuario_Modificacion FOREIGN KEY (Id_Usuario) REFERENCES USUARIOS_USU(Id_Usuario)
);
GO

-- Crear índices en la tabla INSCRIPCIONES
CREATE INDEX IDX_Evento_Inscripcion ON INSCRIPCIONES_EVT(Id_Evento);
CREATE INDEX IDX_Usuario_Inscripcion ON INSCRIPCIONES_EVT(Id_Usuario);
GO

-- Crear índices en la tabla HISTORIAL_MODIFICACION
CREATE INDEX IDX_Evento_Modificacion ON HISTORIAL_MODIFICACION_EVE(Id_Evento);
CREATE INDEX IDX_Usuario_Modificacion ON HISTORIAL_MODIFICACION_EVE(Id_Usuario);
GO
