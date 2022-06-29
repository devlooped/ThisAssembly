using System.Diagnostics.CodeAnalysis;
using CodeGeneration.Model;
using Microsoft.CodeAnalysis;

namespace CodeGeneration;

static class Diagnostics
{
    const string Category = "ThisAssembly";

    readonly static DiagnosticDescriptor InvalidThisAssemblyClassName = MakeErrorDescriptor(
        "THIS010",
        "The name of the ThisAssembly class should be a valid identifier",
        "Invalid ThisAssembly class name '{0}'");

    readonly static DiagnosticDescriptor InvalidNestedClassName = MakeErrorDescriptor(
        "THIS011",
        "The name of a nested class should be a valid identifier",
        "Invalid nested class name '{0}' in class '{1}'");

    readonly static DiagnosticDescriptor InvalidConstantName = MakeErrorDescriptor(
        "THIS012",
        "The name of a constant should be a valid identifier",
        "Invalid constant name '{0}' in class '{1}'");

    readonly static DiagnosticDescriptor DuplicateMemberName = MakeErrorDescriptor(
        "THIS020",
        "No two members of a class should not have the same name",
        "Duplicate member name '{0}' in class '{1}'");

    readonly static DiagnosticDescriptor NamelessConstant = MakeErrorDescriptor(
        "THIS200",
        "A constant should have a name",
        "Constant '{0}' with value '{1}' has no name");

    [DoesNotReturn]
    public static void ThrowInvalidThisAssemblyClassName(string name)
        => throw new DiagnosticException(Diagnostic.Create(InvalidThisAssemblyClassName, null, name));

    [DoesNotReturn]
    public static void ThrowInvalidNestedClassName(LanguageItem? parent, string name)
        => throw new DiagnosticException(Diagnostic.Create(InvalidNestedClassName, null, name, parent?.FullName ?? string.Empty));

    [DoesNotReturn]
    public static void ThrowInvalidConstantName(LanguageItem? parent, string name)
        => throw new DiagnosticException(Diagnostic.Create(InvalidConstantName, null, name, parent?.FullName ?? string.Empty));

    [DoesNotReturn]
    public static void ThrowDuplicateMemberName(LanguageItem? parent, string name)
        => throw new DiagnosticException(Diagnostic.Create(DuplicateMemberName, null, name, parent?.FullName ?? string.Empty));

    [DoesNotReturn]
    public static void ThrowNamelessConstant(string name, string? value)
        => throw new DiagnosticException(Diagnostic.Create(NamelessConstant, null, name ?? string.Empty, value ?? string.Empty));

    static DiagnosticDescriptor MakeErrorDescriptor(string id, string title, string format)
        => new(id, title, format, Category, DiagnosticSeverity.Error, true);
}
