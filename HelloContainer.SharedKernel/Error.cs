namespace HelloContainer.SharedKernel;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string code, string message) =>
        new($"NotFound.{code}", message);

    public static Error Validation(string code, string message) =>
        new($"Validation.{code}", message);

    public static Error Conflict(string code, string message) =>
        new($"Conflict.{code}", message);

    public static Error Unauthorized(string code, string message) =>
        new($"Unauthorized.{code}", message);

    public static Error Forbidden(string code, string message) =>
        new($"Forbidden.{code}", message);

    public static Error Internal(string code, string message) =>
        new($"Internal.{code}", message);
}