# NuCaps

NuCaps produces open capability metadata for the NuGet ecosystem. It reports which privileged operations each package can reach, derived from the package's compiled intermediate language. Records are keyed to [purl](https://github.com/package-url/purl-spec) (package URL) identifiers. The companion command line tool is the reference producer.

> **Status: early.** Nothing here works yet. The repository exists ahead of the code so that its history is public from the first commit.

## Why

A NuGet package can open network connections, start processes, read the file system, or call into native libraries through platform invoke. As of today, none of that has to appear anywhere you can see before you install it.

Capability metadata reports those abilities directly, because it is read out of the compiled code rather than taken from what the author declared. NuCaps never loads and never executes the package it inspects, so a package you do not trust is still safe to analyze.

[Google Capslock](https://github.com/google/capslock) does this for Go. No equivalent exists for .NET, and .NET adds four problems that Go does not have.

- **Platform invoke.** An assembly can declare a call into a native library, and that declaration lives in the metadata rather than in the code.
- **Native payloads.** A package can ship compiled binaries under `runtimes/`. The package that declares the call is often not the package that ships the binary, so attributing a capability needs the dependency graph.
- **Build-time code execution.** MSBuild files inside a package run during build, before any of your own code runs.
- **One package, several assemblies.** A package ships one assembly per target framework, and their capabilities genuinely differ. Some assemblies load native code at run time through `GetProcAddress` and `dlsym` instead of declaring it, so reading the wrong target framework can report no native dependency at all where there are hundreds.

## Shape

This section describes the pieces of the project and what each one is for.

| Piece | What it is |
| --- | --- |
| `NuCaps.Core` | The analysis engine, as a class library that other projects can reference. |
| `NuCaps.Cli` | A .NET tool. It exposes `NuCaps.Core` on the command line and holds no analysis logic of its own. |
| The schema | A versioned record format, keyed to purl. It is a deliverable in its own right and has its own lifecycle. |

The published capability records are the point of the project. The command line tool exists to produce them, and to let you produce them yourself.

## Building and testing

You need the .NET 10 software development kit, and [ShellCheck](https://www.shellcheck.net) for the shell script check.

```bash
script/check
```

That is the single check command. It runs four things:

- ShellCheck over the shell scripts.
- A build, with warnings treated as errors.
- Every test.
- A formatting check against `.editorconfig`.

Continuous integration runs this same script and nothing else, so a green run here is a green run there.

To run only the tests while you work:

```bash
dotnet test
```

## Decisions

[`docs/decisions.md`](docs/decisions.md) records the choices that shape this repository, each with the reason behind it. Read it before proposing a change that reverses one.

## License

Copyright © 2026 Witold Woźniak.

- **NuCaps itself**, meaning everything under `src/`. [Apache 2.0](LICENSE).
- **Capability data and schema.** [CC BY 4.0](LICENSES/data-CC-BY-4.0.txt). Code licenses do not fit data, so the records carry an open data license of their own.

The rest of the repository is infrastructure around the licensed project: the tests, the continuous integration workflows, the build configuration, the shell scripts and the documentation. It ships with the project and you may use it for any purpose. It is not part of the licensed work, and no copyright is claimed over it.

One file is an exception. [`CODE_OF_CONDUCT.md`](CODE_OF_CONDUCT.md) is adapted from the Contributor Covenant and carries [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/), which its own attribution section states.

## Use of generative AI

This project is built with an AI assistant, under a restriction chosen deliberately: **no source code under `src/` is AI-generated.** A human author writes every line of the analysis engine and the command line tool.

The assistant writes everything else in this repository:

- The tests.
- The documentation.
- The issue templates.
- The build and repository configuration, for example the project files, `global.json`, `.editorconfig`, `nuget.config` and the Dependabot configuration.
- The continuous integration workflows.
- The shell scripts.

It is also used for research, design discussion, and code review.

[NLnet's generative AI policy](https://nlnet.nl/foundation/policies/generativeAI/) requires a per-commit provenance log for generated code, and accepts a general description like this one where generative AI is used "only for tasks like testing or creating documentation". Testing produces code as well, which is what shows that the exemption turns on whether the work forms part of the delivered software rather than on whether it has syntax. This project reads it that way, and the list above is what that reading covers. The policy states that a per-commit log is preferred but not required in this case, and this project relies on the general description here instead. The delivered library and command line tool are written by a human.

NLnet's generative AI policy states that purely AI-generated output is not eligible for copyright protection in the European Union, so such code could not be placed under Apache 2.0. `NuCaps.Core` exists to be embedded in other people's projects, which makes a sound license the whole point of it. The infrastructure sits outside the licensed work for the same reason, which the License section above states.

The rules the assistant works under are written down in [`AGENTS.md`](AGENTS.md). `CLAUDE.md` imports that file, because Claude Code reads `CLAUDE.md` and other coding agents read `AGENTS.md`.
