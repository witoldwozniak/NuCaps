# NuCaps

NuCaps produces open capability metadata for the NuGet ecosystem. It reports which privileged operations each package can reach, derived from the package's compiled intermediate language, which is the instruction set that .NET compilers emit. Records are keyed to [purl](https://github.com/package-url/purl-spec) (package URL) identifiers. A command line tool is the reference producer.

> **Status: early.** Nothing here works yet. The repository exists ahead of the code so that its history is public from the first commit.

## The problem

This section explains what NuCaps measures, and why no existing tool measures it for .NET.

A NuGet package manifest declares which other packages it depends on. That is a claim about dependencies, not a statement about behavior. Nothing checks what the compiled code is able to do.

A package can open network connections, start processes, read your file system, or call into native libraries through platform invoke. None of that has to appear anywhere you can see before you install it.

Capability metadata reports those abilities directly, because it is read out of the compiled code rather than taken from what the author declared. NuCaps never loads and never executes the package it inspects, so a package you do not trust is still safe to analyze.

[Google Capslock](https://github.com/google/capslock) does this for Go. No equivalent exists for .NET, and .NET adds four problems that Go does not have.

- **Platform invoke.** An assembly can declare a call into a native library, and that declaration lives in the metadata rather than in the code.
- **Native payloads.** A package can ship compiled binaries under `runtimes/`. The package that declares the call is often not the package that ships the binary, so attributing a capability needs the dependency graph.
- **Build-time code execution.** MSBuild files inside a package run during your build, before any of your own code runs.
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

- **Code.** [Apache 2.0](LICENSE).
- **Capability data and schema.** [CC BY 4.0](LICENSES/data-CC-BY-4.0.txt). Code licenses do not fit data, so the records carry an open data license of their own.

## Use of generative AI

This project is built with an AI assistant, under a restriction chosen deliberately: **no source code under `src/` is AI-generated.** A human author writes every line of the analysis engine and the command line tool.

The assistant is used for research, design discussion, code review, and for writing tests, documentation, build configuration and continuous integration workflows. Those uses are disclosed here rather than logged in every commit, which is what [NLnet's generative AI policy](https://nlnet.nl/foundation/policies/generativeAI/) asks for when generative AI is not used to generate code.

The rules the assistant works under are written down in [`CLAUDE.md`](CLAUDE.md).
