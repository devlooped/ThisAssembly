# Changelog

## [v2.0.5](https://github.com/devlooped/ThisAssembly/tree/v2.0.5) (2024-10-09)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v2.0.4...v2.0.5)

:sparkles: Implemented enhancements:

- Make sure we escape constants XML comments [\#425](https://github.com/devlooped/ThisAssembly/pull/425) (@kzu)
- Since types can be public, ensure we always have XML comments [\#424](https://github.com/devlooped/ThisAssembly/pull/424) (@kzu)

:bug: Fixed bugs:

- Setting GenerateAssemblyInfo=false stops source generator from running [\#407](https://github.com/devlooped/ThisAssembly/issues/407)

## [v2.0.4](https://github.com/devlooped/ThisAssembly/tree/v2.0.4) (2024-10-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v2.0.3...v2.0.4)

:sparkles: Implemented enhancements:

- Allow project properties to specify custom comment [\#420](https://github.com/devlooped/ThisAssembly/pull/420) (@kzu)

:bug: Fixed bugs:

- Properly emit AssemblyInfo even if GenerateAssemblyInfo=false [\#422](https://github.com/devlooped/ThisAssembly/pull/422) (@kzu)

## [v2.0.3](https://github.com/devlooped/ThisAssembly/tree/v2.0.3) (2024-09-30)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v2.0.2...v2.0.3)

:sparkles: Implemented enhancements:

- Add ThisAssembly.Vsix [\#405](https://github.com/devlooped/ThisAssembly/issues/405)
- Avoid non-Git constants conflict, update root comment [\#415](https://github.com/devlooped/ThisAssembly/pull/415) (@kzu)
- Allow constants to define custom root comment summary [\#414](https://github.com/devlooped/ThisAssembly/pull/414) (@kzu)
- Add ThisAssembly.Vsix for VSIX extensibility projects [\#412](https://github.com/devlooped/ThisAssembly/pull/412) (@kzu)

:twisted_rightwards_arrows: Merged:

- Provide custom Metadata class summary [\#418](https://github.com/devlooped/ThisAssembly/pull/418) (@kzu)
- Provide custom AssemblyInfo class summary [\#417](https://github.com/devlooped/ThisAssembly/pull/417) (@kzu)
- Provide custom Vsix class summary [\#416](https://github.com/devlooped/ThisAssembly/pull/416) (@kzu)

## [v2.0.2](https://github.com/devlooped/ThisAssembly/tree/v2.0.2) (2024-09-30)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v2.0.0...v2.0.2)

:bug: Fixed bugs:

- Warnings about invalid XML Comments in generated code [\#408](https://github.com/devlooped/ThisAssembly/issues/408)
- Properly fix formatting error by switching to href [\#411](https://github.com/devlooped/ThisAssembly/pull/411) (@kzu)
- Avoid duplicate \[Obsolete\] attribute in Resources [\#410](https://github.com/devlooped/ThisAssembly/pull/410) (@kzu)

:twisted_rightwards_arrows: Merged:

- Add sponsored API annotations to Resources too [\#409](https://github.com/devlooped/ThisAssembly/pull/409) (@kzu)

## [v2.0.0](https://github.com/devlooped/ThisAssembly/tree/v2.0.0) (2024-09-27)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v2.0.0-alpha...v2.0.0)

:twisted_rightwards_arrows: Merged:

- Remove doc on DateTime being invalid [\#402](https://github.com/devlooped/ThisAssembly/pull/402) (@kzu)

## [v2.0.0-alpha](https://github.com/devlooped/ThisAssembly/tree/v2.0.0-alpha) (2024-09-27)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.1-alpha...v2.0.0-alpha)

:sparkles: Implemented enhancements:

- Add support for non-string constants [\#150](https://github.com/devlooped/ThisAssembly/issues/150)
- Allow switching to static readonly properties instead of constants [\#64](https://github.com/devlooped/ThisAssembly/issues/64)
- Add support for ThisAssemblyVisibility for public and static readonly constants [\#400](https://github.com/devlooped/ThisAssembly/pull/400) (@kzu)
- Reuse doc on root namespace and add visibility doc [\#398](https://github.com/devlooped/ThisAssembly/pull/398) (@kzu)
- Add support for typed constants [\#396](https://github.com/devlooped/ThisAssembly/pull/396) (@kzu)
- Fix multiline values being truncated [\#392](https://github.com/devlooped/ThisAssembly/pull/392) (@kzu)
- Unify and future-proof Assembly\*Attributes with Constants [\#391](https://github.com/devlooped/ThisAssembly/pull/391) (@kzu)
- Make project constants a special case of Constants [\#380](https://github.com/devlooped/ThisAssembly/pull/380) (@kzu)

:bug: Fixed bugs:

- Multiline item values get truncated [\#390](https://github.com/devlooped/ThisAssembly/issues/390)

:twisted_rightwards_arrows: Merged:

- After grace period, emit code with warnings [\#383](https://github.com/devlooped/ThisAssembly/pull/383) (@kzu)
- Make sure SponsorLink disclaimer exists in package description [\#378](https://github.com/devlooped/ThisAssembly/pull/378) (@kzu)

## [v1.5.1-alpha](https://github.com/devlooped/ThisAssembly/tree/v1.5.1-alpha) (2024-09-10)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.0...v1.5.1-alpha)

:sparkles: Implemented enhancements:

- CS0229: Ambiguity between 'ThisAssembly.Info.Description' and 'ThisAssembly.Info.Description' when using Unclassified.NetRevisionTask [\#349](https://github.com/devlooped/ThisAssembly/issues/349)

:twisted_rightwards_arrows: Merged:

- Make sure all packages include the note about SponsorLink [\#377](https://github.com/devlooped/ThisAssembly/pull/377) (@kzu)
- Aggregate individual package readmes into meta-package [\#374](https://github.com/devlooped/ThisAssembly/pull/374) (@kzu)
- +Mᐁ includes [\#370](https://github.com/devlooped/ThisAssembly/pull/370) (@devlooped-bot)
- Avoid expanding sponsors section via include.yml [\#369](https://github.com/devlooped/ThisAssembly/pull/369) (@kzu)
- Update readme.md to use `[!NOTE]` syntax [\#362](https://github.com/devlooped/ThisAssembly/pull/362) (@norwd)

## [v1.5.0](https://github.com/devlooped/ThisAssembly/tree/v1.5.0) (2024-07-24)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.0-rc.3...v1.5.0)

## [v1.5.0-rc.3](https://github.com/devlooped/ThisAssembly/tree/v1.5.0-rc.3) (2024-07-23)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.0-rc.2...v1.5.0-rc.3)

:sparkles: Implemented enhancements:

- Detect metapackage as direct dependency too [\#360](https://github.com/devlooped/ThisAssembly/pull/360) (@kzu)

## [v1.5.0-rc.2](https://github.com/devlooped/ThisAssembly/tree/v1.5.0-rc.2) (2024-07-23)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.0-rc.1...v1.5.0-rc.2)

:sparkles: Implemented enhancements:

- Add support for setting ThisAssembly namespace [\#211](https://github.com/devlooped/ThisAssembly/issues/211)
- Add support for ThisAssemblyNamespace to change root namespace [\#354](https://github.com/devlooped/ThisAssembly/pull/354) (@kzu)

:twisted_rightwards_arrows: Merged:

- +Mᐁ includes [\#355](https://github.com/devlooped/ThisAssembly/pull/355) (@devlooped-bot)

## [v1.5.0-rc.1](https://github.com/devlooped/ThisAssembly/tree/v1.5.0-rc.1) (2024-07-21)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.0-rc...v1.5.0-rc.1)

## [v1.5.0-rc](https://github.com/devlooped/ThisAssembly/tree/v1.5.0-rc) (2024-07-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.5.0-beta...v1.5.0-rc)

:bug: Fixed bugs:

- ThisAssembly.Git exception when project has Git submodule [\#303](https://github.com/devlooped/ThisAssembly/issues/303)

## [v1.5.0-beta](https://github.com/devlooped/ThisAssembly/tree/v1.5.0-beta) (2024-07-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.4.3...v1.5.0-beta)

:sparkles: Implemented enhancements:

- Allow format strings in ThisAssembly.Strings [\#299](https://github.com/devlooped/ThisAssembly/issues/299)
- Allow format strings in ThisAssembly.Strings [\#313](https://github.com/devlooped/ThisAssembly/pull/313) (@kzu)
- Simplify and redesign ThisAssembly.Metadata as a special case of Constants [\#312](https://github.com/devlooped/ThisAssembly/pull/312) (@kzu)

:bug: Fixed bugs:

- Constants and Metadata values containing semicolon lose data after first semicolon [\#319](https://github.com/devlooped/ThisAssembly/issues/319)
- If resx string contains newlines, default comment breaks codegen [\#308](https://github.com/devlooped/ThisAssembly/issues/308)
- ThisAssembly is unusable in old-style NetFX projects. error CS0122: 'ThisAssembly' is inaccessible due to its protection level [\#301](https://github.com/devlooped/ThisAssembly/issues/301)
- ThisAssembly.Resources causes IL3000 warnings when publishing for AOT compilation in .NET 8 [\#279](https://github.com/devlooped/ThisAssembly/issues/279)
- Fails for assembly metadata containing `.` [\#247](https://github.com/devlooped/ThisAssembly/issues/247)
- Automatically escape and unescape constants with semicolon [\#320](https://github.com/devlooped/ThisAssembly/pull/320) (@kzu)
- Sanitize multi-line default comment [\#309](https://github.com/devlooped/ThisAssembly/pull/309) (@kzu)

:twisted_rightwards_arrows: Merged:

- Fixed Git submodule issue in ThisAssembly.Git.targets [\#334](https://github.com/devlooped/ThisAssembly/pull/334) (@TibbsTerry)
- ⬆️ Bump files with dotnet-file sync [\#332](https://github.com/devlooped/ThisAssembly/pull/332) (@kzu)
- Update test project for simplified SL targets [\#323](https://github.com/devlooped/ThisAssembly/pull/323) (@kzu)
- Incorporate SponsorLink v2 [\#321](https://github.com/devlooped/ThisAssembly/pull/321) (@kzu)
- Permanently delete legacy SL implementation [\#315](https://github.com/devlooped/ThisAssembly/pull/315) (@kzu)
- Refactor common files and resources into a shared project [\#311](https://github.com/devlooped/ThisAssembly/pull/311) (@kzu)

## [v1.4.3](https://github.com/devlooped/ThisAssembly/tree/v1.4.3) (2024-01-30)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.4.2...v1.4.3)

:sparkles: Implemented enhancements:

- Remove local path discovery for AOT support [\#288](https://github.com/devlooped/ThisAssembly/pull/288) (@kzu)

:bug: Fixed bugs:

- Generated files should include `/// <auto-generated/>` preamble [\#273](https://github.com/devlooped/ThisAssembly/issues/273)

## [v1.4.2](https://github.com/devlooped/ThisAssembly/tree/v1.4.2) (2024-01-30)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.4.1...v1.4.2)

:bug: Fixed bugs:

- ThisAssembly.Strings: If resource name is not a valid code identifier, compilation fails [\#83](https://github.com/devlooped/ThisAssembly/issues/83)

:twisted_rightwards_arrows: Merged:

- Specify comparer when search resource name [\#280](https://github.com/devlooped/ThisAssembly/pull/280) (@atifaziz)
- Add auto-generated to `EmbeddedResource.cs` [\#278](https://github.com/devlooped/ThisAssembly/pull/278) (@viceroypenguin)

## [v1.4.1](https://github.com/devlooped/ThisAssembly/tree/v1.4.1) (2023-08-30)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.4.0...v1.4.1)

:bug: Fixed bugs:

- Fix improper generation of C\# 11 raw strings [\#262](https://github.com/devlooped/ThisAssembly/pull/262) (@kzu)

:twisted_rightwards_arrows: Merged:

- Escape invalid identifiers for file constants [\#249](https://github.com/devlooped/ThisAssembly/pull/249) (@PhenX)

## [v1.4.0](https://github.com/devlooped/ThisAssembly/tree/v1.4.0) (2023-08-11)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.3.1...v1.4.0)

:twisted_rightwards_arrows: Merged:

- Remove current implementation of SponsorLink for now [\#256](https://github.com/devlooped/ThisAssembly/pull/256) (@kzu)

## [v1.3.1](https://github.com/devlooped/ThisAssembly/tree/v1.3.1) (2023-07-06)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.3.0...v1.3.1)

## [v1.3.0](https://github.com/devlooped/ThisAssembly/tree/v1.3.0) (2023-07-03)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.15...v1.3.0)

:sparkles: Implemented enhancements:

- Use raw string literals if supported by the target language options [\#243](https://github.com/devlooped/ThisAssembly/issues/243)
- Use raw string literals if supported by the target language [\#244](https://github.com/devlooped/ThisAssembly/pull/244) (@kzu)

:bug: Fixed bugs:

- AssemblyDescriptionAttribute is not included in AssemblyInfoGenerator [\#234](https://github.com/devlooped/ThisAssembly/issues/234)

## [v1.2.15](https://github.com/devlooped/ThisAssembly/tree/v1.2.15) (2023-05-09)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.14...v1.2.15)

:sparkles: Implemented enhancements:

- Add missing constant for AssemblyDescriptionAttribute [\#235](https://github.com/devlooped/ThisAssembly/pull/235) (@kzu)

## [v1.2.14](https://github.com/devlooped/ThisAssembly/tree/v1.2.14) (2023-04-22)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.13...v1.2.14)

:sparkles: Implemented enhancements:

- Add PrepareProjectProperties as a public hook [\#226](https://github.com/devlooped/ThisAssembly/pull/226) (@kzu)

:bug: Fixed bugs:

- Assembly location can be null, i.e. Blazor WebAssembly [\#227](https://github.com/devlooped/ThisAssembly/pull/227) (@kzu)

## [v1.2.13](https://github.com/devlooped/ThisAssembly/tree/v1.2.13) (2023-04-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.12...v1.2.13)

:bug: Fixed bugs:

- Allow using ThisAssembly.Resources and ThisAssembly.Strings SxS [\#225](https://github.com/devlooped/ThisAssembly/pull/225) (@kzu)

## [v1.2.12](https://github.com/devlooped/ThisAssembly/tree/v1.2.12) (2023-03-22)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.11...v1.2.12)

:sparkles: Implemented enhancements:

- Only run targets when necessary and no earlier [\#221](https://github.com/devlooped/ThisAssembly/pull/221) (@kzu)
- Remove dependency on Prerequisites package [\#219](https://github.com/devlooped/ThisAssembly/pull/219) (@kzu)

:twisted_rightwards_arrows: Merged:

- Remove obsolete \[Generator\] attribute on SponsorLink [\#220](https://github.com/devlooped/ThisAssembly/pull/220) (@kzu)

## [v1.2.11](https://github.com/devlooped/ThisAssembly/tree/v1.2.11) (2023-03-22)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.10...v1.2.11)

:bug: Fixed bugs:

- Don't warn compiler API version for F\# projects [\#216](https://github.com/devlooped/ThisAssembly/pull/216) (@kzu)

:twisted_rightwards_arrows: Merged:

- +Mᐁ includes [\#215](https://github.com/devlooped/ThisAssembly/pull/215) (@devlooped-bot)

## [v1.2.10](https://github.com/devlooped/ThisAssembly/tree/v1.2.10) (2023-03-20)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.9...v1.2.10)

:sparkles: Implemented enhancements:

- Bump to latest SponsorLink and ensure private assets always [\#214](https://github.com/devlooped/ThisAssembly/pull/214) (@kzu)

:bug: Fixed bugs:

- ThisAssembly should not propagate transitively by default [\#212](https://github.com/devlooped/ThisAssembly/pull/212) (@kzu)
- Fix build issue when string resource has newlines [\#208](https://github.com/devlooped/ThisAssembly/pull/208) (@kzu)

## [v1.2.9](https://github.com/devlooped/ThisAssembly/tree/v1.2.9) (2023-02-18)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.8...v1.2.9)

:sparkles: Implemented enhancements:

- 💜 Bump SponsorLink for better privacy [\#198](https://github.com/devlooped/ThisAssembly/pull/198) (@kzu)

## [v1.2.8](https://github.com/devlooped/ThisAssembly/tree/v1.2.8) (2023-02-16)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.7...v1.2.8)

:sparkles: Implemented enhancements:

- Bump to latest SponsorLink [\#197](https://github.com/devlooped/ThisAssembly/pull/197) (@kzu)

## [v1.2.7](https://github.com/devlooped/ThisAssembly/tree/v1.2.7) (2023-02-15)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.6...v1.2.7)

:sparkles: Implemented enhancements:

- Bump to shorter network timeout \(250ms\) on sponsorlink check [\#196](https://github.com/devlooped/ThisAssembly/pull/196) (@kzu)

## [v1.2.6](https://github.com/devlooped/ThisAssembly/tree/v1.2.6) (2023-02-11)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.5...v1.2.6)

:sparkles: Implemented enhancements:

- Only surface build and analyzer from SponsorLink [\#195](https://github.com/devlooped/ThisAssembly/pull/195) (@kzu)

:bug: Fixed bugs:

- Fix missing first character of local branch name [\#192](https://github.com/devlooped/ThisAssembly/pull/192) (@kzu)

:twisted_rightwards_arrows: Merged:

- Enable local testing of nugetizer [\#194](https://github.com/devlooped/ThisAssembly/pull/194) (@kzu)
- Force no quiet days for local testing in debug builds [\#193](https://github.com/devlooped/ThisAssembly/pull/193) (@kzu)

## [v1.2.5](https://github.com/devlooped/ThisAssembly/tree/v1.2.5) (2023-02-07)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.4...v1.2.5)

:twisted_rightwards_arrows: Merged:

- +Mᐁ includes [\#190](https://github.com/devlooped/ThisAssembly/pull/190) (@devlooped-bot)

## [v1.2.4](https://github.com/devlooped/ThisAssembly/tree/v1.2.4) (2023-02-06)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.3...v1.2.4)

:sparkles: Implemented enhancements:

- Bump to CDN-optimized SponsorLink [\#189](https://github.com/devlooped/ThisAssembly/pull/189) (@kzu)

## [v1.2.3](https://github.com/devlooped/ThisAssembly/tree/v1.2.3) (2023-02-04)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.2...v1.2.3)

:sparkles: Implemented enhancements:

- ThisAssembly.Git should add comment when properties are empty [\#179](https://github.com/devlooped/ThisAssembly/issues/179)
- Downgrade CodeAnalysis dependency to allow running on older compilers [\#187](https://github.com/devlooped/ThisAssembly/pull/187) (@kzu)
- Improve detection of supported SourceLink Git provider [\#185](https://github.com/devlooped/ThisAssembly/pull/185) (@kzu)

## [v1.2.2](https://github.com/devlooped/ThisAssembly/tree/v1.2.2) (2023-02-03)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.1...v1.2.2)

:sparkles: Implemented enhancements:

- Improve experience with empty values before first build [\#181](https://github.com/devlooped/ThisAssembly/pull/181) (@kzu)

## [v1.2.1](https://github.com/devlooped/ThisAssembly/tree/v1.2.1) (2023-01-28)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.0...v1.2.1)

:sparkles: Implemented enhancements:

- Bump to more optimal sponsorlink checks [\#180](https://github.com/devlooped/ThisAssembly/pull/180) (@kzu)

:bug: Fixed bugs:

- Duplicate ProjectProperty items cause build failure [\#172](https://github.com/devlooped/ThisAssembly/issues/172)

## [v1.2.0](https://github.com/devlooped/ThisAssembly/tree/v1.2.0) (2023-01-28)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.0-rc.1...v1.2.0)

:sparkles: Implemented enhancements:

- Deduplicate project properties before generation [\#178](https://github.com/devlooped/ThisAssembly/pull/178) (@kzu)
- Don't generate duplicate code for localized resources [\#177](https://github.com/devlooped/ThisAssembly/pull/177) (@kzu)
- When building locally, default branch to HEAD [\#176](https://github.com/devlooped/ThisAssembly/pull/176) (@kzu)
- Add ThisAssembly.Git to ThisAssembly meta-package [\#174](https://github.com/devlooped/ThisAssembly/pull/174) (@kzu)

:bug: Fixed bugs:

- ThisAssembly.Strings doesn't cope well with localized resx [\#173](https://github.com/devlooped/ThisAssembly/issues/173)
- Fix missing source link information in ThisAssembly.Git [\#175](https://github.com/devlooped/ThisAssembly/pull/175) (@kzu)

## [v1.2.0-rc.1](https://github.com/devlooped/ThisAssembly/tree/v1.2.0-rc.1) (2023-01-28)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.0-rc...v1.2.0-rc.1)

:twisted_rightwards_arrows: Merged:

- +Mᐁ includes [\#171](https://github.com/devlooped/ThisAssembly/pull/171) (@devlooped-bot)

## [v1.2.0-rc](https://github.com/devlooped/ThisAssembly/tree/v1.2.0-rc) (2023-01-25)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.2.0-beta...v1.2.0-rc)

:sparkles: Implemented enhancements:

- Add ThisAssembly.Git [\#69](https://github.com/devlooped/ThisAssembly/issues/69)
- Add ThisAssembly.Git, leveraging Microsoft.SourceLink and ThisAssembly.Constants [\#169](https://github.com/devlooped/ThisAssembly/pull/169) (@kzu)
- Allow constants to request a root area different than Constants [\#168](https://github.com/devlooped/ThisAssembly/pull/168) (@kzu)
- Improve Path Sanitization [\#161](https://github.com/devlooped/ThisAssembly/pull/161) (@viceroypenguin)

:bug: Fixed bugs:

- Compilation fails when resource file is name `Resources.resx` [\#167](https://github.com/devlooped/ThisAssembly/issues/167)

:twisted_rightwards_arrows: Merged:

- +Mᐁ includes [\#170](https://github.com/devlooped/ThisAssembly/pull/170) (@devlooped-bot)

## [v1.2.0-beta](https://github.com/devlooped/ThisAssembly/tree/v1.2.0-beta) (2023-01-24)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.1.3...v1.2.0-beta)

:sparkles: Implemented enhancements:

- Add SponsorLink [\#166](https://github.com/devlooped/ThisAssembly/pull/166) (@kzu)

## [v1.1.3](https://github.com/devlooped/ThisAssembly/tree/v1.1.3) (2023-01-10)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.1.2...v1.1.3)

:sparkles: Implemented enhancements:

- Additional sanitizing at template rendering time [\#160](https://github.com/devlooped/ThisAssembly/pull/160) (@kzu)

:bug: Fixed bugs:

- Problems with Resources generator generated code [\#155](https://github.com/devlooped/ThisAssembly/issues/155)

## [v1.1.2](https://github.com/devlooped/ThisAssembly/tree/v1.1.2) (2023-01-10)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.1.1...v1.1.2)

:bug: Fixed bugs:

- Coverlet doesn't collect coverage for modules referencing ThisAssembly.\* [\#159](https://github.com/devlooped/ThisAssembly/issues/159)
- Embedded resources with the same filename but different extensions will create collisions [\#156](https://github.com/devlooped/ThisAssembly/issues/156)
- ThisAssembly.Resources Improvements [\#157](https://github.com/devlooped/ThisAssembly/pull/157) (@viceroypenguin)

## [v1.1.1](https://github.com/devlooped/ThisAssembly/tree/v1.1.1) (2023-01-06)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.1.0...v1.1.1)

:sparkles: Implemented enhancements:

- Include readme in each package, aggregate on project [\#153](https://github.com/devlooped/ThisAssembly/pull/153) (@kzu)
- Ensure incremental source generators are supported [\#152](https://github.com/devlooped/ThisAssembly/pull/152) (@kzu)

:bug: Fixed bugs:

- Do not test LangVersion [\#101](https://github.com/devlooped/ThisAssembly/issues/101)
- Fix ThisAssembly.Resources assembly name for analyzer [\#149](https://github.com/devlooped/ThisAssembly/pull/149) (@kzu)
- Fix logic for determining default text resources [\#148](https://github.com/devlooped/ThisAssembly/pull/148) (@kzu)

:twisted_rightwards_arrows: Merged:

- +Mᐁ includes [\#154](https://github.com/devlooped/ThisAssembly/pull/154) (@devlooped-bot)

## [v1.1.0](https://github.com/devlooped/ThisAssembly/tree/v1.1.0) (2022-12-31)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.10...v1.1.0)

:sparkles: Implemented enhancements:

- Add ThisAssembly.Resources [\#45](https://github.com/devlooped/ThisAssembly/issues/45)
- Add ThisAssembly.Resources [\#134](https://github.com/devlooped/ThisAssembly/pull/134) (@viceroypenguin)
- Modernize to Incremental Source Generators [\#132](https://github.com/devlooped/ThisAssembly/pull/132) (@viceroypenguin)

:bug: Fixed bugs:

- Conflicts with ThisAssembly produced by Nerdbank.GitVersioning [\#124](https://github.com/devlooped/ThisAssembly/issues/124)

:twisted_rightwards_arrows: Merged:

- ⛙ ⬆️ Bump dependencies [\#145](https://github.com/devlooped/ThisAssembly/pull/145) (@github-actions[bot])
- Remove unused files and shared ones [\#138](https://github.com/devlooped/ThisAssembly/pull/138) (@kzu)
- Build on all supported OSes [\#136](https://github.com/devlooped/ThisAssembly/pull/136) (@kzu)

## [v1.0.10](https://github.com/devlooped/ThisAssembly/tree/v1.0.10) (2022-10-18)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.9...v1.0.10)

:sparkles: Implemented enhancements:

- If resource name contains an invalid identifier character, replace it with underscore [\#125](https://github.com/devlooped/ThisAssembly/issues/125)

:bug: Fixed bugs:

- 'New issue' page URL's [\#108](https://github.com/devlooped/ThisAssembly/issues/108)
- Wrong links in "New issue" page [\#102](https://github.com/devlooped/ThisAssembly/issues/102)
- Packages should not be transitive [\#86](https://github.com/devlooped/ThisAssembly/issues/86)
- Source Generator doesn't work anymore with .NET 6.0.200 SDK [\#85](https://github.com/devlooped/ThisAssembly/issues/85)

:twisted_rightwards_arrows: Merged:

- Fix sample code syntax error in readme.md [\#127](https://github.com/devlooped/ThisAssembly/pull/127) (@makp0)
- Improve support for invalid identifier characters [\#126](https://github.com/devlooped/ThisAssembly/pull/126) (@kzu)
- Build with latest .NET6, no particular version [\#97](https://github.com/devlooped/ThisAssembly/pull/97) (@kzu)
- Remove workaround for 6.0.202 breakage [\#95](https://github.com/devlooped/ThisAssembly/pull/95) (@kzu)
- Add devlooped/.github assets [\#91](https://github.com/devlooped/ThisAssembly/pull/91) (@kzu)
- ⬆️ Bump files with dotnet-file sync [\#90](https://github.com/devlooped/ThisAssembly/pull/90) (@kzu)
- Packages should not be transitive [\#89](https://github.com/devlooped/ThisAssembly/pull/89) (@kzu)

## [v1.0.9](https://github.com/devlooped/ThisAssembly/tree/v1.0.9) (2021-10-21)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.8...v1.0.9)

:bug: Fixed bugs:

- Exception when having a second AssemblyInformationalVersion attribute [\#78](https://github.com/devlooped/ThisAssembly/issues/78)
- Exception when typing out AssemblyInformationalVersion [\#77](https://github.com/devlooped/ThisAssembly/issues/77)

## [v1.0.8](https://github.com/devlooped/ThisAssembly/tree/v1.0.8) (2021-04-29)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.7...v1.0.8)

:bug: Fixed bugs:

- Emitting Description can easily break compilation [\#55](https://github.com/devlooped/ThisAssembly/issues/55)

## [v1.0.7](https://github.com/devlooped/ThisAssembly/tree/v1.0.7) (2021-03-16)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.6...v1.0.7)

:sparkles: Implemented enhancements:

- Make the C\# language check a warning instead of an error [\#51](https://github.com/devlooped/ThisAssembly/issues/51)

## [v1.0.6](https://github.com/devlooped/ThisAssembly/tree/v1.0.6) (2021-03-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.5...v1.0.6)

:bug: Fixed bugs:

- ThisAssembly.Prerequisites fails with VS 16.10 preview [\#50](https://github.com/devlooped/ThisAssembly/issues/50)

## [v1.0.5](https://github.com/devlooped/ThisAssembly/tree/v1.0.5) (2021-01-30)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.4...v1.0.5)

:sparkles: Implemented enhancements:

- Add automatic release notes to releases and changelog link to packages [\#41](https://github.com/devlooped/ThisAssembly/issues/41)

## [v1.0.4](https://github.com/devlooped/ThisAssembly/tree/v1.0.4) (2021-01-27)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.3...v1.0.4)

:sparkles: Implemented enhancements:

- Embed Scriban for reduced size and improved isolation [\#39](https://github.com/devlooped/ThisAssembly/issues/39)
- Use analyzers/dotnet/cs as recommended in the cookbok [\#36](https://github.com/devlooped/ThisAssembly/issues/36)

:bug: Fixed bugs:

- Build fails if FileConstant has absolute path instead of relative dir [\#38](https://github.com/devlooped/ThisAssembly/issues/38)

:twisted_rightwards_arrows: Merged:

- Linked files, embedded scriban and unit tests [\#40](https://github.com/devlooped/ThisAssembly/pull/40) (@kzu)
- Use analyzers/dotnet/cs as recommended in the cookbok [\#37](https://github.com/devlooped/ThisAssembly/pull/37) (@kzu)
- Fix ThisAssembly.Project documentation [\#33](https://github.com/devlooped/ThisAssembly/pull/33) (@0xced)
- ⭮ devlooped/oss + ♡ sponsors [\#29](https://github.com/devlooped/ThisAssembly/pull/29) (@kzu)
- 🖆 Apply kzu/oss template [\#27](https://github.com/devlooped/ThisAssembly/pull/27) (@kzu)

## [v1.0.3](https://github.com/devlooped/ThisAssembly/tree/v1.0.3) (2020-12-15)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.2...v1.0.3)

:sparkles: Implemented enhancements:

- Improve LangVersion detection now that C\# 9.0 is stable [\#26](https://github.com/devlooped/ThisAssembly/issues/26)

## [v1.0.2](https://github.com/devlooped/ThisAssembly/tree/v1.0.2) (2020-12-10)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.1...v1.0.2)

:bug: Fixed bugs:

- 🗄 Repository link to source is broken in package description  [\#25](https://github.com/devlooped/ThisAssembly/issues/25)
- When project uses the unsafe compiler option, generation fails [\#21](https://github.com/devlooped/ThisAssembly/issues/21)

## [v1.0.1](https://github.com/devlooped/ThisAssembly/tree/v1.0.1) (2020-12-10)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0...v1.0.1)

:sparkles: Implemented enhancements:

- Provide changelog information in releases [\#19](https://github.com/devlooped/ThisAssembly/issues/19)

:twisted_rightwards_arrows: Merged:

- ☝ Bump NuGetizer from 0.5.0 to 0.6.0 [\#24](https://github.com/devlooped/ThisAssembly/pull/24) (@kzu)
- 🖆 Apply kzu/oss template via dotnet-file [\#23](https://github.com/devlooped/ThisAssembly/pull/23) (@kzu)
- Create draft releases when tags are pushed, with changelog [\#20](https://github.com/devlooped/ThisAssembly/pull/20) (@kzu)
- No need to shout FILENAMES [\#18](https://github.com/devlooped/ThisAssembly/pull/18) (@kzu)

## [v1.0.0](https://github.com/devlooped/ThisAssembly/tree/v1.0.0) (2020-11-21)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-rc.1...v1.0.0)

:hammer: Other:

- Included in reference list - add 'csharp-sourcegenerator' repo topic? [\#5](https://github.com/devlooped/ThisAssembly/issues/5)

:twisted_rightwards_arrows: Merged:

- Bump dependencies to latest stable .net5 [\#6](https://github.com/devlooped/ThisAssembly/pull/6) (@kzu)

## [v1.0.0-rc.1](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-rc.1) (2020-10-28)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-rc...v1.0.0-rc.1)

## [v1.0.0-rc](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-rc) (2020-10-23)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-beta...v1.0.0-rc)

:twisted_rightwards_arrows: Merged:

- Remove design-time fix, share DebugSourceGenerators property [\#3](https://github.com/devlooped/ThisAssembly/pull/3) (@kzu)
- Add ThisAssembly.Constants  [\#2](https://github.com/devlooped/ThisAssembly/pull/2) (@kzu)

## [v1.0.0-beta](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-beta) (2020-10-15)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-alpha.3...v1.0.0-beta)

## [v1.0.0-alpha.3](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-alpha.3) (2020-10-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-alpha.2...v1.0.0-alpha.3)

## [v1.0.0-alpha.2](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-alpha.2) (2020-10-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-alpha.1...v1.0.0-alpha.2)

## [v1.0.0-alpha.1](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-alpha.1) (2020-10-08)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v1.0.0-alpha...v1.0.0-alpha.1)

:hammer: Other:

- It is working for .NET Core 3.1 ? [\#1](https://github.com/devlooped/ThisAssembly/issues/1)

## [v1.0.0-alpha](https://github.com/devlooped/ThisAssembly/tree/v1.0.0-alpha) (2020-10-03)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v0.10.6...v1.0.0-alpha)

## [v0.10.6](https://github.com/devlooped/ThisAssembly/tree/v0.10.6) (2020-09-21)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/v0.10.5...v0.10.6)

## [v0.10.5](https://github.com/devlooped/ThisAssembly/tree/v0.10.5) (2020-09-21)

[Full Changelog](https://github.com/devlooped/ThisAssembly/compare/08b9d212923a93dd20a62792ed931cbd0030ad04...v0.10.5)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
