# TurnoLink

Plataforma escalable de gestión de reservas que permite a profesionales ofrecer servicios de agendamiento a sus clientes.

## 🏗️ Arquitectura del Proyecto

```
TurnoLink/
├── api/          # Backend .NET 9 (3-tier architecture)
│   ├── TurnoLink.WebAPI/         # Web API Layer
│   ├── TurnoLink.Business/       # Business Layer
│   └── TurnoLink.DataAccess/     # Data Access Layer
└── client/       # Frontend Next.js 15 (próximamente)
```

## 🚀 Inicio Rápido

### Requisitos Previos

- .NET 9 SDK
- PostgreSQL 9.x o superior
- Node.js 20+ (para frontend, cuando esté disponible)

### Configuración Backend

1. **Configurar base de datos**:

```bash
# Asegúrate de que PostgreSQL esté corriendo
# Puerto: 55000 (o el que uses localmente)
```

2. **Configurar variables locales**:

```bash
cd api/TurnoLink.WebAPI
cp appsettings.Development.example.json appsettings.Development.json
# Editar appsettings.Development.json con tus credenciales
```

3. **Aplicar migraciones**:

```bash
cd api
dotnet ef database update --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI
```

4. **Ejecutar API**:

```bash
dotnet run --project TurnoLink.WebAPI
# API disponible en: https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

Ver [README del backend](./api/README.md) para más detalles.

## 🔐 Seguridad

⚠️ **Archivos sensibles NO versionados**:

- `appsettings.Development.json` - Credenciales locales
- `.env*` - Variables de entorno
- `*.secrets.json` - Secretos de usuario

✅ **Usa los archivos `.example` como plantilla** para tu configuración local.

## 📚 Documentación

- [Instrucciones de desarrollo AI](./.github/copilot-instructions.md)
- [Backend README](./api/README.md)
- Frontend README (próximamente)

## 🛠️ Stack Tecnológico

### Backend (.NET 9)

- **Framework**: ASP.NET Core 9
- **ORM**: Entity Framework Core 9
- **Base de datos**: PostgreSQL
- **Autenticación**: JWT Bearer Tokens
- **Documentación**: Swagger/OpenAPI

### Frontend (Next.js 15) - Próximamente

- **Framework**: Next.js 15 (App Router)
- **UI**: Por definir
- **Estado**: Por definir

## 📝 Flujo de Trabajo

### Backend

```bash
# Desarrollo con recarga automática
cd api
dotnet watch --project TurnoLink.WebAPI

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project TurnoLink.DataAccess --startup-project TurnoLink.WebAPI

# Compilar
dotnet build
```

### Branching Strategy

- `main` - Producción estable
- `develop` - Rama de desarrollo
- `feature/*` - Nuevas funcionalidades

## 🧪 Testing

```bash
# Ejecutar tests (cuando estén implementados)
cd api
dotnet test
```

## 🤝 Contribución

1. Crear rama desde `develop`: `git checkout -b feature/nombre-feature`
2. Hacer cambios y commits
3. Push y crear Pull Request a `develop`
4. Code review y merge

## 📄 Licencia

Proyecto privado - Todos los derechos reservados

## 👥 Autores

- Equipo TurnoLink

---

**Nota**: Este proyecto está en desarrollo activo. La documentación se actualiza constantemente.
