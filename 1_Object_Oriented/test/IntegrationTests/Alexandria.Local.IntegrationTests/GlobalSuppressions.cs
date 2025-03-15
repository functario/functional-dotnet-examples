// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Consider calling ConfigureAwait on the awaited task",
    Justification = "Allowed for test project"
)]
[assembly: SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "Allowed for test project"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Allowed for test project",
    Scope = "namespaceanddescendants",
    Target = "~N:Alexandria.Local.IntegrationTests.Tests"
)]
[assembly: SuppressMessage(
    "Usage",
    "CA1816:Dispose methods should call SuppressFinalize",
    Justification = "Allowed for test project",
    Scope = "namespaceanddescendants",
    Target = "~N:Alexandria.Local.IntegrationTests.Tests"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "Allowed with xunit"
)]
