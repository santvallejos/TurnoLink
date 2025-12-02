# TurnoLink API - Backend .NET 9

API REST desarrollada con .NET 9 para la gestión de reservas y turnos. Implementa una arquitectura de 3 capas con PostgreSQL.

## 🏗️ Arquitectura

El proyecto sigue una arquitectura de 3 capas:

```
api/
├── TurnoLink.WebAPI/         # Web API Layer - Controllers, Middleware
├── TurnoLink.Business/       # Business Layer - Services, DTOs, Lógica de Negocio
└── TurnoLink.DataAccess/     # Data Access Layer - Repositories, DbContext, Entidades
```

### Capas del Proyecto

#### 1. **TurnoLink.WebAPI** - Capa de Presentación
- Controllers para endpoints REST
- Configuración de Swagger/OpenAPI
- Middleware y configuración de la aplicación
- Inyección de dependencias

#### 2. **TurnoLink.Business** - Capa de Negocio
- Servicios con lógica de negocio
- DTOs (Data Transfer Objects)
- Interfaces de servicios
- Validaciones de negocio

#### 3. **TurnoLink.DataAccess** - Capa de Acceso a Datos
- Entity Framework Core DbContext
- Entidades del modelo de datos
- Repositorios (Repository Pattern)
- Unit of Work Pattern
- Migraciones de base de datos

## 📊 Modelo de Datos

### Entidades Principales

- **User**: Usuarios profesionales que ofrecen servicios
- **Client**: Clientes que realizan reservas
- **Service**: Servicios ofrecidos por profesionales
- **Booking**: Reservas/citas
- **Availability**: Disponibilidad horaria de profesionales
- **Notification**: Notificaciones enviadas (Email, WhatsApp, SMS)

## 🚀 Comenzando

### Requisitos Previos

- .NET 9 SDK
- PostgreSQL 13 o superior
- Visual Studio 2022 / VS Code / Rider

### Instalación

1. **Clonar el repositorio**
```powershell
cd TurnoLink\api
```

2. **Restaurar paquetes NuGet**
```powershell
dotnet restore
```

3. **Configurar la base de datos**

Editar `appsettings.Development.json` con tu configuración de PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=turnolink_dev;Username=tu_usuario;Password=tu_password"
  }
}
```

4. **Crear la base de datos**
```powershell
# Crear/actualizar la base de datos con las migraciones
dotnet ef database update --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
```

5. **Ejecutar la aplicación**
```powershell
dotnet run --project TurnoLink.WebAPI
```

La API estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger` (solo en Development)

## 🛠️ Comandos Útiles

### Entity Framework Core

```powershell
# Crear una nueva migración
dotnet ef migrations add NombreMigracion --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI

# Actualizar la base de datos
dotnet ef database update --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI

# Revertir última migración
dotnet ef migrations remove --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI

# Ver migraciones aplicadas
dotnet ef migrations list --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
```

### Compilación y Ejecución

```powershell
# Compilar toda la solución
dotnet build

# Ejecutar en modo desarrollo
dotnet run --project TurnoLink.WebAPI

# Ejecutar con hot reload
dotnet watch --project TurnoLink.WebAPI

# Compilar para producción
dotnet publish TurnoLink.WebAPI -c Release -o ./publish
```

## 📡 Endpoints Principales

### Users (Usuarios)
- `GET /api/users` - Obtener todos los usuarios
- `GET /api/users/active` - Obtener usuarios activos
- `GET /api/users/{id}` - Obtener usuario por ID
- `GET /api/users/email/{email}` - Obtener usuario por email
- `POST /api/users` - Crear nuevo usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

### Health Check
- `GET /health` - Verificar estado de la API

## 🔐 Seguridad

- Contraseñas hasheadas con BCrypt
- Configuración CORS para desarrollo
- Validación de datos en DTOs
- **TODO**: Implementar autenticación JWT
- **TODO**: Implementar autorización basada en roles

## 📦 Paquetes NuGet Principales

### TurnoLink.WebAPI
- `Swashbuckle.AspNetCore` - Documentación Swagger/OpenAPI
- `Npgsql.EntityFrameworkCore.PostgreSQL` - Provider PostgreSQL
- `Microsoft.EntityFrameworkCore.Tools` - Herramientas EF Core

### TurnoLink.Business
- `BCrypt.Net-Next` - Hash de contraseñas

### TurnoLink.DataAccess
- `Npgsql.EntityFrameworkCore.PostgreSQL` - Provider PostgreSQL
- `Microsoft.EntityFrameworkCore.Design` - Diseño EF Core

## 🔄 Flujo de Trabajo de Desarrollo

1. **Agregar nueva funcionalidad**:
   - Crear/modificar entidades en `TurnoLink.DataAccess/Entities`
   - Actualizar `DbContext` si es necesario
   - Crear migración: `dotnet ef migrations add NombreMigracion`
   - Crear DTOs en `TurnoLink.Business/DTOs`
   - Implementar servicio en `TurnoLink.Business/Services`
   - Crear controller en `TurnoLink.WebAPI/Controllers`

2. **Probar cambios**:
   - Ejecutar aplicación
   - Usar Swagger UI para probar endpoints
   - Verificar logs en la consola

## 🌐 Integraciones Futuras

- [ ] Sistema de notificaciones por Email
- [ ] Integración con WhatsApp Business API
- [ ] Sincronización con Google Calendar
- [ ] Sincronización con Microsoft Outlook Calendar

## 📝 Notas Importantes

- La cadena de conexión en `appsettings.json` es para **referencia** únicamente
- **NUNCA** commitear credenciales reales en el repositorio
- Usar `appsettings.Development.json` para configuración local
- Las migraciones se encuentran en `TurnoLink.DataAccess/Migrations`

## 🐛 Solución de Problemas

### Error de conexión a PostgreSQL
```
Verificar que PostgreSQL esté ejecutándose:
- Windows: Servicios -> PostgreSQL
- Verificar puerto 5432
- Verificar credenciales en appsettings.Development.json
```

### Error de migración
```powershell
# Eliminar base de datos y recrear
dotnet ef database drop --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
dotnet ef database update --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
```

### Error de compilación
```powershell
# Limpiar y reconstruir
dotnet clean
dotnet build
```

## 📚 Recursos

- [Documentación .NET 9](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [PostgreSQL](https://www.postgresql.org/docs/)
- [Swagger/OpenAPI](https://swagger.io/docs/)

---

**Versión**: 1.0.0  
**Última actualización**: Diciembre 2025
