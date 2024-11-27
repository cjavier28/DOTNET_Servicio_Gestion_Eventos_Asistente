GO
USE GESTION_EVENTOS;
GO
-- Procedimiento para Crear un Evento
CREATE PROCEDURE paCrearEvento
    @Nombre VARCHAR(255),
    @Descripcion TEXT,
    @Fecha_Hora DATETIME,
    @Ubicacion VARCHAR(255),
    @Capacidad_Maxima INT,
    @Id_Usuario INT,
    @Id_Evento INT OUTPUT  -- Parámetro de salida para devolver el Id_Evento
AS
BEGIN
    /* 
    Creado Por: Carlos Javier Marentes
    Fecha: 24/11/2024
    Descripción: Este procedimiento almacena un nuevo evento en la base de datos. Recibe como parámetros los detalles del evento (nombre, descripción, fecha, ubicación, capacidad máxima y el usuario que lo crea) y devuelve el Id_Evento generado automáticamente.
    */
    
    SET NOCOUNT ON;

    -- Insertar el nuevo evento en la tabla
    INSERT INTO GESTION_EVENTOS_EVE (Nombre, Descripcion, Fecha_Hora, Ubicacion, Capacidad_Maxima, Id_Usuario, Estado, FechaCreacion, UsuarioCreacion)
    VALUES (@Nombre, @Descripcion, @Fecha_Hora, @Ubicacion, @Capacidad_Maxima, @Id_Usuario, 1, GETDATE(), @Id_Usuario);

    -- Obtener el Id del evento recién insertado
    SET @Id_Evento = SCOPE_IDENTITY();
END
GO

-- Procedimiento para Editar un Evento
CREATE or alter PROCEDURE paEditarEvento
    @Id_Evento INT,
    @Id_Usuario INT,
    @Fecha_Hora DATETIME,
    @Ubicacion VARCHAR(255),
    @Capacidad_Maxima INT,
    @Id_Evento_Salida INT OUTPUT  -- Parámetro de salida para devolver el Id_Evento
AS
BEGIN
    /* 
    Creado Por: Carlos Javier Marentes
    Fecha: 24/11/2024
    Descripción: Este procedimiento permite editar los detalles de un evento creado por un usuario. Solo se puede modificar la capacidad máxima, la fecha/hora y la ubicación. Devuelve el Id_Evento del evento editado.
    */
    
    SET NOCOUNT ON;

    -- Verificar si el usuario es el creador del evento
    --IF EXISTS (SELECT 1 FROM GESTION_EVENTOS_EVE WHERE Id_Evento = @Id_Evento AND Id_Usuario = @Id_Usuario)
    --BEGIN
        -- Actualizar el evento si el usuario es el creador
        UPDATE GESTION_EVENTOS_EVE
        SET Fecha_Hora = @Fecha_Hora,
            Ubicacion = @Ubicacion,
            Capacidad_Maxima = @Capacidad_Maxima,
            FechaActualizacion = GETDATE(),
            UsuarioActualizacion = @Id_Usuario
        WHERE Id_Evento = @Id_Evento;

        -- Devolver el Id del evento actualizado
        SET @Id_Evento_Salida = @Id_Evento;
   -- END
   -- ELSE
  --  BEGIN
      --  SET @Id_Evento_Salida = NULL;
   -- --END
END
GO

-- Procedimiento para Eliminar un Evento
CREATE OR ALTER PROCEDURE  paEliminarEvento
    @Id_Evento INT,
    @Id_Usuario INT=0
AS
BEGIN
    /* 
    Creado Por: Carlos Javier Marentes
    Fecha: 24/11/2024
    Descripción: Este procedimiento elimina un evento solo si no tiene inscritos. Si el evento tiene inscripciones, no se podrá eliminar. El procedimiento también verifica que el usuario que solicita la eliminación sea el creador del evento.
    */
    
    SET NOCOUNT ON;

    -- Verificar si el usuario es el creador del evento
    
    BEGIN
        -- Verificar si el evento tiene inscripciones
        IF NOT EXISTS (SELECT 1 FROM INSCRIPCIONES_EVT WHERE Id_Evento = @Id_Evento)
        BEGIN
            -- Eliminar el evento si no tiene inscripciones
            DELETE FROM GESTION_EVENTOS_EVE WHERE Id_Evento = @Id_Evento;
        END
    END
END
GO

-- Procedimiento para Inscribir un Usuario en un Evento
CREATE PROCEDURE paInscribirEvento
    @Id_Evento INT,
    @Id_Usuario INT,
    @Id_Inscripcion INT OUTPUT  -- Parámetro de salida para devolver el Id_Inscripcion
AS
BEGIN
    /* 
    Creado Por: Carlos Javier Marentes
    Fecha: 24/11/2024
    Descripción: Este procedimiento inscribe a un usuario en un evento. Se verifica que el usuario no haya creado el evento y que no haya alcanzado el límite de eventos (máximo 3). También se verifica que la capacidad máxima del evento no se haya alcanzado. Devuelve el Id_Inscripcion generado automáticamente.
    */
    
    SET NOCOUNT ON;

    -- Verificar si el usuario ya está inscrito en 3 eventos
    DECLARE @EventosInscritos INT;
    SELECT @EventosInscritos = COUNT(*) 
    FROM INSCRIPCIONES_EVT 
    WHERE Id_Usuario = @Id_Usuario;

    IF @EventosInscritos < 3
    BEGIN
        -- Verificar que el usuario no haya creado el evento
        IF NOT EXISTS (SELECT 1 FROM GESTION_EVENTOS_EVE WHERE Id_Evento = @Id_Evento AND Id_Usuario = @Id_Usuario)
        BEGIN
            -- Verificar si la capacidad del evento no está llena
            IF (SELECT COUNT(*) FROM INSCRIPCIONES_EVT WHERE Id_Evento = @Id_Evento) < (SELECT Capacidad_Maxima FROM GESTION_EVENTOS_EVE WHERE Id_Evento = @Id_Evento)
            BEGIN
                -- Insertar inscripción
                INSERT INTO INSCRIPCIONES_EVT (Id_Evento, Id_Usuario, Fecha_Inscripcion, Estado, FechaCreacion, UsuarioCreacion)
                VALUES (@Id_Evento, @Id_Usuario, GETDATE(), 1, GETDATE(), @Id_Usuario);

                -- Obtener el Id de la inscripción
                SET @Id_Inscripcion = SCOPE_IDENTITY();
            END
            ELSE
            BEGIN
                SET @Id_Inscripcion = NULL;  -- Si la capacidad está llena, no inscribir al usuario
            END
        END
        ELSE
        BEGIN
            SET @Id_Inscripcion = NULL;  -- El usuario no puede inscribirse a su propio evento
        END
    END
    ELSE
    BEGIN
        SET @Id_Inscripcion = NULL;  -- El usuario ya está inscrito en 3 eventos
    END
END
GO
CREATE OR ALTER PROCEDURE ObtenerUsuariosConEventos
   @IdUsuario INT
AS
BEGIN
    /* 
		Creado Por: Carlos Javier Marentes
		Fecha: 24/11/2024
		Descripción: Este procedimiento muestra los detalles de cada evento
	*/
    SELECT        
        EVE.Id_Evento AS IDEVENTO,
        EVE.Nombre AS NOMBRE_EVENTO,
       ISNULL(EVE.Descripcion,'')AS DESCRIPCION,
        EVE.Fecha_Hora AS FECHAHORA,
        UPPER(ISNULL(EVE.Ubicacion,'')) AS UBICACION,
        EVE.Capacidad_Maxima AS CAPACIDADMAXIMA,
        EVE.Estado AS ESTADOEVENTO,
		ISNULL(AGR.TOTAL,0) AS TOTAL,
		EVE.Id_Usuario  AS IDUSUARIO,
		CASE 
			WHEN TAP.Id_Evento > 0 THEN 1
			ELSE 0
		END AS VALIDACION
    FROM 
        GESTION_EVENTOS_EVE EVE  
		LEFT JOIN (SELECT EVE.Id_Evento, COUNT(EVE.Id_Usuario) TOTAL FROM INSCRIPCIONES_EVT EVE GROUP BY EVE.Id_Evento) AGR 
			ON  EVE.Id_Evento = AGR.Id_Evento 
		LEFT JOIN (SELECT Id_Evento FROM INSCRIPCIONES_EVT WHERE Id_Usuario =@IdUsuario) TAP 
			ON TAP.Id_Evento=EVE.Id_Evento
		

END;
GO

GO
CREATE OR ALTER PROCEDURE  paInsertarUsuario
    @Nombre_Usuario VARCHAR(255),
    @Correo_Usuario VARCHAR(255),
    @CnameUsuario VARCHAR(255),
    @ClaveUsuario VARCHAR(MAX),
    @Estado BIT = 1,  -- Por defecto activo
    @UsuarioCreacion INT=1,
    @UsuarioActualizacion INT = NULL , -- Por defecto NULL si no se actualiza
	 @Id_Usuario INT OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;
	    /* 
    Creado Por: Carlos Javier Marentes
    Fecha: 24/11/2024
    Descripción: Este procedimiento inscribe a un usuario en un evento. Se verifica que el usuario no haya creado el evento y que no haya alcanzado el límite de eventos (máximo 3). También se verifica que la capacidad máxima del evento no se haya alcanzado. Devuelve el Id_Inscripcion generado automáticamente.
    */
    -- Verificar si el correo ya existe
    IF EXISTS (SELECT 1 FROM USUARIOS_USU WHERE Correo_Usuario = @Correo_Usuario)
    BEGIN
        -- Si el correo ya existe, devolver 0
        SELECT 0 AS Resultado;
        RETURN;
    END

    -- Insertar los datos en la tabla USUARIOS_USU
    INSERT INTO USUARIOS_USU 
    (
        Nombre_Usuario, 
        Correo_Usuario, 
        CnameUsuario, 
        Estado, 
        ClaveUsuario, 
        UsuarioCreacion, 
        UsuarioActualizacion,
		FechaCreacion
    )
    VALUES
    (
        @Nombre_Usuario, 
        @Correo_Usuario, 
        @CnameUsuario, 
        @Estado, 
        @ClaveUsuario, 
        @UsuarioCreacion, 
        @UsuarioActualizacion,
		GETDATE()

    );

    -- Devolver el ID generado por SCOPE_IDENTITY() si la inserción fue exitosa
        SET @Id_Usuario = SCOPE_IDENTITY();
END;
GO
CREATE OR ALTER PROCEDURE paValidarUsuarioRegistrado    
    @CnameUsuario VARCHAR(255),
    @ClaveUsuario VARCHAR(MAX)   
AS
BEGIN

    /* 
    Creado Por: Carlos Javier Marentes
    Fecha: 24/11/2024
    Descripción: Este procedimiento inscribe a un usuario en un evento. Se verifica que el usuario no haya creado el evento y que no haya alcanzado el límite de eventos (máximo 3). También se verifica que la capacidad máxima del evento no se haya alcanzado. Devuelve el Id_Inscripcion generado automáticamente.
    */
    SET NOCOUNT ON;

    -- Verificar si EXISTE
    IF EXISTS (SELECT 1 FROM USUARIOS_USU WHERE Correo_Usuario = @CnameUsuario and ClaveUsuario =@ClaveUsuario)
    BEGIN
        -- Si el correo ya existe, devolver 0
       SELECT Id_Usuario FROM USUARIOS_USU WHERE Correo_Usuario = @CnameUsuario and ClaveUsuario =@ClaveUsuario
        RETURN;
    END
	ELSE
	BEGIN 
	  SELECT 0 AS RESULTADO;
        RETURN;
	END
END;
GO

