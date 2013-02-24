﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.Phone.Testing.Metadata;

namespace Microsoft.Phone.Testing.Harness
{
  /// <summary>
  /// The IProvideDynamicTestMethods interface is used to provide additional
  /// test methods dynamically at runtime.
  /// </summary>
  public interface IProvideDynamicTestMethods
  {
    /// <summary>
    /// Get the dynamic test methods.
    /// </summary>
    /// <returns>Sequence of dynamic test methods.</returns>
    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does more work than a property should.")]
    IEnumerable<ITestMethod> GetDynamicTestMethods();
  }
}
