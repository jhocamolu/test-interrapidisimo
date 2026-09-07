# API de Registro de Estudiantes

Backend desarrollado con ASP.NET Core para gestionar estudiantes, materias y compañeros de clase.

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- Base de datos InMemory
- MediatR
- FluentValidation
- Swagger / OpenAPI

## Requisitos

- .NET SDK 10 instalado

Verificar la instalación:

```bash
dotnet --version
```

## Ejecución

Desde la carpeta `Back/`:

```bash
dotnet restore
dotnet run --launch-profile http
```

La API estará disponible en:

- `http://localhost:5122`
- Swagger UI: `http://localhost:5122/swagger`

También puede ejecutarse con HTTPS:

```bash
dotnet run --launch-profile https
```

En ese caso, las URLs configuradas son `https://localhost:7189` y `http://localhost:5122`.

## Base de datos

La aplicación utiliza una base de datos InMemory llamada `StudentRegistrationDb`. No requiere configuración de conexión ni migraciones.

Al iniciar, se cargan automáticamente:

- 5 profesores.
- 10 materias.
- Un estudiante de ejemplo, `Juan Pérez`, inscrito en tres materias.

Los datos se mantienen únicamente mientras la aplicación está en ejecución; al reiniciarla se vuelven a crear los datos iniciales.

### Materias disponibles

| ID | Materia | Créditos |
| --- | --- | ---: |
| `01000000-0000-0000-0000-000000000001` | Matemáticas I | 3 |
| `01000000-0000-0000-0000-000000000002` | Álgebra Lineal | 3 |
| `02000000-0000-0000-0000-000000000001` | Física General | 3 |
| `02000000-0000-0000-0000-000000000002` | Mecánica | 3 |
| `03000000-0000-0000-0000-000000000001` | Programación I | 3 |
| `03000000-0000-0000-0000-000000000002` | Bases de Datos | 3 |
| `04000000-0000-0000-0000-000000000001` | Estructura de Datos | 3 |
| `04000000-0000-0000-0000-000000000002` | Redes de Computadores | 3 |
| `05000000-0000-0000-0000-000000000001` | Sistemas Operativos | 3 |
| `05000000-0000-0000-0000-000000000002` | Ingeniería de Software | 3 |

## Endpoints

Todos los endpoints usan la ruta base `/api/students`.

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/api/students` | Obtiene todos los estudiantes. |
| `POST` | `/api/students` | Registra un estudiante. |
| `PUT` | `/api/students/{id}` | Actualiza un estudiante. |
| `DELETE` | `/api/students/{id}` | Elimina un estudiante. |
| `GET` | `/api/students/{studentId}/classmates` | Obtiene los compañeros de un estudiante. |

### Registrar un estudiante

```bash
curl -X POST http://localhost:5122/api/students \
  -H "Content-Type: application/json" \
  -d '{
    "name": "María Gómez",
    "email": "maria.gomez@email.com",
    "subjectIds": [
      "01000000-0000-0000-0000-000000000001",
      "03000000-0000-0000-0000-000000000001"
    ]
  }'
```

Respuesta exitosa (`201 Created`):

```json
{
  "id": "00000000-0000-0000-0000-000000000000"
}
```

El valor de `id` será generado por la aplicación.

### Actualizar un estudiante

El `id` de la URL debe coincidir con el `id` enviado en el cuerpo:

```bash
curl -X PUT http://localhost:5122/api/students/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "id": "{id}",
    "name": "María Gómez Actualizada",
    "email": "maria.actualizada@email.com",
    "subjectIds": [
      "02000000-0000-0000-0000-000000000001"
    ]
  }'
```

### Consultar compañeros

```bash
curl http://localhost:5122/api/students/{studentId}/classmates
```

## Estructura principal

```text
Back/
├── Common/
│   ├── Behaviors/              # Comportamientos de MediatR y validación
│   └── Students/               # Commands, queries y handlers
├── Controllers/                # Endpoints HTTP
├── Data/                       # ApplicationDbContext y seed de datos
├── DTOs/                       # Objetos de transferencia de datos
├── Middlewares/                # Manejo global de excepciones
├── Models/                     # Entidades del dominio
├── Program.cs                  # Configuración de servicios y pipeline
└── Back.csproj                 # Dependencias y configuración del proyecto
```

## Compilar y probar

```bash
dotnet build
dotnet test
```

Actualmente el proyecto no contiene un proyecto de pruebas automatizadas, por lo que `dotnet test` no ejecutará casos de prueba hasta que se agregue uno.
