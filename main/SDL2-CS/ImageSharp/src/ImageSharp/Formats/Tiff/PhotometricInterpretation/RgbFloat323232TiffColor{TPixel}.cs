// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System;
using System.Numerics;
using SixLabors.ImageSharp.Formats.Tiff.Utils;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Tiff.PhotometricInterpretation
{
    /// <summary>
    /// Implements the 'RGB' photometric interpretation with 32 bits for each channel.
    /// </summary>
    internal class RgbFloat323232TiffColor<TPixel> : TiffBaseColorDecoder<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        private readonly bool isBigEndian;

        /// <summary>
        /// Initializes a new instance of the <see cref="RgbFloat323232TiffColor{TPixel}" /> class.
        /// </summary>
        /// <param name="isBigEndian">if set to <c>true</c> decodes the pixel data as big endian, otherwise as little endian.</param>
        public RgbFloat323232TiffColor(bool isBigEndian) => this.isBigEndian = isBigEndian;

        /// <inheritdoc/>
        public override void Decode(ReadOnlySpan<byte> data, Buffer2D<TPixel> pixels, int left, int top, int width, int height)
        {
            var color = default(TPixel);
            color.FromScaledVector4(Vector4.Zero);
            int offset = 0;
            byte[] buffer = new byte[4];

            for (int y = top; y < top + height; y++)
            {
                Span<TPixel> pixelRow = pixels.DangerousGetRowSpan(y).Slice(left, width);

                if (this.isBigEndian)
                {
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        data.Slice(offset, 4).CopyTo(buffer);
                        Array.Reverse(buffer);
                        float r = BitConverter.ToSingle(buffer, 0);
                        offset += 4;

                        data.Slice(offset, 4).CopyTo(buffer);
                        Array.Reverse(buffer);
                        float g = BitConverter.ToSingle(buffer, 0);
                        offset += 4;

                        data.Slice(offset, 4).CopyTo(buffer);
                        Array.Reverse(buffer);
                        float b = BitConverter.ToSingle(buffer, 0);
                        offset += 4;

                        var colorVector = new Vector4(r, g, b, 1.0f);
                        color.FromScaledVector4(colorVector);
                        pixelRow[x] = color;
                    }
                }
                else
                {
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        data.Slice(offset, 4).CopyTo(buffer);
                        float r = BitConverter.ToSingle(buffer, 0);
                        offset += 4;

                        data.Slice(offset, 4).CopyTo(buffer);
                        float g = BitConverter.ToSingle(buffer, 0);
                        offset += 4;

                        data.Slice(offset, 4).CopyTo(buffer);
                        float b = BitConverter.ToSingle(buffer, 0);
                        offset += 4;

                        var colorVector = new Vector4(r, g, b, 1.0f);
                        color.FromScaledVector4(colorVector);
                        pixelRow[x] = color;
                    }
                }
            }
        }
    }
}
