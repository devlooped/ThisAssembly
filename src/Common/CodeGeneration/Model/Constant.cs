namespace CodeGeneration.Model;

sealed class Constant : LanguageItem
{
    public Constant(string name, string? value)
        : base(name)
    {
        Value = value;
    }

    public string? Value { get; }

    public string? XmlSummary { get; init; }
}
