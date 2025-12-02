# Sistema de Autenticación y Autorización - TurnoLink

## 🔐 Características Implementadas

- ✅ **Autenticación JWT** (JSON Web Tokens)
- ✅ **Registro de usuarios** con hash de contraseñas (BCrypt)
- ✅ **Login** con validación de credenciales
- ✅ **Protección de endpoints** con `[Authorize]`
- ✅ **Swagger configurado** para autenticación JWT
- ✅ **Tokens con expiración** configurable

## 📡 Endpoints de Autenticación

### 1. Registro de Usuario
```http
POST /api/auth/register
Content-Type: application/json

{
  "fullName": "Juan Pérez",
  "email": "juan@example.com",
  "password": "SecurePass123!",
  "phoneNumber": "+54911234567"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
  "success": true,
  "message": "Usuario registrado exitosamente",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "juan@example.com",
    "fullName": "Juan Pérez",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "expiresAt": "2025-12-01T22:00:00Z"
  },
  "errors": []
}
```

### 2. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "juan@example.com",
  "password": "SecurePass123!"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "juan@example.com",
    "fullName": "Juan Pérez",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "expiresAt": "2025-12-01T22:00:00Z"
  },
  "errors": []
}
```

**Respuesta de Error (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Credenciales inválidas",
  "data": null,
  "errors": []
}
```

### 3. Verificar Usuario Autenticado
```http
GET /api/auth/me
Authorization: Bearer {token}
```

**Respuesta Exitosa (200 OK):**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "juan@example.com",
  "name": "Juan Pérez",
  "message": "Usuario autenticado correctamente"
}
```

## 🔒 Endpoints Protegidos

Los siguientes endpoints requieren autenticación:

### Usuarios (Todos requieren JWT)
- `GET /api/users` - Listar usuarios
- `GET /api/users/{id}` - Obtener usuario por ID
- `GET /api/users/email/{email}` - Obtener usuario por email
- `POST /api/users` - Crear usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

## 🛠️ Uso del Token JWT

### En Swagger UI

1. Registrarse o hacer login en `/api/auth/register` o `/api/auth/login`
2. Copiar el `token` de la respuesta
3. Hacer clic en el botón **Authorize** 🔓 en la parte superior de Swagger
4. Ingresar: `Bearer {tu-token-aquí}`
5. Hacer clic en **Authorize**
6. Ahora puedes acceder a los endpoints protegidos

### Con curl

```bash
# Login
curl -X POST http://localhost:5009/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan@example.com",
    "password": "SecurePass123!"
  }'

# Usar el token en requests
curl -X GET http://localhost:5009/api/users \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Con JavaScript/Fetch

```javascript
// Login
const loginResponse = await fetch('http://localhost:5009/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'juan@example.com',
    password: 'SecurePass123!'
  })
});

const { data } = await loginResponse.json();
const token = data.token;

// Usar token en requests
const usersResponse = await fetch('http://localhost:5009/api/users', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});

const users = await usersResponse.json();
```

## ⚙️ Configuración JWT

La configuración JWT se encuentra en `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "TurnoLink_SuperSecretKey_ChangeInProduction_2024",
    "Issuer": "TurnoLink",
    "Audience": "TurnoLinkUsers",
    "ExpirationMinutes": 60
  }
}
```

### Parámetros:

- **SecretKey**: Clave secreta para firmar los tokens (⚠️ cambiar en producción)
- **Issuer**: Emisor del token
- **Audience**: Audiencia del token
- **ExpirationMinutes**: Tiempo de expiración del token en minutos (60 min = 1 hora)

## 🔐 Seguridad

### Contraseñas
- Hash con **BCrypt** (algoritmo de hashing robusto)
- Salt automático generado por BCrypt
- No se almacenan contraseñas en texto plano

### Tokens JWT
- Firmados con **HMACSHA256**
- Incluyen claims: `userId`, `email`, `name`
- Validación de expiración
- Validación de issuer y audience

### Validaciones
- Email único en el sistema
- Usuario activo para login
- Token requerido en endpoints protegidos

## 📋 Claims en el Token

El token JWT incluye los siguientes claims:

```json
{
  "nameid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "iss": "TurnoLink",
  "aud": "TurnoLinkUsers",
  "exp": 1638385200
}
```

Estos claims pueden ser accedidos en los controladores:

```csharp
var userId = User.FindFirst("userId")?.Value;
var email = User.FindFirst(ClaimTypes.Email)?.Value;
var name = User.FindFirst(ClaimTypes.Name)?.Value;
```

## 🚀 Flujo de Autenticación

1. **Usuario se registra** → Recibe token JWT
2. **Usuario hace login** → Recibe token JWT
3. **Usuario incluye token** en header `Authorization: Bearer {token}`
4. **Servidor valida token** antes de procesar request
5. **Si token válido** → Procesa request
6. **Si token inválido/expirado** → Retorna 401 Unauthorized

## ⚠️ Notas Importantes

- **SecretKey**: Debe ser una clave segura de al menos 32 caracteres en producción
- **HTTPS**: Obligatorio en producción para enviar tokens de forma segura
- **Expiración**: Tokens expiran después de 60 minutos (configurable)
- **Refresh Tokens**: No implementados (futuro feature)

## 🔄 Próximas Mejoras

- [ ] Refresh tokens para renovar tokens expirados
- [ ] Roles y permisos (Admin, User, etc.)
- [ ] Two-Factor Authentication (2FA)
- [ ] Rate limiting en endpoints de auth
- [ ] Blacklist de tokens revocados
- [ ] Password reset por email

---

**Última actualización**: Diciembre 2025
