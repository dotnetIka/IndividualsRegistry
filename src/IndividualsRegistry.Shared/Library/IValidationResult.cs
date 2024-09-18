namespace IndividualsRegistry.Shared.Library;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.",
        ErrorTypeEnum.UnprocessableEntity
    );

    Error[] Errors { get; }
}
