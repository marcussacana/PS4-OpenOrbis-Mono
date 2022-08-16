// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace SixLabors.ImageSharp
{
    public static class Extensions
    {
        public static void TryGetBuffer(this MemoryStream Stream, out ArraySegment<byte> Buffer)
        {
            Buffer = new ArraySegment<byte>(Stream.ToArray());
        }

        public static unsafe string GetString(this Encoding Encoding, byte* Ptr, int Length)
        {
            byte[] Buffer = new byte[Length];
            for (int i = 0; i < Length; i++)
                Buffer[i] = Ptr[i];

            return Encoding.GetString(Buffer);
        }

        public static bool IsDefined<T>(T Value) where T : struct, Enum
        {
            var Type = typeof(T);
            return Enum.IsDefined(Type, Value);
        }

        public static unsafe void MemoryCopy(byte* source, byte* destination, long destinationSizeInBytes, long sourceBytesToCopy)
        {
            for (int i = 0; i < sourceBytesToCopy && i < destinationSizeInBytes; i++)
            {
                destination[i] = source[i];
            }
        }

        public static dynamic As<TFrom, TTo>(this Vector<TFrom> From) where TFrom : struct where TTo : struct
        {
            var ToType = typeof(TTo);
            if (ToType == typeof(float))
                return Vector.AsVectorSingle(From);
            if (ToType == typeof(double))
                return Vector.AsVectorDouble(From);

            if (ToType == typeof(int))
                return Vector.AsVectorInt32(From);
            if (ToType == typeof(short))
                return Vector.AsVectorInt16(From);
            if (ToType == typeof(long))
                return Vector.AsVectorInt64(From);

            if (ToType == typeof(uint))
                return Vector.AsVectorUInt32(From);
            if (ToType == typeof(ushort))
                return Vector.AsVectorUInt16(From);
            if (ToType == typeof(ulong))
                return Vector.AsVectorUInt64(From);

            throw new NotImplementedException();
        }
    }

    public enum OSPlatform
    {
        OSX, Windows, Unix
    }

    public abstract class FormattableString : IFormattable
    {
        public abstract string Format { get; }

        public abstract object[] GetArguments();

        public abstract int ArgumentCount { get; }

        public abstract object GetArgument(int index);

        public abstract string ToString(IFormatProvider formatProvider);

        string IFormattable.ToString(string ignored, IFormatProvider formatProvider)
        {
            return ToString(formatProvider);
        }
        public static string Invariant(string formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException("formattable");
            }

            return formattable.ToString(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }
    }
}
