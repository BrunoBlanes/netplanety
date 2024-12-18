﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// Supress warnings in migration code
[assembly: SuppressMessage("Style", "IDE0300:Simplify collection initialization", Scope = "namespaceanddescendants", Target = "~N:Netplanety.Identity.Migrations")]
[assembly: SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Scope = "namespaceanddescendants", Target = "~N:Netplanety.Identity.Migrations")]
