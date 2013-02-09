﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;

namespace Microsoft.Phone.Testing.Harness
{
  /// <summary>
  /// Well-known keys that can be used to mark decorator instances in log 
  /// message objects.
  /// </summary>
  public enum UnitTestLogDecorator
  {
    /// <summary>
    /// Indicates that the message is specific to the unit test system.
    /// </summary>
    IsUnitTestMessage,

    /// <summary>
    /// The unit test harness reference.
    /// </summary>
    UnitTestHarness,

    /// <summary>
    /// The metadata interfacing object for a test assembly.
    /// </summary>
    TestAssemblyMetadata,

    /// <summary>
    /// The metadata interfacing object for a test class.
    /// </summary>
    TestClassMetadata,

    /// <summary>
    /// The metadata interfacing object for a test method.
    /// </summary>
    TestMethodMetadata,

    /// <summary>
    /// Indicates that the incorrect exception was intercepted.
    /// </summary>
    IncorrectExceptionMessage,

    /// <summary>
    /// Indicates that the message indicates a skipped/ignored item.
    /// </summary>
    IgnoreMessage,

    /// <summary>
    /// The type of the expected exception.
    /// </summary>
    ExpectedExceptionType,

    /// <summary>
    /// The type of the actual exception.
    /// </summary>
    ActualExceptionType,

    /// <summary>
    /// The actual exception.
    /// </summary>
    ActualException,

    /// <summary>
    /// A TestRunFilter object.
    /// </summary>
    TestRunFilter,

    /// <summary>
    /// A ScenarioResult object.
    /// </summary>
    ScenarioResult,
  }
}
