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

### Usuário & Autenticação
- `POST /api/Usuario`: Criar novo usuário.
- `POST /api/Auth/login`: Autenticar usuário e obter token JWT.

### Catálogo de Jogos
- `GET /api/Jogo`: Listar todos os jogos (Requer perfil `Admin`).
- `GET /api/Jogo/ativos`: Listar apenas jogos ativos (Requer autenticação).
- `POST /api/Jogo`: Cadastrar novo jogo (Requer perfil `Admin`).
- `PUT /api/Jogo/{id}`: Atualizar dados de um jogo (Requer perfil `Admin`).
- `PATCH /api/Jogo/{id}/status`: Alternar status (Ativar/Inativar) de um jogo (Requer perfil `Admin`).

### Biblioteca
- `POST /api/Biblioteca/{jogoId}`: Adicionar um jogo à biblioteca do usuário autenticado.
- `GET /api/Biblioteca`: Listar jogos da biblioteca do usuário autenticado.

## Testes

```bash
dotnet test FiapGame.slnx
```
