# FiapGame - Fase 1

API REST em .NET 8 para cadastro de usuarios, autenticacao JWT, catalogo de jogos e biblioteca de jogos adquiridos.

## Requisitos implementados

- Cadastro de usuarios com validacao de email e senha forte.
- Autenticacao via JWT.
- Dois niveis de acesso:
  - `User`: consulta catalogo e gerencia biblioteca.
  - `Admin`: cadastro de jogos.
- Persistencia com Entity Framework Core + migrations.
- Middleware de excecao e middleware de log com `CorrelationId`.
- Swagger habilitado para documentacao.
- Testes unitarios (xUnit) no dominio.

## Como rodar

1. Ajuste connection string em `src/FiapGame.API/appsettings.Development.json`.
2. Execute:

```bash
dotnet restore
dotnet build FiapGame.slnx
dotnet run --project src/FiapGame.API/FiapGame.API.csproj
```

3. Acesse Swagger:

`http://localhost:5197/swagger`

## Usuario admin padrao

No startup, se nao existir usuario admin:

- Email: `admin@admin.com`
- Senha: `Teste@123`

## Endpoints principais

- `POST /api/Usuario` cria usuario.
- `POST /api/Auth/login` autentica e retorna token JWT.
- `GET /api/Jogo` lista jogos (autenticado).
- `POST /api/Jogo` cria jogo (somente admin).
- `POST /api/Biblioteca/{jogoId}` adiciona jogo a biblioteca do usuario autenticado.
- `GET /api/Biblioteca` lista biblioteca do usuario autenticado.

## Testes

```bash
dotnet test FiapGame.slnx
```
