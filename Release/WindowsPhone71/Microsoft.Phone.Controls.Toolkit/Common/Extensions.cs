﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
    /// <summary>
    /// This set of internal extension methods provide general solutions and 
    /// utilities in a small enough number to not warrant a dedicated extension
    /// methods class.
    /// </summary>
    internal static partial class Extensions
    {
        private const string ExternalAddress = "app://external/";

        /// <summary>
        /// Inverts a Matrix. The Invert functionality on the Matrix type is 
        /// internal to the framework only. Since Matrix is a struct, an out 
        /// parameter must be presented.
        /// </summary>
        /// <param name="m">The Matrix object.</param>
        /// <param name="outputMatrix">The matrix to return by an output 
        /// parameter.</param>
        /// <returns>Returns a value indicating whether the type was 
        /// successfully inverted. If the determinant is 0.0, then it cannot 
        /// be inverted and the original instance will remain untouched.</returns>
        public static bool Invert(this Matrix m, out Matrix outputMatrix)
        {
            double determinant = m.M11 * m.M22 - m.M12 * m.M21;
            if (determinant == 0.0)
            {
                outputMatrix = m;
                return false;
            }

            Matrix matCopy = m;
            m.M11 = matCopy.M22 / determinant;
            m.M12 = -1 * matCopy.M12 / determinant;
            m.M21 = -1 * matCopy.M21 / determinant;
            m.M22 = matCopy.M11 / determinant;
            m.OffsetX = (matCopy.OffsetY * matCopy.M21 - matCopy.OffsetX * matCopy.M22) / determinant;
            m.OffsetY = (matCopy.OffsetX * matCopy.M12 - matCopy.OffsetY * matCopy.M11) / determinant;

            outputMatrix = m;
            return true;
        }

        /// <summary>
        /// An implementation of the Contains member of string that takes in a 
        /// string comparison. The traditional .NET string Contains member uses 
        /// StringComparison.Ordinal.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="value">The string value to search for.</param>
        /// <param name="comparison">The string comparison type.</param>
        /// <returns>Returns true when the substring is found.</returns>
        public static bool Contains(this string s, string value, StringComparison comparison)
        {
            return s.IndexOf(value, comparison) >= 0;
        }

        /// <summary>
        /// Returns whether the page orientation is in portrait.
        /// </summary>
        /// <param name="orientation">Page orientation</param>
        /// <returns>If the orientation is portrait</returns>
        public static bool IsPortrait(this PageOrientation orientation)
        {
            return (PageOrientation.Portrait == (PageOrientation.Portrait & orientation));
        }

        /// <summary>
        /// Returns whether the dark visual theme is currently active.
        /// </summary>
        /// <param name="resources">Resource Dictionary</param>
        public static bool IsDarkThemeActive(this ResourceDictionary resources)
        {
            return ((Visibility)resources["PhoneDarkThemeVisibility"] == Visibility.Visible);
        }

        /// <summary>
        /// Returns whether the uri is from an external source.
        /// </summary>
        /// <param name="uri">The uri</param>
        public static bool IsExternalNavigation(this Uri uri)
        {
            return (ExternalAddress == uri.ToString());
        }

        /// <summary>
        /// Registers a property changed callback for a given property.
        /// </summary>
        /// <param name="element">The element registering the notification</param>
        /// <param name="propertyName">Property name to register</param>
        /// <param name="callback">Callback function</param>
        /// <remarks>This allows a child to be notified of when a property declared in its parent is changed.</remarks>
        public static void RegisterNotification(this FrameworkElement element, string propertyName, PropertyChangedCallback callback)  
        {
            DependencyProperty prop = DependencyProperty.RegisterAttached("Notification" + propertyName,  
                                                                          typeof(object),
                                                                          typeof(FrameworkElement),  
                                                                          new PropertyMetadata(callback));  
          
            element.SetBinding(prop, new Binding(propertyName) { Source = element });  
        }  
    }
}