# Gestión de Feeds de Noticias - Web API en .NET 8

Este proyecto es una Web API desarrollada en .NET 8 con el propósito de gestionar feeds de noticias. Un feed es una agrupación de topics (temas), con un máximo de 5 topics por feed. La API permite a los usuarios registrarse, autenticar sus sesiones mediante JWT, y gestionar feeds y topics creados por el mismo.

## Requisitos
- **.NET 8**: Necesario para ejecutar la aplicación.
- **Docker Desktop**: La solución está dockerizada. Se necesita Docker Desktop o cualquier otra plataforma compatible con Docker para ejecutar el proyecto.

## Autenticación JWT
La API utiliza autenticación JWT para proteger los endpoints. Los tokens JWT deben incluirse en la cabecera de las solicitudes con el formato `Authorization: Bearer <TOKEN_JWT>`.

## Configuración JWT
La configuración del token JWT, incluida la clave secreta y el tiempo de expiración, se define en el archivo `appsettings.json`:

```json
{
  "JWTConfig": {
    "SecretKey": "secretKey",
    "ExpirationMinutes": 180,
    "Issuer": "issuer",
    "Audience": "Audience"
  }
}
```

## Estructura de Respuesta
Todas las respuestas de la API están estandarizadas bajo la siguiente estructura:
```json
{
  "typeOfResponse": 0,
  "message": "string",
  "data": "dinamico"
}
```

### Tipos de Respuesta (TypeOfResponse)
- **OK (0)**: Respuesta exitosa.
- **FailedResponse (1)**: Error controlado, como errores de validación o datos faltantes.
- **Exception (2)**: Error no controlado.
- **TimeOut (3)**: Tiempo de espera excedido.
- **NotFound (4)**: Recurso no encontrado.

## Endpoints

### **Users**

#### **Register**

Registra un nuevo usuario.

- **Método**: POST
- **Endpoint**: `api/Users/Register`
- **Parámetros de entrada**:

| Parámetro   | Tipo   | Requerido | Descripción                      |
|-------------|--------|-----------|----------------------------------|
| `name`      | String | Sí        | Nombre del usuario               |
| `mail`      | String | Sí        | Correo electrónico del usuario   |
| `password`  | String | Sí        | Contraseña del usuario           |

**Salida:** Obejto Response, donde data es el usuario creado.

### **Auth**

#### **GetToken**

Obtiene un token JWT para autenticación.

- **Método**: POST
- **Endpoint**: `api/Auth/GetToken`
- **Parámetros de entrada**:

| Parámetro   | Tipo   | Requerido | Descripción                      |
|-------------|--------|-----------|----------------------------------|
| `mail`      | String | Sí        | Correo electrónico del usuario   |
| `password`  | String | Sí        | Contraseña del usuario           |

**Salida:** Obejto Response, donde data es el token JWT que será usado en las demás llamadas.

### **Feeds**

#### **Create**

Crea un nuevo feed.

- **Método**: POST
- **Endpoint**: `/Feeds/Create`
- **Parámetros de entrada**:

| Parámetro     | Tipo    | Requerido | Descripción                              |
|---------------|---------|-----------|------------------------------------------|
| `name`        | String  | Sí        | Nombre del feed                          |
| `description` | String  | Sí        | Descripción del feed                     |
| `isPrivate`   | Boolean | No        | Indica si el feed es privado (por defecto es `false`) |

**Salida:** Obejto Response, donde data es el feed creado.

#### **EditFeed**

Edita un feed existente.

- **Método**: POST
- **Endpoint**: `/Feeds/EditFeed`
- **Parámetros de entrada**:

| Parámetro     | Tipo    | Requerido | Descripción                    |
|---------------|---------|-----------|--------------------------------|
| `id`          | Integer | Sí        | ID del feed                    |
| `name`        | String  | No        | Nombre del feed                |
| `description` | String  | No        | Descripción del feed           |
| `isPrivate`   | Boolean | No        | Indica si el feed es privado   |

**Salida:** Obejto Response, donde data es el feed editado.
#### **DeleteFeed**

Elimina un feed.

- **Método**: POST
- **Endpoint**: `/Feeds/DeleteFeed`
- **Parámetros de entrada**:

| Parámetro | Tipo    | Requerido | Descripción |
|-----------|---------|-----------|-------------|
| `id`      | Integer | Sí        | ID del feed |

**Salida:** Obejto Response

#### **AddTopic**

Agrega un topic a un feed existente.

- **Método**: POST
- **Endpoint**: `/Feeds/AddTopic`
- **Parámetros de entrada**:

| Parámetro     | Tipo    | Requerido | Descripción                |
|---------------|---------|-----------|----------------------------|
| `feedId`      | Integer | Sí        | ID del feed                |
| `name`        | String  | Sí        | Nombre del topic           |
| `description` | String  | Sí        | Descripción del topic      |

**Salida:** Obejto Response, donde data es el topic creado.
#### **DeleteTopic**

Elimina un topic de un feed.

- **Método**: POST
- **Endpoint**: `/Feeds/DeleteTopic`
- **Parámetros de entrada**:

| Parámetro | Tipo    | Requerido | Descripción   |
|-----------|---------|-----------|---------------|
| `id`      | Integer | Sí        | ID del topic  |

**Salida:** Obejto Response
#### **GetUserFeeds**

Obtiene los feeds creados por el usuario autenticado.

- **Método**: GET
- **Endpoint**: `/Feeds/GetUserFeeds`
- **Parámetros de entrada**:

| Parámetro    | Tipo    | Requerido | Descripción                                  |
|--------------|---------|-----------|----------------------------------------------|
| `PageNumber` | Integer | No        | Número de página (opcional, por defecto 1)   |
| `PageSize`   | Integer | No        | Tamaño de la página (opcional, por defecto 10) |

**Salida:** Obejto Response, donde data es el listado.
#### **GetPublicFeeds**

Obtiene los feeds públicos, no creados por el usuario autenticado.

- **Método**: GET
- **Endpoint**: `/Feeds/GetPublicFeeds`
- **Parámetros de entrada**:

| Parámetro    | Tipo    | Requerido | Descripción                                  |
|--------------|---------|-----------|----------------------------------------------|
| `PageNumber` | Integer | No        | Número de página (opcional, por defecto 1)   |
| `PageSize`   | Integer | No        | Tamaño de la página (opcional, por defecto 10) |

**Salida:** Obejto Response, donde data es el listado.

#### **GetNews**

Obtiene las 20 noticias más relevantes de los topics de un feed.

- **Método**: GET
- **Endpoint**: `/Feeds/GetNews`
- **Parámetros de entrada**:

| Parámetro | Tipo    | Requerido | Descripción                    |
|-----------|---------|-----------|--------------------------------|
| `feedId`  | Integer | Sí        | ID del feed para obtener noticias |

**Salida:** Obejto Response, donde data es el listado.

## To Do

Pendientes del proyecto:

1. **Métodos para suscripción de usuarios a feeds**: Implementar métodos para que los usuarios puedan suscribirse a diferentes feeds. Ya existe una tabla intermedia en la base de datos que registre las suscripciones de los usuarios, no se usa.

2. **Creación de roles para gestión avanzada**: Desarrollar un sistema de roles y permisos para permitir una mayor gestión y control de acceso en la API. 

3. **Función de Azure para integración con APIs de terceros**: Migrar la consulta a un API de terceros para obtener noticias en una función de Azure. Implementar un notificador de mail en otra función de Azure
