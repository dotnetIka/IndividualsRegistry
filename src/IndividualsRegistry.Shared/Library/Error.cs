namespace IndividualsRegistry.Shared.Library;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorTypeEnum.None);
    public static readonly Error NullValue = new(
        "Error.NullValue",
        "The specified result value is null.",
        ErrorTypeEnum.BadRequest
    );

    public Error(string code, string message, ErrorTypeEnum errorType)
    {
        if (code == null) throw new ArgumentNullException(nameof(code));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message;
        ErrorType = errorType;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorTypeEnum ErrorType { get; }

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message && ErrorType == other.ErrorType;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message, ErrorType);

    public override string ToString() => Code;
}
