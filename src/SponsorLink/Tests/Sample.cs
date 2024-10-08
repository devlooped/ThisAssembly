extern alias Analyzer;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Analyzer::Devlooped.Sponsors;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class Sample(ITestOutputHelper output)
{
    [Theory]
    [InlineData("es-AR", SponsorStatus.Unknown)]
    [InlineData("es-AR", SponsorStatus.Expiring)]
    [InlineData("es-AR", SponsorStatus.Expired)]
    [InlineData("es-AR", SponsorStatus.User)]
    [InlineData("es-AR", SponsorStatus.Contributor)]
    [InlineData("es", SponsorStatus.Unknown)]
    [InlineData("es", SponsorStatus.Expiring)]
    [InlineData("es", SponsorStatus.Expired)]
    [InlineData("es", SponsorStatus.User)]
    [InlineData("es", SponsorStatus.Contributor)]
    [InlineData("en", SponsorStatus.Unknown)]
    [InlineData("en", SponsorStatus.Expiring)]
    [InlineData("en", SponsorStatus.Expired)]
    [InlineData("en", SponsorStatus.User)]
    [InlineData("en", SponsorStatus.Contributor)]
    [InlineData("", SponsorStatus.Unknown)]
    [InlineData("", SponsorStatus.Expiring)]
    [InlineData("", SponsorStatus.Expired)]
    [InlineData("", SponsorStatus.User)]
    [InlineData("", SponsorStatus.Contributor)]
    public void Test(string culture, SponsorStatus kind)
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture =
            culture == "" ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(culture);

        var diag = GetDescriptor(["foo"], "bar", "FB", kind);

        output.WriteLine(diag.Title.ToString());
        output.WriteLine(diag.MessageFormat.ToString());
        output.WriteLine(diag.Description.ToString());
    }

    [Fact]
    public void RenderSponsorables()
    {
        Assert.NotEmpty(SponsorLink.Sponsorables);

        foreach (var pair in SponsorLink.Sponsorables)
        {
            output.WriteLine($"{pair.Key} = {pair.Value}");
            // Read the JWK
            var jsonWebKey = Microsoft.IdentityModel.Tokens.JsonWebKey.Create(pair.Value);

            Assert.NotNull(jsonWebKey);

            using var key = RSA.Create(new RSAParameters
            {
                Modulus = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.DecodeBytes(jsonWebKey.N),
                Exponent = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.DecodeBytes(jsonWebKey.E),
            });
        }
    }

    DiagnosticDescriptor GetDescriptor(string[] sponsorable, string product, string prefix, SponsorStatus status) => status switch
    {
        SponsorStatus.Unknown => DiagnosticsManager.CreateUnknown(sponsorable, product, prefix),
        SponsorStatus.Expiring => DiagnosticsManager.CreateExpiring(sponsorable, prefix),
        SponsorStatus.Expired => DiagnosticsManager.CreateExpired(sponsorable, prefix),
        SponsorStatus.User => DiagnosticsManager.CreateSponsor(sponsorable, prefix),
        SponsorStatus.Contributor => DiagnosticsManager.CreateContributor(sponsorable, prefix),
        _ => throw new NotImplementedException(),
    };
}