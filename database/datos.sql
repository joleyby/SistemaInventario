USE SistemaInventario;
GO

INSERT INTO dbo.Categoria (Nombre, Descripcion)
VALUES
    (N'Electrónica',    N'Equipos y accesorios electrónicos'),
    (N'Papelería',      N'Artículos de oficina y escritura'),
    (N'Limpieza',       N'Productos de higiene y limpieza'),
    (N'Alimentos',      N'Productos comestibles no perecederos');
GO

INSERT INTO dbo.Suplidor (NombreEmpresa, NombreContacto, Telefono, Email, SitioWeb)
VALUES
    (N'TecnoDom S.R.L.',       N'Laura Peña',    N'809-555-0101', N'ventas@tecnodom.example',  N'https://tecnodom.example'),
    (N'Papelera Central',      N'Carlos Mejía',  N'809-555-0102', N'pedidos@papelera.example', NULL),
    (N'Distribuidora Limpia',  N'Ana Rosario',   N'809-555-0103', N'info@limpia.example',      NULL);
GO

INSERT INTO dbo.Producto (Nombre, Precio, CategoriaId, SuplidorId)
VALUES
    (N'Mouse inalámbrico',        850.00,  1, 1),
    (N'Teclado USB',              1200.00, 1, 1),
    (N'Cuaderno rayado 100 hojas',  95.50, 2, 2),
    (N'Bolígrafo azul caja x12',   180.00, 2, 2),
    (N'Detergente 1 galón',       320.75,  3, 3),
    (N'Desinfectante 500 ml',     145.00,  3, 3);
GO

INSERT INTO dbo.Foto (CategoriaId, NombreArchivo, Extension, Enlace, Tamaño)
VALUES
    (1, N'mouse.jpg',   N'.jpg', N'categorias/1/3f2a9c1e0d4b4a7f8e6d5c4b3a291801.jpg', 204800),
    (1, N'teclado.png', N'.png', N'categorias/1/9b8c7d6e5f4a3b2c1d0e9f8a7b6c5d4e.png', 317440),
    (2, N'cuaderno.jpg',N'.jpg', N'categorias/2/1a2b3c4d5e6f7a8b9c0d1e2f3a4b5c6d.jpg', 150016);
GO