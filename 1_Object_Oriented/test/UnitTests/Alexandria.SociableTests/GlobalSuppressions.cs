// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Naming",
    "CS5001:Identifiers should not contain underscores",
    Justification = "Allow by test naming convention for readability"
)]
[assembly: SuppressMessage(
    "Design",
    "CA1062:Validate arguments of public methods",
    Justification = "<Pending>",
    Scope = "namespaceanddescendants",
    Target = "~N:Alexandria.SociableTests"
)]
