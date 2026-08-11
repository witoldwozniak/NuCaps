# NuCaps

Open capability metadata for the NuGet ecosystem — which privileged operations each package can
reach, derived from its compiled IL and keyed to [purl](https://github.com/package-url/purl-spec)
identifiers — with a CLI as the reference producer.

> **Status: early.** Nothing here works yet. The repository exists ahead of the code so that its
> history is public from the first commit.

## The problem

A NuGet manifest declares a package's dependencies. That declaration is a *claim*, not a fact, and
nothing checks what an assembly is actually **capable of**. A package can open sockets, start
processes, read your filesystem, or bind native code through P/Invoke without any of it appearing
in the metadata a consumer sees.

Capability data closes that gap. It is derived by reading the compiled IL — the package is never
loaded and never executed, so even a hostile one is safe to inspect.

[Google Capslock](https://github.com/google/capslock) does this for Go. There is no .NET
equivalent, and .NET has wrinkles Go does not: P/Invoke, native payloads under `runtimes/`,
build-time code injection through MSBuild, and one package shipping several assemblies whose
capabilities genuinely differ per target framework.

## Shape

| Piece | What it is |
| --- | --- |
| `NuCaps.Core` | The analysis engine, as an embeddable class library. |
| `NuCaps.Cli` | A dotnet tool. Thin shell over Core. |
| The schema | A versioned record format, keyed to purl. A deliverable in its own right, not a CLI detail. |

The dataset is the product. The CLI is the machine that makes it.

## License

Copyright © 2026 Witold Woźniak.

- **Code** — [Apache 2.0](LICENSE).
- **Capability data and schema** — [CC BY 4.0](LICENSES/data-CC-BY-4.0.txt). Code licences do not
  fit data, so the records carry an open-data license of their own.

## Use of generative AI

This project is built with an AI assistant, under a restriction chosen deliberately: **no source
code under `src/` is AI-generated.** Every line of the analysis engine and the CLI is written by a
human author.

The assistant is used for research, design discussion, code review, and for writing tests,
documentation, build configuration and CI workflows. Those uses are disclosed here rather than
logged per commit, which is what
[NLnet's GenAI policy](https://nlnet.nl/foundation/policies/generativeAI/) asks for when
generative AI is not used to generate code.

The rules the assistant works under are written down in [`CLAUDE.md`](CLAUDE.md).
