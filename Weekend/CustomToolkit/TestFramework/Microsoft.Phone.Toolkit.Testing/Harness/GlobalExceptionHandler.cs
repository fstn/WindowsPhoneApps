﻿// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

// ---
// Skip source-level analysis for this single file
// <auto-generated />
//  ---

using System;
using System.Windows;

namespace Microsoft.Phone.Testing.Harness
{
  /// <summary>
  /// Provides a property that will attach and detach a known event handler
  /// delegate when the bit is flipped.
  /// </summary>
  public class GlobalExceptionHandler
  {
    /// <summary>
    /// The event to fire when attached.
    /// </summary>
    private EventHandler _eventHandler;

    /// <summary>
    /// Whether the event handler is attached as a global unhandled
    /// exception handler.
    /// </summary>
    private bool _attached;

    /// <summary>
    /// Creates a new exception handler "manager" with the provided
    /// EventHandler.
    /// </summary>
    /// <param name="eventHandler">The event handler to manage.</param>
    public GlobalExceptionHandler(EventHandler eventHandler)
    {
      if (eventHandler == null)
      {
        throw new ArgumentNullException("eventHandler");
      }
      _eventHandler = eventHandler;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the handler is currently 
    /// attached to the global exception handler.
    /// </summary>
    public bool AttachGlobalHandler
    {
      get { return _attached; }
      set
      {
        if (value != _attached)
        {
          _attached = value;
          UpdateAttachment();
        }
      }
    }

    /// <summary>
    /// Mark the Handled property in the event args as True to stop any 
    /// event bubbling.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    /// <param name="exceptionHandled">
    /// Value indicating whether the Exception should be marked as handled.
    /// </param>
    public static void ChangeExceptionBubbling(EventArgs e, bool exceptionHandled)
    {
      ApplicationUnhandledExceptionEventArgs args = e as ApplicationUnhandledExceptionEventArgs;
      if (args != null)
      {
        args.Handled = exceptionHandled;
      }
    }

    /// <summary>
    /// Return the Exception property from the EventArgs.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    /// <returns>
    /// Returns the Exception object that the event arguments stores.
    /// </returns>
    public static Exception GetExceptionObject(EventArgs e)
    {
      ApplicationUnhandledExceptionEventArgs args = e as ApplicationUnhandledExceptionEventArgs;
      return (args != null) ? args.ExceptionObject : null;
    }

    /// <summary>
    /// Internal event that is hooked up to the global exception handler.
    /// </summary>
    /// <param name="sender">Source object of the event.</param>
    /// <param name="e">Event arguments.</param>
    private void OnGlobalException(object sender, ApplicationUnhandledExceptionEventArgs e)
    {
      _eventHandler(sender, e);
    }

    /// <summary>
    /// Called after a change to the attachment field value.
    /// </summary>
    private void UpdateAttachment()
    {
      if (_attached)
      {
        AttachHandler();
      }
      else
      {
        DetachHandler();
      }
    }

    /// <summary>
    /// Attach the handler globally.
    /// </summary>
    private void AttachHandler()
    {
      Application.Current.UnhandledException += OnGlobalException;
    }

    /// <summary>
    /// Detach the handler globally.
    /// </summary>
    private void DetachHandler()
    {
      //NOTE: This may not work for console hosted scenarios
      Application.Current.UnhandledException -= OnGlobalException;
    }
  }
}
