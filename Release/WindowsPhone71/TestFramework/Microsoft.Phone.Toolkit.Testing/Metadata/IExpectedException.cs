﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Phone.Testing.Metadata
{
  /// <summary>
  /// An expected exception marker for a test method.
  /// </summary>
  [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "The ExpectedException name is the only clear identifier of this functionality.")]
  public interface IExpectedException
  {
    /// <summary>
    /// Gets the expected exception type.
    /// </summary>
    Type ExceptionType { get; }

    /// <summary>
    /// Gets any message associated with the expected exception object.
    /// </summary>
    string Message { get; }
  }
}
