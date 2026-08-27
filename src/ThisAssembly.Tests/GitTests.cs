using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ThisAssemblyTests;

/// <summary>
/// Exercises the <c>InitializeGitInformation</c> fallback that reads HEAD when
/// SourceLink does not provide <c>BranchName</c> (typical on .NET SDK 8).
/// </summary>
public record class GitTests(ITestOutputHelper Output)
{
    /// <summary />
    [Fact]
    public void ReadsBranchFromGitDirectory()
    {
        using var dir = new TempDirectory();
        var repo = Path.Combine(dir.Path, "repo");
        Directory.CreateDirectory(Path.Combine(repo, ".git"));
        File.WriteAllText(Path.Combine(repo, ".git", "HEAD"), "ref: refs/heads/main\n");

        Assert.Equal("main", EvaluateBranch(repo));
    }

    /// <summary />
    [Fact]
    public void ReadsBranchFromWorktreeGitDirFileWithRelativePath()
    {
        using var dir = new TempDirectory();
        var main = Path.Combine(dir.Path, "main");
        var worktree = Path.Combine(dir.Path, "worktree");
        var gitDir = Path.Combine(main, ".git", "worktrees", "worktree");

        Directory.CreateDirectory(gitDir);
        Directory.CreateDirectory(worktree);
        File.WriteAllText(Path.Combine(gitDir, "HEAD"), "ref: refs/heads/feature/worktree\n");

        var relativeGitDir = Path.GetRelativePath(worktree, gitDir).Replace('\\', '/');
        File.WriteAllText(Path.Combine(worktree, ".git"), $"gitdir: {relativeGitDir}\n");

        Assert.Equal("feature/worktree", EvaluateBranch(worktree));
    }

    /// <summary />
    [Fact]
    public void ReadsBranchFromWorktreeGitDirFileWithAbsolutePath()
    {
        using var dir = new TempDirectory();
        var main = Path.Combine(dir.Path, "main");
        var worktree = Path.Combine(dir.Path, "worktree");
        var gitDir = Path.Combine(main, ".git", "worktrees", "worktree");

        Directory.CreateDirectory(gitDir);
        Directory.CreateDirectory(worktree);
        File.WriteAllText(Path.Combine(gitDir, "HEAD"), "ref: refs/heads/reproduce-thisassembly-worktree\n");

        var absoluteGitDir = gitDir.Replace('\\', '/');
        File.WriteAllText(Path.Combine(worktree, ".git"), $"gitdir: {absoluteGitDir}\n");

        Assert.Equal("reproduce-thisassembly-worktree", EvaluateBranch(worktree));
    }

    /// <summary />
    [Fact]
    public void DoesNotFailWhenGitHeadIsMissing()
    {
        using var dir = new TempDirectory();
        var repo = Path.Combine(dir.Path, "repo");
        Directory.CreateDirectory(repo);

        Assert.Equal("", EvaluateBranch(repo));
    }

    string EvaluateBranch(string repositoryRoot)
    {
        var targetsPath = Path.GetFullPath(Path.Combine(ThisAssembly.Git.Root, "src", "ThisAssembly.Git", "ThisAssembly.Git.targets"));
        Assert.True(File.Exists(targetsPath), $"Could not find ThisAssembly.Git.targets at {targetsPath}");

        var evalDir = Path.Combine(Path.GetTempPath(), "ThisAssemblyGitEval", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(evalDir);
        try
        {
            var projectPath = Path.Combine(evalDir, "git.proj");
            var outputPath = Path.Combine(evalDir, "branch.txt");
            var root = repositoryRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            File.WriteAllText(projectPath, $$"""
                <Project>
                  <Target Name="InitializeSourceControlInformation" />
                  <Import Project="{{Xml(targetsPath)}}" />
                  <!-- Force the .git/HEAD fallback, as if CI/SourceLink did not populate the branch. -->
                  <PropertyGroup>
                    <RepositoryBranch></RepositoryBranch>
                  </PropertyGroup>
                  <ItemGroup>
                    <SourceRoot Include="{{Xml(root)}}">
                      <SourceControl>git</SourceControl>
                    </SourceRoot>
                  </ItemGroup>
                  <Target Name="WriteBranch" DependsOnTargets="InitializeGitInformation">
                    <WriteLinesToFile File="{{Xml(outputPath)}}" Lines="$(RepositoryBranch)" Overwrite="true" />
                  </Target>
                </Project>
                """);

            var psi = new ProcessStartInfo("dotnet")
            {
                Arguments = $"msbuild \"{projectPath}\" -nologo -v:m -t:WriteBranch",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = evalDir,
            };

            using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start dotnet msbuild.");
            var stdout = process.StandardOutput.ReadToEnd();
            var stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Output.WriteLine(stdout);
            if (!string.IsNullOrWhiteSpace(stderr))
                Output.WriteLine(stderr);

            if (process.ExitCode != 0)
                throw new InvalidOperationException($"msbuild failed ({process.ExitCode}):{Environment.NewLine}{stdout}{stderr}");

            return File.Exists(outputPath) ? File.ReadAllText(outputPath).Trim() : "";
        }
        finally
        {
            try { Directory.Delete(evalDir, recursive: true); } catch { }
        }
    }

    static string Xml(string value) => value
        .Replace("&", "&amp;")
        .Replace("\"", "&quot;")
        .Replace("'", "&apos;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");

    sealed class TempDirectory : IDisposable
    {
        public string Path { get; } = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(), "ThisAssemblyGitTests", Guid.NewGuid().ToString("N"));

        public TempDirectory() => Directory.CreateDirectory(Path);

        public void Dispose()
        {
            try { Directory.Delete(Path, recursive: true); } catch { }
        }
    }
}
