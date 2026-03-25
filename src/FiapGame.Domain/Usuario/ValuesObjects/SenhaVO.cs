using FiapGame.Shared.Exceptions;

namespace FiapGame.Domain.Usuario.ValuesObjects;

public class SenhaVO
{
    public string Hash { get; private set; }

    protected SenhaVO() { }

    private SenhaVO(string hash)
    {
        Hash = hash;
    }

    public static SenhaVO Create(string plainPassword)
    {
        if (!IsValid(plainPassword))
            throw new DomainException("Senha deve ter no mínimo 8 caracteres com letras, números e especiais");

        var hash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

        return new SenhaVO(hash);
    }

    public bool Verify(string plainPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, Hash);
    }

    private static bool IsValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        bool hasLetter = password.Any(char.IsLetter);
        bool hasNumber = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

        return hasLetter && hasNumber && hasSpecial;
    }
}
