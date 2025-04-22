extern alias Analyzer;
using System.Security.Cryptography;
using System.Text.Json;
using Analyzer::Devlooped.Sponsors;
using Devlooped.Sponsors;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Devlooped.Tests;

public class SponsorManifestTests
{
    // We need to convert to jwk string since the analyzer project has merged the JWT assembly and types.
    public static string ToJwk(SecurityKey key)
        => JsonSerializer.Serialize(
            JsonWebKeyConverter.ConvertFromSecurityKey(key),
            JsonOptions.JsonWebKey);

    [Fact]
    public void ValidateSponsorable()
    {
        var sponsorable = SponsorableManifest.Create(new Uri("https://foo.com"), [new Uri("https://github.com/sponsors/bar")], "ASDF1234");
        var jwt = sponsorable.ToJwt();
        var jwk = ToJwk(sponsorable.SecurityKey);

        // NOTE: sponsorable manifest doesn't have expiration date.
        var manifest = SponsorLink.ParseManifest(jwt, jwk, false);

        Assert.True(manifest.IsValid);
        Assert.Equal(ManifestStatus.Valid, manifest.Status);
    }

    [Fact]
    public void ValidateWrongKey()
    {
        var sponsorable = SponsorableManifest.Create(new Uri("https://foo.com"), [new Uri("https://github.com/sponsors/bar")], "ASDF1234");
        var jwt = sponsorable.ToJwt();
        var jwk = ToJwk(new RsaSecurityKey(RSA.Create()));

        var manifest = SponsorLink.ParseManifest(jwt, jwk, false);

        Assert.Equal(ManifestStatus.Invalid, manifest.Status);

        // We should still be a able to read the data, knowing it may have been tampered with.
        Assert.NotNull(manifest.Principal);
        Assert.NotNull(manifest.SecurityToken);
    }

    [Fact]
    public void ValidateExpiredSponsor()
    {
        var sponsorable = SponsorableManifest.Create(new Uri("https://foo.com"), [new Uri("https://github.com/sponsors/bar")], "ASDF1234");
        var jwk = ToJwk(sponsorable.SecurityKey);
        var sponsor = sponsorable.Sign([], expiration: TimeSpan.Zero);

        // Will be expired after this.
        Thread.Sleep(1000);

        var manifest = SponsorLink.ParseManifest(sponsor, jwk, true);

        Assert.Equal(ManifestStatus.Expired, manifest.Status);

        // We should still be a able to read the data, even if expired (but not tampered with).
        Assert.NotNull(manifest.Principal);
        Assert.NotNull(manifest.SecurityToken);
    }

    [Fact]
    public void ValidateUnknownFormat()
    {
        var sponsorable = SponsorableManifest.Create(new Uri("https://foo.com"), [new Uri("https://github.com/sponsors/bar")], "ASDF1234");
        var jwk = ToJwk(sponsorable.SecurityKey);

        var manifest = SponsorLink.ParseManifest("asdfasdf", jwk, false);

        Assert.Equal(ManifestStatus.Unknown, manifest.Status);

        // Nothing could be read at all.
        Assert.False(manifest.IsValid);
        Assert.NotNull(manifest.Principal);
        Assert.Null(manifest.Principal.Identity);
        Assert.Null(manifest.SecurityToken);
    }

    [Fact]
    public void TryRead()
    {
        var fooSponsorable = SponsorableManifest.Create(new Uri("https://foo.com"), [new Uri("https://github.com/sponsors/foo")], "ASDF1234");
        var barSponsorable = SponsorableManifest.Create(new Uri("https://bar.com"), [new Uri("https://github.com/sponsors/bar")], "GHJK5678");

        // Org sponsor and member of team
        var fooSponsor = fooSponsorable.Sign([new("sub", "kzu"), new("email", "me@foo.com"), new("roles", "org"), new("roles", "team")], expiration: TimeSpan.FromDays(30));
        // Org + personal sponsor
        var barSponsor = barSponsorable.Sign([new("sub", "kzu"), new("email", "me@bar.com"), new("roles", "org"), new("roles", "user")], expiration: TimeSpan.FromDays(30));

        Assert.True(SponsorLink.TryRead(out var principal, [(fooSponsor, ToJwk(fooSponsorable.SecurityKey)), (barSponsor, ToJwk(barSponsorable.SecurityKey))]));

        // Can check role across both JWTs
        Assert.True(principal.IsInRole("org"));
        Assert.True(principal.IsInRole("team"));
        Assert.True(principal.IsInRole("user"));

        Assert.True(principal.HasClaim("sub", "kzu"));
        Assert.True(principal.HasClaim("email", "me@foo.com"));
        Assert.True(principal.HasClaim("email", "me@bar.com"));
    }

    [LocalFact]
    public void ValidateCachedManifest()
    {
        var path = Environment.ExpandEnvironmentVariables("%userprofile%\\.sponsorlink\\github\\devlooped.jwt");
        if (!File.Exists(path))
            return;

        var jwt = File.ReadAllText(path);

        var manifest = SponsorLink.ParseManifest(jwt,
            """
            {
              "e": "AQAB",
              "kty": "RSA",
              "n": "5inhv8QymaDBOihNi1eY-6-hcIB5qSONFZxbxxXAyOtxAdjFCPM-94gIZqM9CDrX3pyg1lTJfml_a_FZSU9dB1ii5mSX_mNHBFXn1_l_gi1ErdbkIF5YbW6oxWFxf3G5mwVXwnPfxHTyQdmWQ3YJR-A3EB4kaFwLqA6Ha5lb2ObGpMTQJNakD4oTAGDhqHMGhu6PupGq5ie4qZcQ7N8ANw8xH7nicTkbqEhQABHWOTmLBWq5f5F6RYGF8P7cl0IWl_w4YcIZkGm2vX2fi26F9F60cU1v13GZEVDTXpJ9kzvYeM9sYk6fWaoyY2jhE51qbv0B0u6hScZiLREtm3n7ClJbIGXhkUppFS2JlNaX3rgQ6t-4LK8gUTyLt3zDs2H8OZyCwlCpfmGmdsUMkm1xX6t2r-95U3zywynxoWZfjBCJf41leM9OMKYwNWZ6LQMyo83HWw1PBIrX4ZLClFwqBcSYsXDyT8_ZLd1cdYmPfmtllIXxZhLClwT5qbCWv73V"
            }
            """
            , false);

        Assert.Equal(ManifestStatus.Valid, manifest.Status);
    }
}
