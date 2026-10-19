IF DB_ID(N'SistemaInventario') IS NULL
    CREATE DATABASE SistemaInventario;
GO

USE SistemaInventario;
GO

/* ------------------------------------------------------------
   TABLAS
   ------------------------------------------------------------ */

IF OBJECT_ID(N'dbo.Categoria', N'U') IS NULL
CREATE TABLE dbo.Categoria
(
    Id                 INT            IDENTITY(1,1) NOT NULL,
    Nombre             NVARCHAR(100)  NOT NULL,
    Descripcion        NVARCHAR(255)  NOT NULL CONSTRAINT DF_Categoria_Descripcion DEFAULT (N''),
    Activo             BIT            NOT NULL CONSTRAINT DF_Categoria_Activo DEFAULT (1),
    FechaCreacion      DATETIME       NOT NULL CONSTRAINT DF_Categoria_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME       NOT NULL CONSTRAINT DF_Categoria_FechaModificacion DEFAULT (GETDATE()),
    CONSTRAINT PK_Categoria PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Categoria_Nombre UNIQUE (Nombre)
);
GO

IF OBJECT_ID(N'dbo.Suplidor', N'U') IS NULL
CREATE TABLE dbo.Suplidor
(
    Id                 INT            IDENTITY(1,1) NOT NULL,
    NombreEmpresa      NVARCHAR(150)  NOT NULL,
    NombreContacto     NVARCHAR(100)  NOT NULL CONSTRAINT DF_Suplidor_NombreContacto DEFAULT (N''),
    Telefono           NVARCHAR(30)   NOT NULL CONSTRAINT DF_Suplidor_Telefono DEFAULT (N''),
    Email              NVARCHAR(150)  NOT NULL CONSTRAINT DF_Suplidor_Email DEFAULT (N''),
    SitioWeb           NVARCHAR(255)  NULL,
    Activo             BIT            NOT NULL CONSTRAINT DF_Suplidor_Activo DEFAULT (1),
    FechaCreacion      DATETIME       NOT NULL CONSTRAINT DF_Suplidor_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME       NOT NULL CONSTRAINT DF_Suplidor_FechaModificacion DEFAULT (GETDATE()),
    CONSTRAINT PK_Suplidor PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Suplidor_NombreEmpresa UNIQUE (NombreEmpresa)
);
GO

IF OBJECT_ID(N'dbo.Producto', N'U') IS NULL
CREATE TABLE dbo.Producto
(
    Id                 INT            IDENTITY(1,1) NOT NULL,
    Nombre             NVARCHAR(150)  NOT NULL,
    Precio             DECIMAL(18,2)  NOT NULL,
    CategoriaId        INT            NOT NULL,
    SuplidorId         INT            NOT NULL,
    Activo             BIT            NOT NULL CONSTRAINT DF_Producto_Activo DEFAULT (1),
    FechaCreacion      DATETIME       NOT NULL CONSTRAINT DF_Producto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME       NOT NULL CONSTRAINT DF_Producto_FechaModificacion DEFAULT (GETDATE()),
    CONSTRAINT PK_Producto PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Producto_Precio CHECK (Precio >= 0),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (CategoriaId) REFERENCES dbo.Categoria (Id),
    CONSTRAINT FK_Producto_Suplidor FOREIGN KEY (SuplidorId) REFERENCES dbo.Suplidor (Id)
);
GO

IF OBJECT_ID(N'dbo.Foto', N'U') IS NULL
CREATE TABLE dbo.Foto
(
    Id                 INT            IDENTITY(1,1) NOT NULL,
    CategoriaId        INT            NOT NULL,
    NombreArchivo      NVARCHAR(255)  NOT NULL,
    Extension          NVARCHAR(10)   NOT NULL,
    Enlace             NVARCHAR(500)  NOT NULL,
    Tamaño             BIGINT         NOT NULL CONSTRAINT DF_Foto_Tamaño DEFAULT (0),
    Activo             BIT            NOT NULL CONSTRAINT DF_Foto_Activo DEFAULT (1),
    FechaCreacion      DATETIME       NOT NULL CONSTRAINT DF_Foto_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME       NOT NULL CONSTRAINT DF_Foto_FechaModificacion DEFAULT (GETDATE()),
    CONSTRAINT PK_Foto PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Foto_Tamaño CHECK (Tamaño >= 0),
    CONSTRAINT UQ_Foto_Enlace UNIQUE (Enlace),
    CONSTRAINT FK_Foto_Categoria FOREIGN KEY (CategoriaId) REFERENCES dbo.Categoria (Id)
);
GO

IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
CREATE TABLE dbo.Usuario
(
    Id                 INT            IDENTITY(1,1) NOT NULL,
    NombreUsuario      NVARCHAR(50)   NOT NULL,
    NombreCompleto     NVARCHAR(150)  NOT NULL,
    PasswordHash       NVARCHAR(255)  NOT NULL,
    Rol                NVARCHAR(20)   NOT NULL,
    Activo             BIT            NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT (1),
    FechaCreacion      DATETIME       NOT NULL CONSTRAINT DF_Usuario_FechaCreacion DEFAULT (GETDATE()),
    FechaModificacion  DATETIME       NOT NULL CONSTRAINT DF_Usuario_FechaModificacion DEFAULT (GETDATE()),
    CONSTRAINT PK_Usuario PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT CK_Usuario_Rol CHECK (Rol IN (N'Administrador', N'Operador'))
);
GO

IF OBJECT_ID(N'dbo.Bitacora', N'U') IS NULL
CREATE TABLE dbo.Bitacora
(
    Id             INT            IDENTITY(1,1) NOT NULL,
    FechaHora      DATETIME2(0)   NOT NULL CONSTRAINT DF_Bitacora_FechaHora DEFAULT (SYSDATETIME()),
    UsuarioId      INT            NULL,
    UsuarioNombre  NVARCHAR(50)   NOT NULL,
    Accion         NVARCHAR(50)   NOT NULL,
    Entidad        NVARCHAR(50)   NOT NULL,
    EntidadId      INT            NULL,
    Detalle        NVARCHAR(MAX)  NOT NULL CONSTRAINT DF_Bitacora_Detalle DEFAULT (N''),
    CONSTRAINT PK_Bitacora PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Bitacora_Usuario FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuario (Id) ON DELETE SET NULL
);
GO

/* ------------------------------------------------------------
   ÍNDICES
   ------------------------------------------------------------ */

/* 
CREATE NONCLUSTERED INDEX IX_Producto_CategoriaId ON dbo.Producto (CategoriaId) INCLUDE (Activo);
CREATE NONCLUSTERED INDEX IX_Producto_SuplidorId  ON dbo.Producto (SuplidorId)  INCLUDE (Activo);
GO

CREATE NONCLUSTERED INDEX IX_Foto_CategoriaId_Activo ON dbo.Foto (CategoriaId, Activo);
GO


CREATE NONCLUSTERED INDEX IX_Bitacora_FechaHora ON dbo.Bitacora (FechaHora DESC, Id DESC);
GO
*/