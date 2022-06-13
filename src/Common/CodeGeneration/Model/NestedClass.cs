namespace CodeGeneration.Model;

sealed record NestedClass : ClassBase
{
    public string Name { get; init; } = string.Empty;
}
