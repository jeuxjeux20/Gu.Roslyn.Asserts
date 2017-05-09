﻿namespace Gu.Roslyn.Asserts
{
    using System;

    /// <summary>
    /// Helper for throwing assert exceptions.
    /// </summary>
    internal static class Fail
    {
        private static Action<string> action;

        /// <summary>
        /// Throw an <see cref="AssertException"/> or an exception specific to the testing library used if found.
        /// </summary>
        /// <param name="message">The message explaining why the assertion failed.</param>
        internal static void WithMessage(string message)
        {
            if (action == null)
            {
                var type = Type.GetType(
                    "NUnit.Framework.AssertionException, nunit.framework, Version=3.6.1.0, Culture=neutral, PublicKeyToken=2638cd05610744eb",
                    throwOnError: false);
                if (type == null)
                {
                    action = text => throw new AssertException(text);
                }
                else
                {
                    action = text => throw (Exception)Activator.CreateInstance(type, text);
                }
            }

            action(message);
        }
    }
}
