# Sistema de Gestión de Inventario

## 1. Requisitos previos
1. Windows 11 de 64 bits. Las interfaces WinForms y WPF solo se ejecutan en Windows.
2. .NET SDK 10.0 o superior.

## 2. Instalar la base de datos
Ejecutar los scripts en este orden:
1. `database\schema.sql`: tablas, claves foráneas y restricciones.
2. `database\datos.sql`: 5 categorías, 5 suplidores y 23 productos de prueba.

Con `sqlcmd`:
```bat
sqlcmd -S localhost -E -i database\schema.sql
sqlcmd -S localhost -E -i database\datos.sql
```

## 3. Configurar la cadena de conexión
1. Por defecto se usa la autenticación de Windows:
   `Server=localhost;Database=SistemaInventario;Trusted_Connection=True;TrustServerCertificate=True;`
2. Si usas un usuario de SQL Server, define la variable de entorno antes de abrir la aplicación:
```bat
set SISTEMA_INVENTARIO_CN=Server=.\SQLEXPRESS;Database=SistemaInventario;User Id=usuario_app;Password=...;TrustServerCertificate=True;Encrypt=True;
```

## 4. Compilar y ejecutar
1. Desde Visual Studio: abrir `SistemaInventario.sln`, establecer `Presentacion` (o `PresentacionWpf`) como proyecto de inicio y presionar F5.
2. Desde consola:
```bat
dotnet restore
dotnet build -c Release
dotnet run --project Presentacion
```

## 5. Respetar la arquitectura por capas
1. estructura:
```
SistemaInventario.sln
├── Entidades        (Class Library)  Modelos de dominio, excepciones, Logger
├── AccesoDatos      (Class Library)  Singleton de conexión, IRepositorio<T>, repositorios
├── LogicaNegocio    (Class Library)  Servicios, validaciones, LINQ, genéricos, fábrica
├── Presentacion     (WinForms App)   Formularios y controles de usuario
└── PresentacionWpf  (WPF App)        Versión WPF con MVVM
```
2. Respetar las dependencias permitidas:
   - `Presentacion` y `PresentacionWpf` → `LogicaNegocio`, `Entidades`. Ninguna referencia `AccesoDatos`.
   - `LogicaNegocio` → `AccesoDatos`, `Entidades`.
   - `AccesoDatos` → `Entidades`.
3. Aplicar los patrones indicados:
   - Repository con `IRepositorio<T>` y `RepositorioBase<T>`.
   - Singleton en `ConexionSingleton` con `Lazy<T>` (thread-safe).
   - Inyección de dependencias por constructor en servicios y formularios. `FabricaServicios` arma el grafo en `LogicaNegocio`.
   - Template Method en `ControlCrudBase<T>` (WinForms) y `CrudViewModelBase<T>` (WPF).

## 6. Implementar funcionalidades
1. **Categorías**
   - Crear, editar, eliminar y listar.
   - Nombre único de 3 a 100 caracteres; descripción obligatoria de hasta 500 caracteres.
   - De 0 a 5 fotos por categoría (`.jpg`, `.jpeg`, `.png`, máximo 2 MB cada una).
   - Registrar fechas de creación y modificación.
2. **Suplidores**
   - Empresa única, contacto, teléfono dominicano (`809|829|849-XXX-XXXX`), correo válido y sitio web opcional (URL http/https).
3. **Productos**
   - Nombre, precio (> 0, máximo 2 decimales), categoría activa y suplidor activo.
   - Filtros combinables: texto parcial, categoría, suplidor y rango de precios.
4. **Borrado lógico y papelera**
   - Usar `Activo BIT` en todas las entidades.
   - Impedir eliminar una categoría o suplidor con productos activos.
   - Mostrar los eliminados en la pestaña Papelera y permitir restaurarlos. No restaurar un producto si su categoría o su suplidor sigue eliminado.
5. **Búsqueda en tiempo real**
   - Filtrar mientras se escribe, sin distinguir mayúsculas y minúsculas.
6. **Validaciones**
   - Implementar las reglas en `LogicaNegocio` (`Validador` y servicios).
   - Mostrar los errores de campo con `ErrorProvider` (WinForms) o `INotifyDataErrorInfo` (WPF). Los errores generales van en un mensaje.
7. **Manejo de errores**
   - Envolver cada llamada a la base de datos en `try/catch` sobre `SqlException` y convertirla en `AccesoDatosException`.
   - Escribir los errores en `errores.log`, junto al ejecutable, mediante `Logger`.

## 7. Implementar las consultas LINQ
1. Productos agrupados por categoría con conteo: `ReporteServicio.ProductosPorCategoria`.
2. Top 5 suplidores con más productos: `ReporteServicio.Top5Suplidores`.
3. Promedio de precios por categoría: `ReporteServicio.PromedioPreciosPorCategoria`.
4. Productos en un rango de precios filtrados por categoría: `ProductoServicio.Buscar`.
5. Búsqueda parcial ignorando mayúsculas y minúsculas: `Buscador.Filtrar` y `ProductoServicio.Buscar`.
6. Valor total del inventario (`Sum`): `ReporteServicio.ValorTotalInventario`.

## 8. Implementar los genéricos propios
1. `IRepositorio<T>` y `RepositorioBase<T>` con restricción `where T : EntidadBase`.
2. `Buscador.Filtrar<T>(...)`: filtrado parcial sobre cualquier colección y cualquier número de campos.
3. `ExportadorCsv.Exportar<T>(...)`: exporta cualquier colección a CSV usando reflexión.
4. `ControlCrudBase<T>`: control genérico reutilizado por Categorías, Suplidores, Productos y Usuarios.

## 9. Implementar las extensiones adicionales
1. Módulo de Reportes con selector de reporte, valor total del inventario y exportación a CSV compatible con Excel en español.
2. Papelera unificada que muestre categorías, suplidores y productos eliminados en una sola grilla, con filtro por tipo.
3. Sincronizar fotos: al quitar una foto en el formulario, hacer borrado lógico en la base de datos.
4. Combinar filtros de productos y limpiarlos con un botón.
5. Refrescar automáticamente cada pestaña al entrar.

## 10. Añadir capturas de pantalla
1. Colocar las capturas en `docs/capturas/`:
   - `docs/capturas/categorias.png`
   - `docs/capturas/productos.png`
   - `docs/capturas/papelera.png`
   - `docs/capturas/reportes.png`
   - `docs/capturas/usuarios.png`
   - `docs/capturas/bitacora.png`

## 11. Implementar la versión WPF con MVVM
1. `PresentacionWpf/` reutiliza `Entidades`, `AccesoDatos` (a través de `LogicaNegocio`) y `LogicaNegocio`.
2. Estructura:
```
PresentacionWpf/
├── App.xaml / App.xaml.cs        Composition root WPF
├── Views/                        MainWindow y Plantillas.xaml
├── ViewModels/                   Categorías, Suplidores, Productos, Reportes, MainViewModel
├── MVVM/                         ViewModelBase, RelayCommand, CrudViewModelBase<T>, IModulo
└── Servicios/                    IDialogService y su implementación WPF
```
3. Aplicar:
   - Binding bidireccional, con la vista sin lógica de negocio.
   - `RelayCommand` para Nuevo, Guardar, Eliminar, Restaurar, agregar, quitar y abrir fotos.
   - Validación con `INotifyDataErrorInfo` y `ValidatesOnNotifyDataErrors`.
   - `CrudViewModelBase<T>` genérico para los tres módulos.
   - `IDialogService` para abstraer diálogos (sin `MessageBox` ni `OpenFileDialog` en los ViewModels).
   - Navegación por pestañas ligada a `PestanaActiva`, con refresco al activar.
4. Ejecutar con `dotnet run --project PresentacionWpf` o estableciendo `PresentacionWpf` como proyecto de inicio. Requiere la misma base de datos.
5. Estado actual: la versión WPF no tiene inicio de sesión ni control de roles. Sus acciones se registran en la bitácora como usuario `sistema`.

## 12. Configurar el almacenamiento de fotos (archivo en disco + enlace en BD)
1. No guardar binarios en SQL Server.
2. Usar `AlmacenFotosArchivo` (AccesoDatos) para escribir el archivo con nombre único (GUID):
   `Fotos\categorias\<CategoriaId>\<guid>.jpg`.
3. Guardar en la tabla `Foto`: enlace relativo (`Enlace`, p. ej. `categorias/3/….jpg`), tamaño (`Tamaño`), nombre y extensión.
4. Al consultar, convertir el enlace en una URL absoluta (`file:///…`) en la capa de negocio. Abrir con doble clic (WinForms) o con el botón "Abrir foto" (WPF).
5. Configurar:
   - Carpeta por defecto: `Fotos\` junto al ejecutable. Cambiarla con `SISTEMA_INVENTARIO_FOTOS`.
   - Bloquear rutas fuera de la carpeta raíz (no permitir `..`).
   - Si falla el insert en la base de datos, eliminar el archivo recién escrito.
   - Al quitar una foto desde el formulario, hacer borrado lógico del registro y conservar el archivo.

## 13. Inicio de sesión, usuarios y roles
1. Usuario inicial: `admin` con contraseña `admin123`. Se crea solo si la tabla de usuarios está vacía. Cambiar esa contraseña en el primer inicio (Usuarios → Editar).
2. Roles:
   - **Administrador:** acceso completo, incluidos los módulos Usuarios y Bitácora.
   - **Operador:** categorías, suplidores, productos, papelera y reportes.
3. Registro de usuarios: solo un administrador puede crear usuarios (Usuarios → Nuevo). La contraseña debe tener al menos 8 caracteres, con letras y números.
4. Cambio de rol: se hace desde Usuarios → Editar. Reglas de seguridad:
   - Nadie puede cambiar su propio rol.
   - Nadie puede desactivar su propia cuenta.
   - Siempre debe quedar al menos un administrador activo.
5. Desactivar y reactivar: "Eliminar" en Usuarios desactiva la cuenta sin borrarla. Para reactivarla, editar el usuario y marcar **Cuenta activa**.
6. Seguridad de contraseñas: se guardan con PBKDF2-SHA256 y 210 000 iteraciones. No se guardan en texto plano ni se registran en la bitácora, tampoco en intentos fallidos.

## 14. Bitácora de actividad
1. Registra quién hizo qué y cuándo:
   - Inicios y cierres de sesión, e intentos fallidos (sin contraseña).
   - Altas, ediciones, eliminaciones y restauraciones de categorías, suplidores, productos y usuarios.
   - Cambios de rol y restablecimientos de contraseña.
2. Solo los administradores pueden consultarla, en la pestaña Bitácora. Tiene búsqueda en tiempo real.
3. La base de datos no permite modificar ni borrar registros (trigger `TR_Bitacora_SoloLectura`).

## 15. Solución de problemas
1. **"You must install or update .NET" al ejecutar:** instalar el runtime de .NET 10 que indica el proyecto.
2. **FileLoadException: "Una directiva de Control de aplicaciones bloqueó este archivo":** Smart App Control o una política de empresa bloquea ensamblados sin firma. Desactivarlo en un equipo de pruebas: Configuración → Privacidad y seguridad → Seguridad de Windows → Control de aplicaciones y del navegador.
3. **Error de conexión a la base de datos:** revisar `SISTEMA_INVENTARIO_CN` y comprobar que SQL Server acepte el tipo de autenticación usado.
4. **Errores en tiempo de ejecución:** revisar `errores.log`, en la carpeta del ejecutable.


