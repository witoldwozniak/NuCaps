# Decisions

This document records the decisions that shape this repository, each with its reason, and the open questions that will become decisions later. It exists so that a settled question is not argued again from memory, and so that a reader can see why the project looks the way it does. Add an entry when a decision is made. Do not delete entries. If a decision is reversed, add a new one that says so and why. When an open question is settled, move it up into the sections above.

## Analysis

- **Target `net10.0` only. netstandard2.0 is deferred.** `System.Reflection.Metadata` ships inside the .NET 10 shared framework, so reading metadata needs no package reference. On netstandard2.0 the same code needs three packages and a hand-written `IsExternalInit` type before `record` and `init` compile. Revisit this if there is a concrete reason to embed `NuCaps.Core` in a Roslyn analyzer or an MSBuild task, because both of those still run on .NET Framework inside Visual Studio.
- **Read metadata with `System.Reflection.Metadata`, not Mono.Cecil.** Read-only scanning at corpus scale is what `System.Reflection.Metadata` is designed for, and it needs no package reference. Mono.Cecil is stronger at rewriting IL, which this project never does.
- **Analysis is static. NuCaps never loads or executes an assembly it inspects.** This is what makes it safe to analyze a package nobody trusts, and it is why any target framework can be read regardless of what NuCaps itself runs on.
- **Package fetching lives in `NuCaps.Cli`, never in `NuCaps.Core`.** `NuCaps.Core` accepts a package that already exists on disk, by file path or by seekable stream, and opens no network connection. A `.nupkg` file is a zip archive and needs random access, so a forward-only stream is not enough. The command line tool owns `NuGet.Protocol` and everything about resolution, package sources, credentials, caching and retries. There are three reasons. `NuGet.Protocol` pulls in nine packages, including `Newtonsoft.Json`, and every project embedding `NuCaps.Core` would inherit all of them. Untrusted packages have to be parsed in an isolated process, because the metadata reader is not hardened against malformed input, and that process should hold no credentials and open no connections. `NuCaps.Core` is also meant to be embeddable in a Roslyn analyzer or an MSBuild task, and neither of those may reach the network. Adding fetching to `NuCaps.Core` later is possible, while removing it from a published library is a breaking change, so this is the direction that stays reversible.
- **Capability profiles are per target framework, plus a computed union.** A single package ships one assembly per target framework and their capabilities genuinely differ. Reporting one arbitrary framework produces wrong answers: some assemblies bind native code through `GetProcAddress` and `dlsym` instead of P/Invoke declarations, so a profile taken from the wrong framework can report no native dependency where there are hundreds.

## Testing

- **Test with TUnit.** Chosen over xUnit v3 and the in-box xUnit v2 template. Source generated, no reflection at runtime, and fast across a large corpus run.
- **`dotnet test` is the single check command.** TUnit runs on Microsoft.Testing.Platform, which the .NET 10 software development kit opts into through `global.json`. The older `TestingPlatformDotnetTestSupport` property is the legacy Visual Studio Test route and the .NET 10 software development kit rejects it.
- **Opt out of test platform telemetry, in `.envrc`.** The `TUnit` package pulls in a telemetry extension. A tool that reports what other packages do should not send usage data itself.

## Licensing and authorship

- **Apache 2.0 for code.** `NuCaps.Core` exists to be embedded, and a copyleft license would block adoption in most corporate .NET environments. Apache 2.0 also carries a patent grant and matches the surrounding ecosystem.
- **CC BY 4.0 for the capability data and the schema.** Code licenses do not fit data. Attribution keeps NuCaps named when its records are used elsewhere.
- **No AI-generated code under `src/`.** Purely AI-generated output carries no copyright in the European Union, so it cannot be placed under Apache 2.0. Generated code in `src/` would leave part of this project unlicensed, which matters most for a library meant to be embedded. `CLAUDE.md` holds the full rule.

## Conventions

- **American English throughout.** Chosen for consistency with .NET and NuGet naming, which is American everywhere.
- **Plain prose commit messages. No Conventional Commits.** An imperative subject under 50 characters, and a body wrapped at 72 columns that explains why. Conventional Commits earns its place when tooling reads commit messages to compute a version number, and nothing here does that.
- **Sign commits and tags with SSH, not GPG.** Signing proves a commit came from someone holding the author's key, which matters if a GitHub token is ever stolen: an attacker can still push, but the commits show as unverified. SSH signing reuses the existing ed25519 key, so there is no second key to expire, back up, or revoke, and GitHub shows the same verified badge either way. GPG would only win if verification were needed outside GitHub through a web of trust. Signing covers the source history only. Provenance for published packages is a separate matter, handled by trusted publishing.
- **Semantic versioning, derived from git tags rather than from commit messages.** Semantic versioning is required of a published NuGet package. Deriving it from tags, with a tool such as MinVer or Nerdbank.GitVersioning, gives the same result without imposing a format on every commit. Decide the tool when publishing is set up.

## Open questions

These are not decided yet. Each one is recorded here so that it gets decided deliberately rather than by accident, on the day someone first needs an answer.

- **How should the analysis be isolated when it runs over untrusted packages at scale?** `PEReader` and `MetadataReader` are not hardened against malformed input, and Microsoft documents out-of-bounds reads, crashes and hangs. So a package designed to attack the reader can take down whatever process reads it. The mitigation is to parse in an isolated process that can be killed, but the shape of that isolation is open. An operating system process with time and memory limits is the cheap answer. A purpose-built container with no network access, a read-only file system, and hard resource limits is the strong answer, and it costs an image to build, host, scan and keep patched, which is real surface for a project whose subject is supply-chain risk. This becomes pressing at corpus scale, where thousands of packages are analyzed unattended, and it does not need answering before then. It is a separate question from the continuous integration runner image, which needs no customization.
