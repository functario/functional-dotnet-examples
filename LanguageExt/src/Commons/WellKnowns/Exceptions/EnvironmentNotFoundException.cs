namespace WellKnowns.Exceptions;

public sealed class EnvironmentNotFoundException : Exception
{
    private static string MessageBuilder(string environmentVariableName) =>
        $"Environmement variable '{environmentVariableName}' not found.";

    public EnvironmentNotFoundException(string environmentVariableName)
        : base(MessageBuilder(environmentVariableName)) { }

    public EnvironmentNotFoundException(string environmentVariableName, Exception innerException)
        : base(MessageBuilder(environmentVariableName), innerException) { }

    private EnvironmentNotFoundException() { }
}
