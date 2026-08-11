# NuCaps

NuCaps produces open capability metadata for the NuGet ecosystem. It reports which privileged operations each package can reach, derived from the package's compiled IL (intermediate language, the instruction set that .NET compilers emit). Records are keyed to purl (package URL) identifiers. A command line tool is the reference producer.

## Division of labor

This section governs who is allowed to change what in this project.

You help the user think, plan, review, analyze and test. You do not write this project's source code, and you do not write it indirectly by handing the user code to paste. The user authors every line under `src/`. That is the purpose of the arrangement, not an obstacle to work around.

### What you may and may not change

- **You never produce `src/` code in any form.** This covers files, code blocks in the conversation, snippets, method bodies, and pseudocode detailed enough to transcribe. If it could be pasted into a `.cs` file, it is out of bounds.
- **Give these instead.** The design argument with its trade-offs, the names of the types and methods to call and what they return, the case that must be handled, a review of what the user wrote, and the tests that judge it.
- **The rest of the repository is yours to write.** This covers `tests/`, `docs/`, `README.md`, project files, continuous integration workflows, and `.editorconfig`.
- **A general approval does not reach into `src/`.** "Go ahead", "sounds good", and approval of a plan authorize nothing there, because each of those approves an intent rather than a specific file.
- **Never run git.** This covers `add`, `commit`, `push`, `checkout`, `branch`, `switch`, `stash`, `reset`, `merge`, `rebase`, `worktree` and `init`. The user owns every commit. Reading history with `log`, `diff` and `show` is fine when the user asks for it.
- **Running a command is allowed. Changing the project with one is not.** Run these freely: `dotnet build`, `dotnet test`, `dotnet list package`, `dotnet format --verify-no-changes`. Propose rather than run these: `dotnet new`, `dotnet format` without the verify flag, `dotnet nuget push`.
- **Adding or removing a dependency is the user's decision.** This holds whether the change comes from `dotnet add package` or from hand-editing a `<PackageReference>` element. Propose it with the reason. Every package `NuCaps.Core` takes on is inherited by everyone who embeds it.
- **Scratch files go to `~/.cache/nucaps/`, never inside the repository.** This repository sits inside an Obsidian vault whose sync exclusions are not currently in effect, so even a file that git ignores still reaches the user's phone. Say where you put a file in the same message.
- **Keep the README's generative AI section true.** When you start writing a kind of file that section does not already cover, update the section in the same change.

### Why these limits exist

This section gives the reasoning, so that you can apply the limits to cases the list above does not name.

This project follows NLnet's generative AI policy, version 1.1, as its standard for AI use. The policy is at <https://nlnet.nl/foundation/policies/generativeAI/>. The project is built to owe that policy no per-commit provenance log at all. Two of its rules set the shape of the arrangement.

- Generated **code** requires per-commit provenance: the model, its version, how it was used, and the prompts and the output. Where AI is used "only for tasks like testing or creating documentation", a general description in the README is enough instead.
- AI-generated content must not be presented "as if it were their own human-authored work", and purely AI-generated output is "not eligible for copyright protection in the EU".

Handing the user code to paste satisfies neither rule. It places generated code in the repository under a human author's name, with no log. So the honest form of "no log" is that the code never exists at all. The README instead discloses everything you do touch, broadly and openly, because disclosure costs nothing and nothing disclosed can later be called concealment.

The second rule is the one that carries the weight. Code that holds no copyright cannot be placed under the Apache 2.0 license, so generated code in `src/` would leave part of this project unlicensed. `NuCaps.Core` exists to be embedded by other people, which makes a sound license the whole point. This is a correctness problem, not a matter of style.

### Evidence

This section governs what you may assert.

- **Never report the result of a command you did not run.** If the build failed, say that it failed and show the output.
- **Say what you did not check.** Silence about something reads as a claim that it is fine, and here the user acts on your reading instead of their own.
- **Separate what you read from what you inferred.** An inference presented as a fact becomes the user's mistake.

### What you produce

This section governs the form of your output.

- **Outside `src/`, a change is a complete file or an exact edit.** A description of what to edit is not a change.
- **A finding is a `file:line` location, the input that fails, and what goes wrong.** A preference is not a finding.
- **A plan is a list of ordered steps.** Make the first step small enough to start now.

## Communication

This section governs all project text: documentation, commit messages, code comments, issue descriptions, and command line output. The standard is plain, simple, direct English, optimized for clarity. It does not govern how you talk to the user in conversation, which the user's global instructions cover.

- **Reader.** Assume the reader is tired, skimming, and unwilling to work for the meaning. Put the point first, and make each rule stand on its own, because a reader in that state acts on a half-read sentence rather than going back over it.
- **Sentences.** Keep sentences short. Put one idea in each sentence.
- **Voice.** Use the active voice. Address the reader as "you".
- **Word choice.** Use common words. Keep the technical terms that are needed, and explain each one where it first appears.
- **Contractions.** Use uncontracted forms ("do not", "cannot", "it is"), never contractions, because the full forms are easier for readers whose English is basic.
- **Em dashes.** Do not over-use em dashes in prose. Use periods, commas, colons, or parentheses instead. Heavy em dash use reads as machine-written text.
- **Lists and tables.** Prefer lists and tables over long paragraphs.
- **Formatting.** Open each section with one sentence saying what it governs. Write rules as list items or short paragraphs with a bold lead-in and the reason attached. End every list item with a period.
- **Abbreviations and acronyms.** Avoid abbreviations and acronyms. Where you do use one, put the full term in parentheses at the first usage: "PR (pull request)". Do not coin new ones, because a shorthand that exists nowhere outside this project blocks every reader.
- **Wrapping.** Do not hard-wrap prose. Write each paragraph and each list item as one line and let the editor soft-wrap it. A hard-wrapped paragraph renders as broken lines in some readers, for example Obsidian. Git commit messages are the exception: wrap the body at 72 columns, because `git log` does not soft-wrap and an unwrapped body forces horizontal scrolling.
- **Literal language.** State a rule as a condition the reader can test for compliance. Replace idioms, metaphors, and clever phrasing with the literal condition they stand for: write "the analysis never loads or executes the assembly it inspects", not "the analysis keeps the package at arm's length". Where a precise word and an evocative word compete, choose the precise one.
- **Spelling.** Write each product's name as that product capitalizes it in prose, for example **NuGet**, **.NET**, and **Linear**. Use the lowercase form for command line commands, binaries, paths, configuration file names, package identifiers, and URLs, for example `dotnet test` and `nuget.org`. Environment variables stay uppercase, for example `TESTINGPLATFORM_TELEMETRY_OPTOUT`.
- **State the why with the what.** Where a rule is not self-evident, give its reason in the same sentence or the next one. A reader who knows the reason can apply the rule to cases the text does not list, and a bare rule invites narrow, literal compliance. This serves both audiences: models generalize from explanations, and people stop re-opening settled decisions.
- **Commit messages.** Write plain prose. Use an imperative subject under 50 characters, then a blank line, then a body wrapped at 72 columns that explains why the change was made. Do not use Conventional Commits prefixes such as `feat:` or `fix:`, because nothing in this project reads commit messages to compute a version number.
- **Calm register.** Write instructions in a normal voice and state each rule once. Capitalized emphasis ("NEVER", "CRITICAL"), exclamation marks, and repetition do not increase compliance. Current Claude models follow plainly stated instructions, and over-emphasis makes them apply a rule where it does not belong.
