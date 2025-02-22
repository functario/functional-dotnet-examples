// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Style",
    "CA1812:Avoid uninstantiated internal classes",
    Justification = "Instantiated at runtime",
    Scope = "namespaceanddescendants",
    Target = "~N:Example.WebApi.EndPoints"
)]
[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Consider calling ConfigureAwait on the awaited task",
    Justification = "Uncessary for demo"
)]
