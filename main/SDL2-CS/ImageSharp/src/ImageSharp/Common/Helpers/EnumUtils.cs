// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System;
using System.Runtime.CompilerServices;

namespace SixLabors.ImageSharp
{
    /// <summary>
    /// Common utility methods for working with enums.
    /// </summary>
    internal static class EnumUtils
    {
        /// <summary>
        /// Converts the numeric representation of the enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">The type of enum </typeparam>
        /// <param name="value">The value to parse</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <returns>The <typeparamref name="TEnum"/>.</returns>
        public static TEnum Parse<TEnum>(int value, TEnum defaultValue)
            where TEnum : struct, Enum
        {
            DebugGuard.IsTrue(Unsafe.SizeOf<TEnum>() == sizeof(int), "Only int-sized enums are supported.");

            TEnum valueEnum = Unsafe.As<int, TEnum>(ref value);
            if (Extensions.IsDefined(valueEnum))
            {
                return valueEnum;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns a value indicating whether the given enum has a flag of the given value.
        /// </summary>
        /// <typeparam name="TEnum">The type of enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="flag">The flag.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool HasFlag<TEnum>(TEnum value, TEnum flag)
          where TEnum : struct, Enum
        {
            DebugGuard.IsTrue(Unsafe.SizeOf<TEnum>() == sizeof(int), "Only int-sized enums are supported.");

            uint flagValue = Unsafe.As<TEnum, uint>(ref flag);
            return (Unsafe.As<TEnum, uint>(ref value) & flagValue) == flagValue;
        }
    }
}
