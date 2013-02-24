﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;
using Microsoft.Phone.Testing.Metadata;

namespace Microsoft.Phone.Testing.Harness
{
  /// <summary>
  /// A container that stores instances of the unit test harness and provider.
  /// </summary>
  public abstract class UnitTestCompositeWorkItem : CompositeWorkItem
  {
    /// <summary>
    /// The unit test provider.
    /// </summary>
    private IUnitTestProvider _provider;

    /// <summary>
    /// Initializes a new unit test work item container.
    /// </summary>
    /// <param name="testHarness">The unit test harness.</param>
    /// <param name="unitTestProvider">The unit test metadata provider.</param>
    protected UnitTestCompositeWorkItem(UnitTestHarness testHarness, IUnitTestProvider unitTestProvider)
      : base()
    {
      _provider = unitTestProvider;
      TestHarness = testHarness;
      if (TestHarness == null)
      {
        throw new InvalidOperationException(Properties.UnitTestMessage.UnitTestCompositeWorkItem_ctor_NoTestHarness);
      }
    }

    /// <summary>
    /// Logs a new message.
    /// </summary>
    /// <param name="message">Message object.</param>
    protected void LogMessage(LogMessage message)
    {
      LogWriter.Enqueue(message);
    }

    /// <summary>
    /// Logs a message about the harness.
    /// </summary>
    /// <param name="harnessInformation">Information about the harness.</param>
    protected void LogMessage(string harnessInformation)
    {
      LogWriter.TestExecution(harnessInformation);
    }

    /// <summary>
    /// Gets the log message writer for the unit test system.
    /// </summary>
    protected UnitTestLogMessageWriter LogWriter
    {
      get { return TestHarness.LogWriter; }
    }

    /// <summary>
    /// Gets the test harness instance.
    /// </summary>
    public UnitTestHarness TestHarness
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets the unit test provider instance.
    /// </summary>
    protected IUnitTestProvider Provider
    {
      get { return _provider; }
    }
  }
}
