# TurnoLink - AI Coding Instructions

## Arquitectura del Proyecto

TurnoLink es una plataforma escalable de gestión de reservas que permite a profesionales ofrecer servicios de agendamiento a sus clientes.

### Estructura de Carpetas
```
TurnoLink/
├── api/          # Backend .NET 9 (3-tier architecture)
└── client/       # Frontend Next.js 15
```

## Backend (.NET 9) - Arquitectura 3-Tier

### Capas de la Aplicación
1. **Web API Layer**: Controllers, Middleware, API endpoints (Swagger configurado)
2. **Business Layer**: Services, lógica de negocio, validaciones
3. **Data Access Layer**: Repositories, Entity Framework Core, PostgreSQL

### Convenciones de Backend
- **Base de datos**: PostgreSQL con Entity Framework Core
- **Autenticación**: Sistema de login implementado (especificar JWT/Identity cuando esté definido)
- **Documentación API**: Swagger/OpenAPI habilitado en desarrollo
- **Estructura de proyectos**:
  ```
  api/
  ├── TurnoLink.WebAPI/         # Web API Layer - Controllers, Middleware, Program.cs
  ├── TurnoLink.Business/       # Business Layer - Services, DTOs, Business Logic
  └── TurnoLink.DataAccess/     # Data Access Layer - Repositories, DbContext, Entities
  ```

### Patrones y Prácticas Backend
- Usar **Repository Pattern** para acceso a datos
- Implementar **Dependency Injection** nativo de .NET
- DTOs para transferencia de datos entre capas
- Validación con FluentValidation (o Data Annotations)
- Manejo de errores centralizado con middleware
- **Documentación obligatoria**: XML comments en métodos públicos

## Frontend (Next.js 15)

### Convenciones de Frontend
- **Framework**: Next.js 15 (App Router)
- **Gestión de estado**: (Definir: Context API, Zustand, Redux, etc.)
- **Estilos**: (Definir: Tailwind CSS, CSS Modules, etc.)
- **Estructura de carpetas**:
  ```
  client/
  ├── app/              # App Router pages
  ├── components/       # Componentes reutilizables
  ├── lib/             # Utilidades y helpers
  ├── services/        # API clients y servicios externos
  └── types/           # TypeScript types/interfaces
  ```

### Patrones Frontend
- Componentes **Server Components** por defecto (Next.js 15)
- Client Components solo cuando se necesite interactividad
- Nomenclatura: PascalCase para componentes, camelCase para funciones
- **Documentación obligatoria**: JSDoc en componentes y funciones complejas

## Integraciones Externas

### Servicios Planificados
1. **Email**: Sistema de notificaciones por correo (configurar proveedor: SendGrid, Resend, etc.)
2. **WhatsApp**: Integración para notificaciones y comunicación
3. **Calendarios**: Sincronización bidireccional con calendarios de clientes y profesionales
   - Google Calendar
   - Outlook/Microsoft Calendar
   - (Otros según necesidad)

### Implementación de Integraciones
- Abstraer servicios externos detrás de interfaces
- Configuración en `appsettings.json` (backend) y variables de entorno
- Manejo de errores robusto para servicios externos
- Rate limiting y retry policies

## Workflow de Desarrollo

### Backend
```powershell
cd api
dotnet restore
dotnet build
dotnet ef database update --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
dotnet run --project TurnoLink.WebAPI
```

### Frontend
```powershell
cd client
npm install
npm run dev  # Desarrollo en http://localhost:3000
```

### Base de Datos
- Migraciones con Entity Framework Core: `dotnet ef migrations add <NombreMigracion>`
- String de conexión en `appsettings.Development.json` (no commitear credenciales)

## Estándares de Código

### Generales
- **Idioma**: Código en inglés, comentarios en español si clarifica
- **Naming**: Seguir convenciones de cada lenguaje (C# PascalCase, TypeScript camelCase)
- **Documentación obligatoria**:
  - Métodos públicos con XML comments (C#)
  - Componentes y funciones complejas con JSDoc (TypeScript)
  - README en cada proyecto principal

### Control de Calidad
- No commitear código comentado sin explicación
- Validar inputs en ambos frontend y backend
- Implementar logging apropiado (Serilog en backend, console en desarrollo frontend)
- Pruebas unitarias para lógica de negocio crítica

## Modelo de Datos Principal

### Entidades Core (a desarrollar)
- **User**: Usuarios del sistema (profesionales)
- **Client**: Clientes que reservan
- **Service**: Servicios ofrecidos por profesionales
- **Booking**: Reservas/citas
- **Availability**: Disponibilidad de profesionales
- **Notification**: Registro de notificaciones enviadas

## Seguridad

- Variables sensibles en variables de entorno (nunca hardcoded)
- HTTPS obligatorio en producción
- Validación y sanitización de inputs
- CORS configurado apropiadamente
- Rate limiting en endpoints públicos

## Comandos Útiles

### Crear nueva migración
```powershell
cd api
dotnet ef migrations add <NombreMigracion> --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
```

### Generar documentación Swagger
Acceder a `/swagger` en desarrollo (backend corriendo)

---

**Nota**: Este documento evoluciona con el proyecto. Actualizar cuando se tomen decisiones arquitectónicas importantes.
