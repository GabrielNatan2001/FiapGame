namespace FiapGame.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() { }

        public DomainException(string message)
            : base(message) { }
    }
}
