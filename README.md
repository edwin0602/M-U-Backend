# BackEnd

## Pre-Requisitos

Añadir cadena de conexion a la BD en el archivo appsettings.json (RestBackend.Api/appsettings.json)

Es necesario aplicar las migraciones de Entity Framework en BD usando el siguiente comando:
```
dotnet ef --startup-project ./RestBackend.Api/RestBackend.Api.csproj -p ./RestBackend.Data/RestBackend.Data.csproj database update
```
(Requiere Entity Framework Tools) 
```
Mas info: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
```
## Credenciales de prueba
Credenciales para la realizacion de pruebas, se cargan al realizar la migracion
```
{
  "userName": "admin",
  "password": "Tempora02"
}
```
