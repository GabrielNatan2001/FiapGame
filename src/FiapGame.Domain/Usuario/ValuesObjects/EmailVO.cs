using FiapGame.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace FiapGame.Domain.Usuario.ValuesObjects;

public class EmailVo
{
    public string Value { get; private set; }

    protected EmailVo() { } // EF Core

    public EmailVo(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email não pode ser vazio");

        value = value.Trim().ToLower();

        if (!IsValid(value))
            throw new DomainException("Email inválido");

        Value = value;
    }

    private static bool IsValid(string email)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}
