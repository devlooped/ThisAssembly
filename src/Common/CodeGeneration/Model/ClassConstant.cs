namespace CodeGeneration.Model;

sealed record ClassConstant
{
    public string Name { get; init; } = string.Empty;

    public string? Value { get; init; }
}
