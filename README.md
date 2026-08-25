# ReservaAi

API REST para reserva de salas, em ASP.NET Core com EF Core e SQLite.

## Stack

- .NET 10
- ASP.NET Core (Controllers)
- EF Core 10 + SQLite
- OpenAPI (em Development)

## Requisitos

- [SDK do .NET 10](https://dotnet.microsoft.com/download)
- Ferramenta EF Core, só na primeira vez:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Como rodar

```bash
# 1. restaurar dependências
dotnet restore

# 2. criar o banco (ReservaAi.db) a partir das migrations
dotnet ef database update

# 3. subir a API
dotnet run
```

A API sobe em `http://localhost:5119` (e `https://localhost:7097` no perfil https).

Para usar outra porta:

```bash
dotnet run --urls "http://localhost:5199"
```

O banco é o arquivo `ReservaAi.db` na raiz do projeto; a connection string fica em `appsettings.json`.

## Formato das respostas

Todo endpoint devolve o mesmo envelope:

```json
{
  "data": { },
  "errors": []
}
```

Em caso de erro, `data` vem `null` e `errors` traz as mensagens.

## Endpoints

### Usuários

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/v1/users` | Lista todos | 200 |
| GET | `/v1/users/{id}` | Busca por id | 200 / 404 |
| POST | `/v1/users` | Cria | 201 / 400 |
| PUT | `/v1/users/{id}` | Atualiza | 200 / 400 / 404 |
| DELETE | `/v1/users/{id}` | Remove | 200 / 404 |

**Corpo de POST / PUT:**

```json
{
  "username": "edulago",
  "email": "edulago@teste.com",
  "fullName": "Eduardo Lago",
  "birthdate": "1998-04-23",
  "password": "senha123"
}
```

- `password` é **obrigatório no POST** e **opcional no PUT** — se omitido na atualização, a senha atual é mantida. Quando enviado, precisa ter entre 6 e 100 caracteres.
- `birthdate` é só data, sem hora (`AAAA-MM-DD`).
- A senha é gravada com hash (PBKDF2, via `IPasswordHasher<User>`) e **nunca** volta nas respostas.

**Resposta:**

```json
{
  "data": {
    "id": 1,
    "username": "edulago",
    "email": "edulago@teste.com",
    "fullName": "Eduardo Lago",
    "birthdate": "1998-04-23",
    "created_at": "2026-08-25T09:51:58Z"
  },
  "errors": []
}
```

### Salas

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/v1/rooms` | Lista todas | 200 |
| GET | `/v1/rooms/{id}` | Busca por id | 200 / 404 |
| POST | `/v1/rooms` | Cria | 201 / 400 |
| PUT | `/v1/rooms/{id}` | Atualiza | 200 / 400 / 404 |
| DELETE | `/v1/rooms/{id}` | Remove | 200 / 404 |
| GET | `/v1/rooms/available?start=&end=` | Salas livres no intervalo | 200 |

**Corpo de POST / PUT:**

```json
{
  "name": "Sala Alfa",
  "capacity": 12,
  "description": "Sala de reuniao",
  "status": 0
}
```

- `name`: 3 a 40 caracteres · `capacity`: 1 a 1000 · `description`: até 200 · `status`: 0 a 10.

**Disponibilidade** — `start` e `end` são datas com hora (ISO 8601):

```
GET /v1/rooms/available?start=2026-08-25T08:00:00&end=2026-08-25T10:00:00
```