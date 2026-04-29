# language: pt
Funcionalidade: Criação de Usuário
  Como um visitante do sistema
  Eu quero me cadastrar como usuário
  Para que eu possa acessar os recursos do sistema

Cenário: Cadastro de usuário com e-mail não registrado
  Dado que eu informo os seguintes dados para cadastro:
    | Nome         | Email              | Senha    |
    | Gabriel Teste| teste@fiapgame.com | Senha@123 |
  E o e-mail "teste@fiapgame.com" não está cadastrado no sistema
  Quando eu solicitar a criação do usuário
  Então o usuário deve ser criado com sucesso
  E o repositório deve salvar as alterações

Cenário: Cadastro de usuário com e-mail já registrado
  Dado que eu informo os seguintes dados para cadastro:
    | Nome         | Email              | Senha    |
    | Gabriel Teste| teste@fiapgame.com | Senha@123 |
  E o e-mail "teste@fiapgame.com" já está cadastrado no sistema
  Quando eu solicitar a criação do usuário
  Então o sistema deve retornar um erro de domínio com a mensagem "Email já cadastrado"

Cenário: Cadastro de usuário com e-mail inválido
  Dado que eu informo os seguintes dados para cadastro:
    | Nome         | Email         | Senha    |
    | Gabriel Teste| emailinvalido | Senha@123 |
  Quando eu solicitar a criação do usuário
  Então o sistema deve retornar um erro de domínio com a mensagem "Email inválido"

Cenário: Cadastro de usuário com senha inválida
  Dado que eu informo os seguintes dados para cadastro:
    | Nome         | Email              | Senha    |
    | Gabriel Teste| teste@fiapgame.com | fraca |
  Quando eu solicitar a criação do usuário
  Então o sistema deve retornar um erro de domínio com a mensagem "Senha deve ter no mínimo 8 caracteres com letras, números e especiais"
