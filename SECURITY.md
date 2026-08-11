# Security policy

This document tells you how to report a security problem in NuCaps, and what counts as one.

## Status

NuCaps is pre-alpha. No version has been released and no package has been published, so there is nothing deployed for an attacker to reach yet. Report anything you find anyway. A problem found now costs almost nothing to fix, and the same problem found after release costs a great deal.

## How to report

Use private vulnerability reporting on this repository. Open the Security tab and choose "Report a vulnerability". Your report stays private until a fix is published.

Do not open a public issue for a security problem. A public report tells everyone about the weakness before there is anything to update to.

## What to expect

One person maintains this project, part time. Expect a first reply within seven days. If seven days pass with no answer, the report was missed rather than ignored, so please send a reminder.

## What counts as a vulnerability

NuCaps reads compiled code from packages that nobody trusts. Three kinds of problem matter, and the third is specific to what this tool is for.

- **Anything that makes NuCaps run code from a package it analyzes.** The analysis is meant to be entirely static: NuCaps reads a package and never loads or executes it. A way around that is the most serious finding possible here, because it turns an auditing tool into the delivery mechanism for the thing it was auditing.
- **Anything that makes NuCaps crash, hang, or use unbounded memory on a crafted package.** The metadata reader NuCaps depends on is documented as not hardened against malformed input, so this class of problem is expected rather than surprising. Reports are still wanted. Handling malformed input is part of the work, not an excuse for skipping it.
- **Any repeatable way to hide a capability from the analysis.** People act on these reports. A package built so that NuCaps reports no network access while the package does reach the network defeats the entire purpose, even though nothing crashed and no code escaped. Tell us how you did it.

## What does not count

A capability report that is merely wrong or incomplete is a bug. Open a normal issue for it, with the package and version that produced it.

The difference from the third case above is repeatability and intent. A gap anyone can exploit deliberately is a vulnerability. An analysis that misses something because the feature is not built yet is a bug, and the limits of the analysis are documented rather than hidden.
