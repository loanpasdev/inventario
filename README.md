# Mini Sistema de Gestión de Inventario (.NET 8)

## 1. Información General

| Campo | Valor |
|-------|-------|
| **Proyecto** | Mini Sistema de Gestión de Inventario |
| **Tipo de Solución** | Web API + Aplicación Web Razor Pages |
| **Framework** | .NET 8 |
| **Arquitectura** | Clean Architecture + CQRS |
| **Repositorio** | `https://gitlab.com/loanpasdev/inventario` |
| **Base de Datos** | SQL Server / SQL Server LocalDB |
| **Fecha** | Junio 2026 |

## 2. Descripción Funcional

La solución permite administrar un inventario de productos mediante una API protegida con JWT y una interfaz web construida con Razor Pages. El sistema cubre autenticación, registro de productos, consulta de stock bajo, visualización de dashboard, catálogos maestros y manejo de equivalencias monetarias en VES y USD.

Reglas funcionales principales:

- El acceso a los módulos protegidos requiere autenticación.
- El login de la interfaz web se realiza con **correo electrónico y contraseña**.
- La API acepta autenticación por usuario o correo.
- La base de datos se crea y actualiza por migraciones al iniciar la API.
- Se cargan datos semilla para categorías, unidades, productos, usuarios, roles y tasa de cambio.
- La tasa de cambio actual se maneja como un valor semilla de demostración.
- Para un entorno real, se recomienda integrar la tasa oficial diaria desde una API del **BCV** o, en su defecto, implementar un módulo de actualización manual diaria.

### 2.1 Casos de Uso

| Caso | Resultado |
|------|-----------|
| Login correcto | Acceso al dashboard y módulos protegidos |
| Login inválido | Mensaje de credenciales inválidas |
| Registro de producto exitoso | Producto agregado al inventario |
| Consulta de productos | Listado con filtros, estado, precios y código de barras |
| Consulta de stock bajo | Listado de productos críticos y notificaciones |
| Consulta de categorías | Visualización del catálogo de categorías |
| Consulta de unidades | Visualización del catálogo de unidades de medida |
| Consulta de monedas | Visualización del catálogo de monedas base |
| Dashboard | Gráficos y métricas generales del inventario |

### 2.2 Módulos Construidos

| Módulo | Descripción |
|--------|-------------|
| Login y autenticación | Pantalla de acceso, consumo de JWT y manejo de sesión |
| Dashboard | Resumen de productos, stock bajo, categorías y valor del inventario |
| Productos | Listado, alta de productos, validaciones, filtros y código de barras |
| Inventario | Consulta de productos con stock bajo |
| Categorías | Catálogo maestro de categorías |
| Unidades | Catálogo maestro de unidades de medida |
| Monedas | Catálogo maestro de monedas |
| Notificaciones | Campana con contador y acceso rápido a stock bajo |
| Tasa de cambio | Conversión referencial entre USD y VES |

## 3. Arquitectura

### 3.1 Diagrama de Flujo

```text
[Usuario]
   |
   v
[Razor Pages Web]
   |
   v
[HttpClient + Session + JWT]
   |
   v
[InventoryManagement.Api]
   |
   v
[Application - MediatR / CQRS]
   |
   +--> [Consultas Dapper]
   |
   +--> [Comandos EF Core]
   |
   v
[SQL Server]
```

### 3.2 Componentes Modificados/Creados

#### 3.2.1 Capa de Presentación

| Archivo | Tipo | Descripción |
|---------|------|-------------|
| `src/InventoryManagement.Web/Pages/Login.cshtml.cs` | Existente | Inicio de sesión por correo y contraseña |
| `src/InventoryManagement.Web/Pages/Index.cshtml.cs` | Existente | Dashboard y consumo de productos/indicadores |
| `src/InventoryManagement.Web/Pages/Productos/Index.cshtml.cs` | Existente | Listado y registro de productos |
| `src/InventoryManagement.Web/Pages/Inventario/Index.cshtml.cs` | Existente | Consulta de productos con stock bajo |
| `src/InventoryManagement.Web/Pages/Categorias/Index.cshtml.cs` | Existente | Catálogo de categorías |
| `src/InventoryManagement.Web/Pages/Unidades/Index.cshtml.cs` | Existente | Catálogo de unidades |
| `src/InventoryManagement.Web/Pages/Monedas/Index.cshtml.cs` | Existente | Catálogo de monedas |
| `src/InventoryManagement.Web/Pages/Shared/_Layout.cshtml` | Modificado | Navbar, notificaciones y estructura común |

#### 3.2.2 Capa de API

| Archivo | Tipo | Descripción |
|---------|------|-------------|
| `src/InventoryManagement.Api/Controllers/AuthController.cs` | Existente | Endpoint de autenticación |
| `src/InventoryManagement.Api/Controllers/ProductsController.cs` | Existente | Endpoints de productos, stock bajo y valor por categoría |
| `src/InventoryManagement.Api/Controllers/CategoriesController.cs` | Existente | Endpoint de categorías |
| `src/InventoryManagement.Api/Controllers/UnitsOfMeasureController.cs` | Existente | Endpoint de unidades |
| `src/InventoryManagement.Api/Controllers/CurrenciesController.cs` | Existente | Endpoint de monedas |
| `src/InventoryManagement.Api/Program.cs` | Existente | Configuración, JWT, Swagger y migraciones automáticas |

#### 3.2.3 Capa de Aplicación

| Archivo | Tipo | Descripción |
|---------|------|-------------|
| `src/InventoryManagement.Application/Products/Commands/CreateProduct/CreateProductCommandHandler.cs` | Existente | Registro de productos |
| `src/InventoryManagement.Application/Products/Queries/GetProducts/GetProductsQueryHandler.cs` | Existente | Consulta de productos |
| `src/InventoryManagement.Application/Products/Queries/GetLowStockProducts/GetLowStockProductsQueryHandler.cs` | Existente | Consulta de stock bajo |
| `src/InventoryManagement.Application/Products/Queries/GetInventoryValueByCategory/GetInventoryValueByCategoryQueryHandler.cs` | Existente | Indicadores por categoría |
| `src/InventoryManagement.Application/Categories/Queries/GetCategories/GetCategoriesQueryHandler.cs` | Existente | Consulta de categorías |
| `src/InventoryManagement.Application/UnitsOfMeasure/Queries/GetUnitsOfMeasure/GetUnitsOfMeasureQueryHandler.cs` | Existente | Consulta de unidades |
| `src/InventoryManagement.Application/Currencies/Queries/GetCurrencies/GetCurrenciesQueryHandler.cs` | Existente | Consulta de monedas |

#### 3.2.4 Capa de Infraestructura

| Archivo | Tipo | Descripción |
|---------|------|-------------|
| `src/InventoryManagement.Infrastructure/Persistence/ApplicationDbContext.cs` | Existente | Contexto EF Core |
| `src/InventoryManagement.Infrastructure/Persistence/Queries/ProductQueries.cs` | Existente | Consultas Dapper de productos |
| `src/InventoryManagement.Infrastructure/Persistence/Repositories/ProductCommandRepository.cs` | Existente | Persistencia de comandos de productos |
| `src/InventoryManagement.Infrastructure/Persistence/Repositories/UserAuthRepository.cs` | Existente | Autenticación por usuario o correo |
| `src/InventoryManagement.Infrastructure/Persistence/Seeding/DemoDataSeeder.cs` | Existente | Datos iniciales de prueba |
| `src/InventoryManagement.Infrastructure/Persistence/Configurations/UserConfiguration.cs` | Modificado | Usuarios semilla y correos por rol |
| `src/InventoryManagement.Infrastructure/Persistence/Migrations` | Existente | Migraciones de base de datos |

#### 3.2.5 Tests

| Archivo | Tipo | Descripción |
|---------|------|-------------|
| `tests/InventoryManagement.Application.Tests/Products/Commands/CreateProductCommandHandlerTests.cs` | Existente | Pruebas unitarias del registro de productos |

## 4. Especificación de Módulos y Endpoints

### 4.1 Endpoints Principales

```text
POST /api/auth/login
POST /api/products
GET  /api/products
GET  /api/products/low-stock
GET  /api/products/inventory-value-by-category
GET  /api/categories
GET  /api/units-of-measure
GET  /api/currencies
```

#### Headers Requeridos

| Header | Tipo | Obligatorio | Descripción |
|--------|------|-------------|-------------|
| Authorization | String | Si, excepto login | Bearer token JWT |
| Content-Type | String | Si en POST | `application/json` |

#### Request de Login

```json
{
  "username": "admin@mail.com",
  "password": "Admin123*"
}
```

#### Response de Login

```json
{
  "accessToken": "jwt-token",
  "tokenType": "Bearer",
  "username": "admin",
  "fullName": "System Administrator",
  "roles": ["Administrator"]
}
```

## 5. Modelo de Datos

### 5.1 Entidades Principales

| Entidad | Descripción |
|---------|-------------|
| `Products` | Productos del inventario con precios, moneda, stock y código de barras |
| `Categories` | Catálogo de categorías |
| `UnitsOfMeasure` | Catálogo de unidades |
| `Currencies` | Catalogo de monedas base |
| `ExchangeRates` | Tasa activa de conversión USD/VES |
| `Users` | Usuarios del sistema |
| `Roles` | Roles del sistema |
| `UserRoles` | Relacion usuario-rol |

### 5.2 Consideraciones sobre la Tasa de Cambio

| Campo | Valor |
|-------|-------|
| **Uso actual** | Conversion referencial entre VES y USD |
| **Origen actual** | Valor semilla de demostración |
| **Recomendación** | Integración con API del BCV |
| **Alternativa manual** | Módulo administrativo para actualizar la tasa diariamente |

## 6. Validaciones Implementadas

### 6.1 Login por correo en la Web

- La pantalla de acceso solicita correo electrónico válido y contraseña.
- Si la API no devuelve token, el acceso no se concede.

### 6.2 Registro de productos

- Campos requeridos para nombre, categoría, unidad, moneda, precios, stock y stock mínimo.
- Validaciones de cliente y servidor en español.
- El código de barras se representa visualmente en cada tarjeta de producto.

### 6.3 Autenticación y sesión

- Los módulos protegidos requieren token en sesión.
- Si la sesión expira, la aplicación redirige al login.

### 6.4 Base de datos y configuración

- La API aplica migraciones automáticamente.
- Si la cadena de conexión cambia, debe actualizarse en `src/InventoryManagement.Api/appsettings.json`.
- Si cambia la URL de la API, debe actualizarse `ApiSettings:BaseUrl` en `src/InventoryManagement.Web/appsettings.json`.

## 7. Precisión de Decimales

Los precios de compra y venta se manejan con `decimal`. La interfaz presenta equivalencias entre monedas usando la tasa activa disponible en base de datos.

## 8. Internacionalización (i18n)

No se implementó un sistema formal de i18n. Los mensajes de validación y la interfaz fueron ajustados en español para la experiencia del usuario.

## 9. Pruebas Unitarias

### 9.1 Archivo de Pruebas

- `tests/InventoryManagement.Application.Tests/Products/Commands/CreateProductCommandHandlerTests.cs`

### 9.2 Casos de Prueba

| Test | Descripción |
|------|-------------|
| `CreateProductCommandHandlerTests` | Valida el registro de productos en la capa de aplicación |

### 9.3 Ejecución

```bash
dotnet test .\tests\InventoryManagement.Application.Tests\InventoryManagement.Application.Tests.csproj
```

### 9.4 Resultado

Ejecución esperada: `Passed`.

## 10. Repositorio y Ejecución

### 10.1 Repositorio

- `https://gitlab.com/loanpasdev/inventario`

### 10.2 Clonar el Proyecto

```bash
git clone https://gitlab.com/loanpasdev/inventario.git
cd inventario
```

### 10.3 Crear o Recrear el Archivo `.sln`

```bash
dotnet new sln -n InventoryManagement
dotnet sln .\InventoryManagement.sln add .\src\InventoryManagement.Api\InventoryManagement.Api.csproj
dotnet sln .\InventoryManagement.sln add .\src\InventoryManagement.Web\InventoryManagement.Web.csproj
```

Si deseas incluir Application, Domain, Infrastructure y Tests:

```bash
dotnet sln .\InventoryManagement.sln add .\src\InventoryManagement.Application\InventoryManagement.Application.csproj
dotnet sln .\InventoryManagement.sln add .\src\InventoryManagement.Domain\InventoryManagement.Domain.csproj
dotnet sln .\InventoryManagement.sln add .\src\InventoryManagement.Infrastructure\InventoryManagement.Infrastructure.csproj
dotnet sln .\InventoryManagement.sln add .\tests\InventoryManagement.Application.Tests\InventoryManagement.Application.Tests.csproj
```

### 10.4 Configuración

#### Requisitos Previos

- .NET SDK 8
- SQL Server LocalDB o SQL Server
- Visual Studio 2022 o superior, o CLI de .NET

Opcional:

```bash
dotnet dev-certs https --trust
```

#### Cadena de Conexión

Archivo a modificar:

- `src/InventoryManagement.Api/appsettings.json`

Valor actual:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=inventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Ejemplo con SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=inventoryDB;User Id=TU_USUARIO;Password=TU_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

#### Comunicación Web -> API

Archivo a revisar:

- `src/InventoryManagement.Web/appsettings.json`

```json
"ApiSettings": {
  "BaseUrl": "https://localhost:7075/"
}
```

### 10.5 Ejecución de la Base de Datos y la Aplicación

Si eliminas la base de datos, basta con volver a ejecutar la API. Las migraciones y los datos semilla se aplicarán automáticamente.

#### Opción A: Visual Studio

> Este repositorio no incluye `.sln` por defecto. Si abres solo la carpeta y presionas `F5`, normalmente iniciará un solo proyecto.

Forma recomendada:

1. Abrir la carpeta del repositorio o crear la solución `.sln`.
2. Ejecutar primero la API.
3. Verificar Swagger en `https://localhost:7075/swagger`.
4. Ejecutar luego la Web.
5. Abrir la Web en `https://localhost:7255`.

Si trabajas con una solución `.sln`, puedes configurar **Multiple startup projects**:

- `InventoryManagement.Api` = Start
- `InventoryManagement.Web` = Start

#### Opción B: CLI

Primera terminal:

```bash
dotnet run --project .\src\InventoryManagement.Api\InventoryManagement.Api.csproj
```

Segunda terminal:

```bash
dotnet run --project .\src\InventoryManagement.Web\InventoryManagement.Web.csproj
```

### 10.6 URLs por Defecto

| Aplicacion | URL |
|------------|-----|
| API HTTPS | `https://localhost:7075/swagger` |
| API HTTP | `http://localhost:5129/swagger` |
| Web HTTPS | `https://localhost:7255` |
| Web HTTP | `http://localhost:5080` |

### 10.7 Credenciales Demo

| Campo | Valor |
|-------|-------|
| **Correo principal** | `admin@mail.com` |
| **Contraseña** | `Admin123*` |
| **Otros correos semilla** | `operator@mail.com`, `auditor@mail.com` |

## 11. Dependencias

| Dependencia | Uso |
|-------------|-----|
| ASP.NET Core | API y Web |
| Entity Framework Core | Persistencia y migraciones |
| Dapper | Consultas optimizadas |
| MediatR | CQRS |
| Serilog | Logging |
| JWT Bearer | Autenticación |
| Bootstrap | UI |
| jQuery Validation | Validaciones cliente |
| xUnit / Moq | Pruebas unitarias |

## 12. Configuración

No se requiere configuración adicional fuera de:

- cadena de conexión de la API
- URL base de la API consumida por la Web
- certificados HTTPS locales si aplica

## 13. Consideraciones de Despliegue

1. La base de datos debe permitir aplicar las migraciones de EF Core.
2. La API debe tener conectividad con SQL Server.
3. La Web debe apuntar a la URL correcta de la API.
4. En entorno real, la tasa de cambio no debería depender de un valor fijo semilla.
5. Se recomienda integrar la tasa desde BCV o implementar un módulo de actualización manual diaria.
6. Si se crea una solución `.sln`, debe mantenerse actualizada al agregar nuevos proyectos.
