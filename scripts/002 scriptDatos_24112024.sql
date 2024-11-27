GO
USE GESTION_EVENTOS;
GO
USE GESTION_EVENTOS;
GO

-- Resetear el valor de Identity en la tabla USUARIOS_USU
DBCC CHECKIDENT ('USUARIOS_USU', RESEED, 1);
GO

-- Resetear el valor de Identity en la tabla GESTION_EVENTOS_EVE
DBCC CHECKIDENT ('GESTION_EVENTOS_EVE', RESEED, 1);
GO

-- Resetear el valor de Identity en la tabla INSCRIPCIONES_EVT
DBCC CHECKIDENT ('INSCRIPCIONES_EVT', RESEED, 1);
GO

-- Resetear el valor de Identity en la tabla HISTORIAL_MODIFICACION_EVE
DBCC CHECKIDENT ('HISTORIAL_MODIFICACION_EVE', RESEED, 1);
GO
-- Insertar datos en la tabla USUARIOS_USU
INSERT INTO USUARIOS_USU (Nombre_Usuario, Correo_Usuario,ClaveUsuario,CnameUsuario, Estado, UsuarioCreacion, UsuarioActualizacion)
VALUES 
    ('Carlos Perez', 'carlos.perez@example.com','user1','', 1, 1, NULL),
    ('Ana Gómez', 'ana.gomez@example.com', 'user2','',1, 1, NULL),
    ('Luis Martínez', 'luis.martinez@example.com','user3','', 1, 1, NULL),
    ('María Sánchez', 'maria.sanchez@example.com', 'user4','',1, 1, NULL),
    ('Javier López', 'javier.lopez@example.com','user5','', 0, 1, NULL);  -- Usuario inactivo
GO
-- Insertar datos en GESTION_EVENTOS_EVE
INSERT INTO GESTION_EVENTOS_EVE (Nombre, Descripcion, Fecha_Hora, Ubicacion, Capacidad_Maxima, Id_Usuario, Estado, UsuarioCreacion, UsuarioActualizacion)
VALUES 
    ('Conferencia de Tecnología', 'Evento sobre las últimas innovaciones en tecnología', '2024-12-10 09:00:00', 'Auditorio A, Universidad X', 300,4, 1, 1, NULL),  -- Evento activo
    ('Concierto de Rock', 'Concierto en vivo de varias bandas de rock', '2024-12-15 19:00:00', 'Estadio Olímpico', 5000, 3, 1, 1, NULL);  -- Evento activo
GO
-- Insertar datos en INSCRIPCIONES_EVT
INSERT INTO INSCRIPCIONES_EVT (Id_Evento, Id_Usuario, Fecha_Inscripcion, Estado, UsuarioCreacion, UsuarioActualizacion)
VALUES 
    (1, 3, '2024-12-05 08:30:00', 1, 1, NULL),  -- Carlos Perez se inscribe en la conferencia de tecnología
    (1, 4, '2024-12-05 09:00:00', 1, 1, NULL),  -- Ana Gómez se inscribe en la conferencia de tecnología
	 (2, 4, '2024-12-05 09:00:00', 1, 1, NULL);  -- Ana Gómez se inscribe en la conferencia de tecnología
GO
-- Insertar datos en HISTORIAL_MODIFICACION_EVE
INSERT INTO HISTORIAL_MODIFICACION_EVE (Id_Evento, Id_Usuario, Fecha_Modificacion, Descripcion_Modificacion, Estado, UsuarioCreacion, UsuarioActualizacion)
VALUES 
    (1, 1, '2024-12-05 10:00:00', 'Se actualizó la ubicación del evento', 1, 1, NULL),
    (2, 2, '2024-12-10 12:00:00', 'Se cambiaron los horarios del concierto', 1, 1, NULL);


	

