namespace CodeGeneration.Model;

sealed record ThisAssemblyClass : ClassBase
{
    public bool IsMainPart { get; init; } = false;
}
