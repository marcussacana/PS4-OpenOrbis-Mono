// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SixLabors
{
    /// <summary>
    /// Provides methods to protect against invalid parameters for a DEBUG build.
    /// </summary>
    [DebuggerStepThrough]
    internal static partial class DebugGuard
    {
        /// <summary>
        /// Ensures that the value is not null.
        /// </summary>
        /// <param name="value">The target object, which cannot be null.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Conditional("DEBUG")]
        public static void NotNull<TValue>(TValue value, string parameterName)
            where TValue : class
        {
            if (value is null)
            {
                ThrowArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Ensures that the target value is not null, empty, or whitespace.
        /// </summary>
        /// <param name="value">The target string, which should be checked against being null or empty.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty or contains only blanks.</exception>
        [Conditional("DEBUG")]
        public static void NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value is null)
            {
                ThrowArgumentNullException(parameterName);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                ThrowArgumentException("Must not be empty or whitespace.", parameterName);
            }
        }

        /// <summary>
        /// Ensures that the specified value is less than a maximum value.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is greater than the maximum value.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeLessThan<TValue>(TValue value, TValue max, string parameterName)
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(max) >= 0)
            {
                ThrowArgumentOutOfRangeException(parameterName, $"Value {value} must be less than {max}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is less than or equal to a maximum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is greater than the maximum value.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeLessThanOrEqualTo<TValue>(TValue value, TValue max, string parameterName)
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(max) > 0)
            {
                ThrowArgumentOutOfRangeException(parameterName, $"Value {value} must be less than or equal to {max}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is greater than a minimum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is less than the minimum value.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeGreaterThan<TValue>(TValue value, TValue min, string parameterName)
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(min) <= 0)
            {
                ThrowArgumentOutOfRangeException(
                    parameterName,
                    $"Value {value} must be greater than {min}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is greater than or equal to a minimum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is less than the minimum value.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeGreaterThanOrEqualTo<TValue>(TValue value, TValue min, string parameterName)
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(min) < 0)
            {
                ThrowArgumentOutOfRangeException(parameterName, $"Value {value} must be greater than or equal to {min}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is greater than or equal to a minimum value and less than
        /// or equal to a maximum value and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is less than the minimum value of greater than the maximum value.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeBetweenOrEqualTo<TValue>(TValue value, TValue min, TValue max, string parameterName)
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                ThrowArgumentOutOfRangeException(parameterName, $"Value {value} must be greater than or equal to {min} and less than or equal to {max}.");
            }
        }

        /// <summary>
        /// Verifies, that the method parameter with specified target value is true
        /// and throws an exception if it is found to be so.
        /// </summary>
        /// <param name="target">The target value, which cannot be false.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="target"/> is false.
        /// </exception>
        [Conditional("DEBUG")]
        public static void IsTrue(bool target, string parameterName, string message)
        {
            if (!target)
            {
                ThrowArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Verifies, that the method parameter with specified target value is false
        /// and throws an exception if it is found to be so.
        /// </summary>
        /// <param name="target">The target value, which cannot be true.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="target"/> is true.
        /// </exception>
        [Conditional("DEBUG")]
        public static void IsFalse(bool target, string parameterName, string message)
        {
            if (target)
            {
                ThrowArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Verifies, that the `source` span has the length of 'minLength', or longer.
        /// </summary>
        /// <typeparam name="T">The element type of the spans.</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> has less than <paramref name="minLength"/> items.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeSizedAtLeast<T>(ReadOnlySpan<T> source, int minLength, string parameterName)
        {
            if (source.Length < minLength)
            {
                ThrowArgumentException($"Span-s must be at least of length {minLength}!", parameterName);
            }
        }

        /// <summary>
        /// Verifies, that the `source` span has the length of 'minLength', or longer.
        /// </summary>
        /// <typeparam name="T">The element type of the spans.</typeparam>
        /// <param name="source">The target span.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> has less than <paramref name="minLength"/> items.
        /// </exception>
        [Conditional("DEBUG")]
        public static void MustBeSizedAtLeast<T>(Span<T> source, int minLength, string parameterName)
        {
            if (source.Length < minLength)
            {
                ThrowArgumentException($"The size must be at least {minLength}.", parameterName);
            }
        }

        /// <summary>
        /// Verifies that the 'destination' span is not shorter than 'source'.
        /// </summary>
        /// <typeparam name="TSource">The source element type.</typeparam>
        /// <typeparam name="TDest">The destination element type.</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="destination">The destination span.</param>
        /// <param name="destinationParamName">The name of the argument for 'destination'.</param>
        [Conditional("DEBUG")]
        public static void DestinationShouldNotBeTooShort<TSource, TDest>(
            ReadOnlySpan<TSource> source,
            Span<TDest> destination,
            string destinationParamName)
        {
            if (destination.Length < source.Length)
            {
                ThrowArgumentException($"Destination span is too short!", destinationParamName);
            }
        }

        /// <summary>
        /// Verifies that the 'destination' span is not shorter than 'source'.
        /// </summary>
        /// <typeparam name="TSource">The source element type.</typeparam>
        /// <typeparam name="TDest">The destination element type.</typeparam>
        /// <param name="source">The source span.</param>
        /// <param name="destination">The destination span.</param>
        /// <param name="destinationParamName">The name of the argument for 'destination'.</param>
        [Conditional("DEBUG")]
        public static void DestinationShouldNotBeTooShort<TSource, TDest>(
            Span<TSource> source,
            Span<TDest> destination,
            string destinationParamName)
        {
            if (destination.Length < source.Length)
            {
                ThrowArgumentException($"Destination span is too short!", destinationParamName);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentException(string message, string parameterName) =>
            throw new ArgumentException(message, parameterName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeException(string parameterName, string message) =>
            throw new ArgumentOutOfRangeException(parameterName, message);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentNullException(string parameterName) =>
            throw new ArgumentNullException(parameterName);
    }
}
