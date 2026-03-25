using FiapGame.Application.Usuario.Dtos;
using FiapGame.Domain.Usuario.Entities;
using FiapGame.Domain.Usuario.Interfaces;
using FiapGame.Shared.Exceptions;

namespace FiapGame.Application.Usuario.Services;

public class CriarUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public CriarUsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Execute(CriarUsuarioDto.Request request)
    {
        var existente = await _repository.ObterPorEmailAsync(request.Email);

        if (existente is not null)
            throw new DomainException("Email já cadastrado");

        var usuario = UsuarioEntity.Criar(
            request.Nome,
            request.Email,
            request.Senha
        );

        await _repository.Adicionar(usuario);
        await _repository.SalvarAlteracoes();

        return usuario.Id;
    }
}
