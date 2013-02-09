﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;

namespace Microsoft.Phone.Testing.Harness
{
  /// <summary>
  /// Event information marking the completion of a test run.
  /// </summary>
  public class TestHarnessCompletedEventArgs : EventArgs
  {
    /// <summary>
    /// Gets the final test harness state.
    /// 
    /// This contains the final pass versus fail result at a minimum.
    /// Specific harnesses may extend the type to contain additional
    /// information, logs, scenario counts, or anything else.
    /// </summary>
    public TestHarnessState State { get; private set; }

    /// <summary>
    /// Creates a new TestHarnessCompletedEventArgs.
    /// </summary>
    /// <param name="testHarnessState">The final test harness state.</param>
    public TestHarnessCompletedEventArgs(TestHarnessState testHarnessState)
    {
      State = testHarnessState;
    }
  }
}
