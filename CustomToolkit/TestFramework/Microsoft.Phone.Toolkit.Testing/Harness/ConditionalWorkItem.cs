﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;

namespace Microsoft.Phone.Testing.Harness
{
  /// <summary>
  /// A test work item that is complete once the condition is asserted.
  /// </summary>
  public class ConditionalWorkItem : WorkItem
  {
    /// <summary>
    /// The conditional delegate.
    /// </summary>
    private Func<bool> _delegate;

    /// <summary>
    /// Construct a new conditional work item.
    /// </summary>
    /// <param name="conditionalMethod">Conditional delegate.</param>
    public ConditionalWorkItem(Func<bool> conditionalMethod)
      : base()
    {
      _delegate = conditionalMethod;
    }

    /// <summary>
    /// Invoke the condition, will continue invoking until 
    /// the condition is false.
    /// </summary>
    /// <returns>Completes the invocation once the condition is true.</returns>
    public override bool Invoke()
    {
      // BUG: test does not fail if an exception is thrown from a 
      // conditional delegate
      if (_delegate() == true)
      {
        this.WorkItemComplete();
      }
      return base.Invoke();
    }
  }
}
